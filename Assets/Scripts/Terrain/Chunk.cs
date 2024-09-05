using Constant;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.PlayerLoop.PreUpdate;
using UnityEngine.Rendering;
using System;
using Unity.Mathematics;
using System.Collections;
using Tools;
using System.Collections.Generic;

namespace Terrain
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public partial class Chunk : MonoBehaviour
    {
        Coroutine meshUpdater;
        Mesh mesh;
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        MeshCollider meshCollider;

        ChunkMeshBuilder.NativeMeshData meshData;
        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
            mesh = new Mesh();
        }

        private void Start()
        {
            meshFilter.mesh = mesh;
        }

        void OnDestroy()
        {
            meshData?.jobHandle.Complete();
            meshData?.Dispose();
        }

        IEnumerator UpdateMesh()
        {
            meshData?.Dispose();
            meshData = new ChunkMeshBuilder.NativeMeshData(new int2(ChunkConstant.width, ChunkConstant.height));

            yield return meshData.ScheduleMeshingJob(heightMap, new int2(ChunkConstant.width, ChunkConstant.height));

            meshData.GetMeshInformation(out int verticeSize, out int indicesSize);

            if (verticeSize > 0 && indicesSize > 0)
            {
                mesh.Clear();
                mesh.SetVertices(meshData.nativeVertices, 0, verticeSize);
                mesh.SetNormals(meshData.nativeNormals, 0, verticeSize);
                mesh.SetUVs(0, meshData.nativeUVs, 0, verticeSize);
                mesh.SetIndices(meshData.nativeIndices, 0, indicesSize, MeshTopology.Triangles, 0);

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                mesh.RecalculateTangents();

                ChunkColliderBuilder.Instance.Enqueue(this, mesh);
                meshRenderer.SetMaterials(new List<Material> { LoadTool.LoadMaterial("Normal/BrickMater") });
            }

            meshData.Dispose();
            meshUpdater = null;
        }

        public void SetSharedMesh(Mesh bakedMesh)
        {
            meshCollider.sharedMesh = bakedMesh;
        }
    }

    public partial class Chunk : MonoBehaviour
    {
        Transform root;
        Vector2Int MapCoord;
        Voxel[] voxels;
        public int[,] heightMap;

        public void Init(Transform root, Vector2Int coord)
        {
            this.root = root;
            MapCoord = coord;
            voxels = new Voxel[ChunkConstant.width * ChunkConstant.height];
            GenerateNoise();
            meshUpdater = StartCoroutine(nameof(UpdateMesh));
        }

        public void GenerateNoise()
        {
            NoiseGenerator.GetChunkNoise(MapCoord, out heightMap);
        }
    }
}
