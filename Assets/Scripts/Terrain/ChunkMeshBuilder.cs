using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public static class ChunkMeshBuilder
{
    public static readonly int2 AtlasSize = new int2(16, 16); // 替换为你的纹理图集大小

    public static void InitializeShaderParameter()
    {
        Shader.SetGlobalInt("_AtlasX", AtlasSize.x);
        Shader.SetGlobalInt("_AtlasY", AtlasSize.y);
        Shader.SetGlobalVector("_AtlasRec", new Vector4(1.0f / AtlasSize.x, 1.0f / AtlasSize.y));
    }

    public class NativeMeshData
    {
        public NativeArray<float3> nativeVertices;
        public NativeArray<float3> nativeNormals;
        public NativeArray<int> nativeIndices;
        public NativeArray<float4> nativeUVs;
        public JobHandle jobHandle;
        NativeCounter faceCounter;

        public NativeMeshData(int2 chunkSize)
        {
            int numCells = chunkSize.x * chunkSize.y;
            int maxVertices = 4 * numCells * 4; // 每个体素4个面，每个面4个顶点
            int maxIndices = 6 * numCells * 4; // 每个体素4个面，每个面6个索引

            nativeVertices = new NativeArray<float3>(maxVertices, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeNormals = new NativeArray<float3>(maxVertices, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeUVs = new NativeArray<float4>(maxVertices, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            nativeIndices = new NativeArray<int>(maxIndices, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            faceCounter = new NativeCounter(Allocator.TempJob);
        }

        ~NativeMeshData()
        {
            jobHandle.Complete();
            Dispose();
        }

        public void Dispose()
        {
            if (nativeVertices.IsCreated)
                nativeVertices.Dispose();

            if (nativeNormals.IsCreated)
                nativeNormals.Dispose();

            if (nativeIndices.IsCreated)
                nativeIndices.Dispose();

            if (nativeUVs.IsCreated)
                nativeUVs.Dispose();

            if (faceCounter.IsCreated)
                faceCounter.Dispose();
        }

        public IEnumerator ScheduleMeshingJob(int[,] heightMap, int2 chunkSize)
        {
            NativeArray<int> flatHeightMap = new NativeArray<int>(chunkSize.x * chunkSize.y, Allocator.TempJob);
            for (int x = 0; x < chunkSize.x; x++)
            {
                for (int z = 0; z < chunkSize.y; z++)
                {
                    flatHeightMap[x + z * chunkSize.x] = heightMap[x, z];
                }
            }

            ScheduleMeshingJobNative(flatHeightMap, chunkSize);

            int frameCount = 0;
            yield return new WaitUntil(() =>
            {
                frameCount++;
                return jobHandle.IsCompleted || frameCount >= 4;
            });

            jobHandle.Complete();
            flatHeightMap.Dispose();
        }

        void ScheduleMeshingJobNative(NativeArray<int> heightMap, int2 chunkSize)
        {
            VoxelGreedyMeshingJob voxelMeshingJob = new VoxelGreedyMeshingJob
            {
                heightMap = heightMap,
                chunkSize = chunkSize,
                vertices = nativeVertices,
                normals = nativeNormals,
                uvs = nativeUVs,
                indices = nativeIndices,
                faceCounter = faceCounter
            };

            jobHandle = voxelMeshingJob.Schedule();
            JobHandle.ScheduleBatchedJobs();
        }

        public void GetMeshInformation(out int vertexSize, out int indexSize)
        {
            vertexSize = faceCounter.Count * 4;
            indexSize = faceCounter.Count * 6;
        }
    }

    [BurstCompile]
    struct VoxelGreedyMeshingJob : IJob
    {
        [ReadOnly] public NativeArray<int> heightMap;
        [ReadOnly] public int2 chunkSize;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float3> vertices;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float3> normals;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<float4> uvs;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<int> indices;

        [WriteOnly] public NativeCounter faceCounter;

        public void Execute()
        {
            int width = chunkSize.x;
            int height = chunkSize.y;

            // 使用 NativeArray 来存储体素的顶点和索引数据
            NativeArray<float3> voxelVertices = new NativeArray<float3>(4, Allocator.Temp);
            NativeArray<int> voxelIndices = new NativeArray<int>(6, Allocator.Temp);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    int currentHeight = heightMap[x + z * width];

                    // 生成体素的顶点和索引
                    CreateVoxelMesh(x, z, currentHeight, voxelVertices, voxelIndices);
                }
            }

            voxelVertices.Dispose();
            voxelIndices.Dispose();
        }

        void CreateVoxelMesh(int x, int z, int height, NativeArray<float3> voxelVertices, NativeArray<int> voxelIndices)
        {
            int faceIndex = faceCounter.Increment();

            // 计算顶点和索引
            voxelVertices[0] = new float3(x, height, z); // 顶部左前
            voxelVertices[1] = new float3(x + 1, height, z); // 顶部右前
            voxelVertices[2] = new float3(x + 1, height, z + 1); // 顶部右后
            voxelVertices[3] = new float3(x, height, z + 1); // 顶部左后

            voxelIndices[0] = 0; // 顶部左前
            voxelIndices[1] = 3; // 顶部右前
            voxelIndices[2] = 2; // 顶部右后
            voxelIndices[3] = 2; // 顶部右后
            voxelIndices[4] = 1; // 顶部左后
            voxelIndices[5] = 0; // 顶部左前

            for (int i = 0; i < 4; i++)
            {
                vertices[faceIndex * 4 + i] = voxelVertices[i];
                normals[faceIndex * 4 + i] = new float3(0, 1, 0); // 顶面法线
                uvs[faceIndex * 4 + i] = new float4(0, 0, 0, 0); // 可以替换为合适的UV坐标
            }

            for (int i = 0; i < 6; i++)
            {
                indices[faceIndex * 6 + i] = voxelIndices[i] + faceIndex * 4;
            }
        }
    }
}
