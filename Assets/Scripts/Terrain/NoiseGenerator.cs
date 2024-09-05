using Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Terrain
{
    public class NoiseGenerator
    {
        static PerlinNoise noise;

        static NoiseGenerator()
        {
            noise = new PerlinNoise();
        }

        [BurstCompile]
        public static void GetChunkNoise(Vector2Int coord, out int[,] chunkNoise)
        {
            chunkNoise = new int[ChunkConstant.width, ChunkConstant.height];
            Vector3 worldPos = ChunkUtil.ChunkCoordToWorld(coord);
            Vector2 vecTemp = new Vector2();
            for (int i = 0; i < chunkNoise.GetLength(0); i++)
            {
                for (int j = 0; j < chunkNoise.GetLength(1); j++)
                {
                    vecTemp.x = worldPos.x + i * BrickConstant.width + 0.5f;
                    vecTemp.y = worldPos.z + j * BrickConstant.height + 0.5f;
                    chunkNoise[i, j] = CalcuAltitudes(vecTemp.x, vecTemp.y);
                }
            }
        }

        [BurstCompile]
        static int CalcuAltitudes(float x, float y)
        {
            return (int)(noise.FractalBrownianMotion(x, y, 3) * 50);
        }
    }
}
