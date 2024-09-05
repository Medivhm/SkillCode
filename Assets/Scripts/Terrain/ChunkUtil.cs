using Constant;
using UnityEngine;

namespace Terrain
{
    public class ChunkUtil
    {
        public static Vector3 ChunkCoordToWorld(Vector2Int coord)
        {
            if(coord.x > 0)
            {
                coord.x -= 1;
            }
            if(coord.y > 0)
            {
                coord.y -= 1;
            }

            return new Vector3
            {
                x = coord.x * ChunkConstant.ChunkWLength,
                y = 0,
                z = coord.y * ChunkConstant.ChunkHLength,
            };
        }

        public static Vector2Int WorldToChunkCoord(Vector3 pos)
        {
            Vector2Int coord = new Vector2Int();
            coord.x = Mathf.FloorToInt(pos.x / ChunkConstant.ChunkWLength);
            coord.y = Mathf.FloorToInt(pos.z / ChunkConstant.ChunkHLength);
            if(coord.x >= 0)
            {
                coord.x += 1;
            }
            if(coord.y >= 0)
            {
                coord.y += 1;
            }
            return coord;
        }
    }
}
