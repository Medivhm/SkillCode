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
        // 缓存
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

    // 层级
    public static class Layer
    {
        public static int Enemy  = 6;
        public static int Player = 7;
        public static int Ground = 8;
    }

    // 武器类型
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

    // 血条
    public static class HUDConstant
    {
        // HUD3DCanvas的渲染层级
        public static int   HUD3DCanvasSortingOrder = 20;
        public static float HUDHeight               = 4.2f;
    }

    // 定时器
    public static class TimerConstant
    {
        public static int TimerMax = 2000;
    }

    // 渲染层级
    public static class SortingOrderConstant
    {
        // 主角渲染层级
        public static int PlayerSortingOrder = 20;
        // 怪物渲染层级
        public static int EnemySortingOrder = 20;
    }

    // 相机相关
    public static class CameraConstant
    {
        // 相机相关参数
        public static float CameraDistanceZ = 2;
        public static float CameraDistanceY = 1.1f;
        public static float CameraAngleX = 30;
    }

    // 怪物初始化
    public static class EnemyConstant
    {
        // 怪物游荡范围
        public static float DefaultHoldRange = 8f;
        // 怪物攻击欲望范围
        public static float MinDesire = 0f;
        public static float MaxDesire = 100f;

        public static BasicAttr DefaultEntityAttr = new()
        {
            defence        = 10f,               // 防御
            moveSpeed      = 1.8f,              // 移动速度
            maxMoveSpeed   = 3.0f,              // 最大移动速度
            minMoveSpeed   = 1.0f,              // 最小移动速度

            blood          = 20f,               // 血量
            power          = 20f,               // 耐力
            stamina        = 20f,               // 法力

            bloodMax       = 20f,               // 血量最大
            powerMax       = 20f,               // 耐力最大
            staminaMax     = 20f,               // 法力最大

            bloodRecover   = 0f,                // 血量恢复
            powerRecover   = 0f,                // 耐力恢复
            staminaRecover = 0f,                // 法力恢复
        };

        public static AttackAttr DefaultAttackAttr = new()
        {
            damage         = 5f,                // 伤害
            attackSpeed    = 0.3f,              // 攻击速度（/秒）
            critiOdds      = 0f,                // 暴击几率（0，1）
            critiCoeff     = 1.5f,              // 暴击伤害倍率（正常你得大于1）
        };
    }

    public static class PoolConstant
    {
        // 对象池名称
        public static string CarrierPoolName = "CarrierPool";
        public static string UIPoolName      = "UIPool";
        public static string BrickPoolName   = "BrickPool";

        // 对象池内对象自动释放时间
        public static float defaultCarrierPoolRealeaseObjectTime = 5f;
        public static float defaultUIPoolRealeaseObjectTime      = 10f;
    }

    public static class WaitSecondConstant
    {
        // 等一秒
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
        public static string SettingUI            = "SettingUI";                 // 设置
        public static string BagUI                = "BagUI";                     // 背包
        public static string TaskUI               = "TaskUI";                    // 任务
        public static string LiaisonsUI           = "LiaisonsUI";                // 联络人
        public static string ProductionUI         = "ProductionUI";              // 生产
        public static string FishingUI            = "FishingUI";                 // 钓鱼
        public static string FarmUI               = "FarmUI";                    // 种植
        public static string CreateUI             = "CreateUI";                  // 制造
        public static string ForgeUI              = "ForgeUI";                   // 锻造
        public static string SkillUI              = "SkillUI";                   // 技能

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
