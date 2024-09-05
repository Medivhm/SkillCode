using Constant;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Unity.VisualScripting;

namespace Manager
{
    public partial class ChunkManager : MonoSingleton<ChunkManager>
    {
        // 大块加载方式
        public Dictionary<Vector2Int, Chunk> MapChunks;      // 不会也 不应该有(0, 0)

        public Material ChunkMaterial;

        public void Init()
        {
            ChunkMeshBuilder.InitializeShaderParameter();
            MapChunks = new Dictionary<Vector2Int, Chunk>();
        }

        Vector2Int chunkCoord;
        Vector2Int tempV2Int;
        int[,] checks = new int[,]
        {
            { 0, 0 },
            { 0, 1 },
            { 0,-1 },
            { 1, 0 },
            {-1, 0 },

            { 1, 1 },
            { 1,-1 },
            {-1, 1 },
            {-1,-1 },

            { 0, 2 },
            { 0,-2 },
            { 2, 0 },
            {-2, 0 },
        };
        public void CheckPlayerPos(float _, Vector3 pos)
        {
            //chunkCoord = ChangeVec3ToCoord(pos);
            //for (int i = 0; i < checks.GetLength(0); i++)
            //{
            //    tempV2Int.x = chunkCoord.x + checks[i, 0];
            //    tempV2Int.y = chunkCoord.y + checks[i, 1];
            //    if (tempV2Int.x == 0)
            //    {
            //        if (checks[i, 0] > 0)
            //        {
            //            tempV2Int.x += 1;
            //        }
            //        else if (checks[i, 0] < 0)
            //        {
            //            tempV2Int.x -= 1;
            //        }
            //    }
            //    if (tempV2Int.y == 0)
            //    {
            //        if (checks[i, 1] > 0)
            //        {
            //            tempV2Int.y += 1;
            //        }
            //        else if (checks[i, 1] < 0)
            //        {
            //            tempV2Int.y -= 1;
            //        }
            //    }
            //    if (!MapChunks.ContainsKey(tempV2Int))
            //    {
            //        CreateChunk(tempV2Int);
            //    }
            //}
        }

        Chunk CreateChunk(Vector2Int chunkCoord)
        {
            Transform chunkRoot = new GameObject(string.Format("Chunk_{0}_{1}", chunkCoord.x, chunkCoord.y)).transform;
            chunkRoot.gameObject.tag = TagConstant.Ground;
            chunkRoot.transform.SetParent(Main.SceneRoot);
            chunkRoot.position = ChunkUtil.ChunkCoordToWorld(chunkCoord);

            Chunk newChunk = chunkRoot.AddComponent<Chunk>();
            newChunk.Init(chunkRoot, chunkCoord);
            MapChunks.Add(chunkCoord, newChunk);

            return newChunk;
        }

        Vector2Int coord;
        // 把物理坐标变成块坐标
        public Vector2Int ChangeVec3ToCoord(Vector3 pos)
        #region
        {
            coord.x = (Mathf.Sign(pos.x) * (Mathf.Abs(pos.x) / (BrickConstant.width * ChunkConstant.width) + 1)).ClosestIntToZero();
            coord.y = (Mathf.Sign(pos.z) * (Mathf.Abs(pos.z) / (BrickConstant.height * ChunkConstant.height) + 1)).ClosestIntToZero();
            return coord;
        }
        #endregion

        public void Clear()
        {
            //StopAllCoroutines();
            MapChunks.Clear();
        }

        //public void TestCreate()
        //{
        //    Chunk chunk = new Chunk(new Vector2Int(1, 1), ChunkConstant.width, ChunkConstant.height);
        //    chunk.Create();
        //    MakeTrue(chunk);
        //    MapChunks.Add(new Vector2Int(1, 1), chunk);
        //}


        //public void SyncShowChunk(Vector2Int mapPos, int width, int height)
        //{
        //    if (MapChunks.ContainsKey(mapPos))
        //    {
        //    }
        //    else
        //    {
        //        Chunk chunk = new Chunk(mapPos, chunkWidth, chunkHeight);
        //        chunk.Create();
        //        MakeTrue(chunk);
        //        MapChunks.Add(mapPos, chunk);
        //        //BRG_Example.Instance.UpdateVoxel();
        //    }
        //}


