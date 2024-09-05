using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Constant
{
    // Terrain
    public static class TerrainConstant
    {
        public static int width = 20;
        public static int height = 20;
    }

    // Chunk
    public static class ChunkConstant
    {
        // ����
        public static float chunkWLength = -1;
        public static float ChunkWLength
        {
            get
            {
                if(chunkWLength < 0)
                {
                    chunkWLength = ChunkConstant.width * BrickConstant.width;
                }
                return chunkWLength;
            }
        }
        public static float chunkHLength = -1;
        public static float ChunkHLength
        {
            get
            {
                if (chunkHLength < 0)
                {
                    chunkHLength = ChunkConstant.height * BrickConstant.height;
                }
                return chunkHLength;
            }
        }
        public static float chunkTLength = -1;
        public static float ChunkTLength
        {
            get
            {
                if (chunkHLength < 0)
                {
                    chunkHLength = ChunkConstant.tall * BrickConstant.tall;
                }
                return chunkHLength;
            }
        }


        public static int width = 50;
        public static int height = 50;
        public static int tall = 20;
    }

    // Brick
    public static class BrickConstant
    {
        public static float width = 1;
        public static float height = 1;
        public static float tall = 1;
        public static string NormalBrick = "NormalBrick";
        public static string GrassBrick  = "GrassBrick";
    }

    // Widgets
    public static class Widgets
    {
        public static string Text = "QText"; 
    }

    // MsgID
    public enum MsgID
    {
        VoxelChange,
    }

    // Tag
    public static class TagConstant
    {
        public static string Playable = "Playable";
        public static string Player   = "Player";
        public static string Obstacal = "Obstacal";
        public static string Ground   = "Ground";
        public static string UI       = "UI";
    }

    // �㼶
    public static class Layer
    {
        public static int Enemy  = 6;
        public static int Player = 7;
        public static int Ground = 8;
    }

    // ��������
    public static class WeaponType
    {
        public static int Close = 0;
        public static int Far   = 1;
        public static int Magic = 2;
    }

    // BUFF
    public static class BuffConstant
    {
        public static int BuffMax = 1000;
    }

    // Ѫ��
    public static class HUDConstant
    {
        // HUD3DCanvas����Ⱦ�㼶
        public static int   HUD3DCanvasSortingOrder = 20;
        public static float HUDHeight               = 4.2f;
    }

    // ��ʱ��
    public static class TimerConstant
    {
        public static int TimerMax = 2000;
    }

    // ��Ⱦ�㼶
    public static class SortingOrderConstant
    {
        // ������Ⱦ�㼶
        public static int PlayerSortingOrder = 20;
        // ������Ⱦ�㼶
        public static int EnemySortingOrder = 20;
    }

    // ������
    public static class CameraConstant
    {
        // �����ز���
        public static float CameraDistanceZ = 2;
        public static float CameraDistanceY = 1.1f;
        public static float CameraAngleX = 30;
    }

    // �����ʼ��
    public static class EnemyConstant
    {
        // �����ε���Χ
        public static float DefaultHoldRange = 8f;
        // ���﹥��������Χ
        public static float MinDesire = 0f;
        public static float MaxDesire = 100f;

        public static BasicAttr DefaultEntityAttr = new()
        {
            defence        = 10f,               // ����
            moveSpeed      = 1.8f,              // �ƶ��ٶ�
            maxMoveSpeed   = 3.0f,              // ����ƶ��ٶ�
            minMoveSpeed   = 1.0f,              // ��С�ƶ��ٶ�

            blood          = 20f,               // Ѫ��
            power          = 20f,               // ����
            stamina        = 20f,               // ����

            bloodMax       = 20f,               // Ѫ�����
            powerMax       = 20f,               // �������
            staminaMax     = 20f,               // �������

            bloodRecover   = 0f,                // Ѫ���ָ�
            powerRecover   = 0f,                // �����ָ�
            staminaRecover = 0f,                // �����ָ�
        };

        public static AttackAttr DefaultAttackAttr = new()
        {
            damage         = 5f,                // �˺�
            attackSpeed    = 0.3f,              // �����ٶȣ�/�룩
            critiOdds      = 0f,                // �������ʣ�0��1��
            critiCoeff     = 1.5f,              // �����˺����ʣ�������ô���1��
        };
    }

    public static class PoolConstant
    {
        // ���������
        public static string CarrierPoolName = "CarrierPool";
        public static string UIPoolName      = "UIPool";
        public static string BrickPoolName   = "BrickPool";

        // ������ڶ����Զ��ͷ�ʱ��
        public static float defaultCarrierPoolRealeaseObjectTime = 5f;
        public static float defaultUIPoolRealeaseObjectTime      = 10f;
    }

    public static class WaitSecondConstant
    {
        // ��һ��
        public static WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
    }

    public static class UIConstant
    {
        public static float UIZAxis = 100f;

        public static string ActionInfoUI         = "ActionInfoUI";
        public static string ActionInfoCell       = "ActionInfoCell";
        public static string ActivitiesUI         = "ActivitiesUI";
        public static string ActivitiesCell       = "ActivitiesCell";
        public static string ActivitiesCellButton = "ActivitiesCellButton";
        public static string ButtonList           = "ButtonList";
        public static string ButtonListCell       = "ButtonListCell";
        public static string SettingUI            = "SettingUI";                 // ����
        public static string BagUI                = "BagUI";                     // ����
        public static string TaskUI               = "TaskUI";                    // ����
        public static string LiaisonsUI           = "LiaisonsUI";                // ������
        public static string ProductionUI         = "ProductionUI";              // ����
        public static string FishingUI            = "FishingUI";                 // ����
        public static string FarmUI               = "FarmUI";                    // ��ֲ
        public static string CreateUI             = "CreateUI";                  // ����
        public static string ForgeUI              = "ForgeUI";                   // ����
        public static string SkillUI              = "SkillUI";                   // ����

        public static string TaskDetail           = "TaskDetail";
        public static string Item                 = "Item";
        public static string ItemTips             = "ItemTips";
        public static string Tab                  = "Tab";
        public static string JumpNumCell          = "JumpNumCell";
    }

    public static class ColorConstant
    {
        public static Color[] ColorDefine = new Color[]
        {
            new Color(0xFF, 0xFF, 0xFF),
            new Color(  0f,   0f,   0f),
            new Color(0xFF,   0f,   0f),
            new Color(  0f, 0xFF,   0f),
            new Color(  0f,   0f, 0xFF),
            new Color(0xFF, 0xFF,   0f),
            new Color(0x80,   0f, 0x80),
            new Color(0xFF, 0xC0, 0xCB),
            new Color(0xFF, 0xD7,   0f),
            new Color(0x80, 0x80, 0x80),
        };
    }

    public static class Vector2IntConstant
    {
        public static Vector2Int Up    = new Vector2Int( 0, 1);
        public static Vector2Int Left  = new Vector2Int(-1, 0);
        public static Vector2Int Down  = new Vector2Int( 0,-1);
        public static Vector2Int Right = new Vector2Int( 1, 0);
    }

    public enum Dir
    {
        Up,
        Down,
        Right,
        Left,
    }

    public static class SceneName
    {
        public static string MenuScene = "MenuScene";
        public static string GameScene = "GameScene";
    }
}