        //GameObject brickCache;
        //IEnumerator MakeTrueEx(object c)
        //{
        //    int count = 0;
        //    if (c is Chunk chunk)
        //    {
        //        for (int i = 0; i < chunk.width; i++)
        //        {
        //            for (int j = 0; j < chunk.height; j++)
        //            {
        //                if (chunk.altitudes[i, j] < 2)
        //                {
        //                    brickCache = chunk.GetABrick(BrickConstant.NormalBrick);
        //                }
        //                else
        //                {
        //                    brickCache = chunk.GetABrick(BrickConstant.NormalBrick);
        //                }
        //                brickCache.transform.SetParent(chunk.chunkRoot);
        //                brickCache.transform.localPosition = new Vector3(Instance.brickLengthX * (i - 0.5f), Instance.brickLengthY * chunk.altitudes[i, j], Instance.brickLengthZ * (j - 0.5f));
        //                count++;
        //                if (count == 30)
        //                {
        //                    count = 0;
        //                    //yield return null;
        //                }
        //            }
        //        }
        //        //StartCoroutine("CombineMesh", chunk.chunkRoot);
        //    }
        //    yield return null;
        //    if (Main.MainPlayerCtrl.IsNotNull())
        //    {
        //        Main.MainPlayerCtrl.RefreshCheckGround();
        //    }
        //}
    }

    //[BurstCompile]
    //public class Chunk
    //{
    //    public int width;          // 横向格子个数
    //    public int height;         // 纵向格子个数
    //    public float signX;        // x符号，判断象限
    //    public float signY;        // y符号，判断象限
    //    public int[,] altitudes;   // 左下到右上体素高度信息


    //    public Transform chunkRoot;    // 大块根节点
    //    public Vector3 leftDownPos;    // 大块左下角坐标
    //    Vector2Int mapPos;             // 大块位置坐标
    //    Vector3 centerPos;             // 大块中心坐标


    //    public Chunk(Vector2Int mapPos, int width, int height)
    //    {
    //        this.mapPos = mapPos;
    //        this.width = width;
    //        this.height = height;
    //        this.signX = Mathf.Sign(mapPos.x);
    //        this.signY = Mathf.Sign(mapPos.y);

    //        altitudes = new int[width, height];
    //    }

    //    public void Create()
    //    {
    //        CreateChunkRoot();
    //        CreateAltitudes();
    //    }

    //    void CreateChunkRoot()
    //    {
    //        chunkRoot = new GameObject(string.Format("Chunk_{0}_{1}", mapPos.x, mapPos.y)).transform;
    //        chunkRoot.SetParent(Main.SceneRoot);

    //        centerPos = new Vector3(signX * width * ChunkManager.Instance.brickLengthX * (Mathf.Abs(mapPos.x) - 0.5f), 0, signY * height * ChunkManager.Instance.brickLengthZ * (Mathf.Abs(mapPos.y) - 0.5f));
    //        leftDownPos = new Vector3(centerPos.x - width * ChunkManager.Instance.brickLengthX * 0.5f, centerPos.y, centerPos.z - height * ChunkManager.Instance.brickLengthZ * 0.5f);
    //        chunkRoot.localPosition = leftDownPos;
    //    }

    //    void CreateAltitudes()
    //    {
    //        int startY;
    //        int stepY;
    //        int startX;
    //        int stepX;
    //        if (signX > 0) { startX = 0; stepX = 1; } else { startX = width - 1; stepX = -1; }
    //        if (signY > 0) { startY = 0; stepY = 1; } else { startY = height - 1; stepY = -1; }

    //        for (int y = startY, ky = 0; ky < height; y += stepY, ky++)
    //        {
    //            for (int x = startX, kx = 0; kx < width; x += stepX, kx++)
    //            {
    //                altitudes[x, y] = CalcuAltitudes(kx, ky);
    //            }
    //        }
    //    }

    //    [BurstCompile]
    //    int CalcuAltitudes(int kx, int ky)
    //    {
    //        return (int)(ChunkManager.Instance.perlinNoise.FractalBrownianMotion(
    //                    signX * (Mathf.Abs(mapPos.x) - 1) * width +
    //                    signX * (kx + 0.5f),
    //                    signY * (Mathf.Abs(mapPos.y) - 1) * height +
    //                    signY * (ky + 0.5f), 3)
    //                    * 50);
    //    }

    //    ////////////////

    //    public GameObject GetABrick(string brickName)
    //    {
    //        return PoolManager.GetBrickPool().Spawn(brickName);
    //    }

    //    void ChangeShow(bool oldShow, bool newShow)
    //    {
    //        if (oldShow == newShow) return;

    //        if (newShow == true)
    //        {
    //            chunkRoot.localPosition = centerPos;
    //        }
    //        else
    //        {
    //            chunkRoot.localPosition = new Vector3(9999, 9999, 9999);
    //        }
    //    }
    //}
}
