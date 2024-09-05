using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constant
{
    public static class PlayerConstant
    {
        public static BasicAttr DefaultEntityAttr = new()
        {
            defence        = 10f,               // 防御
            moveSpeed      = 4f,                // 移动速度

            blood          = 80f,               // 血量
            power          = 80f,               // 耐力
            stamina        = 80f,               // 法力

            bloodMax       = 100f,              // 血量最大
            powerMax       = 100f,              // 耐力最大
            staminaMax     = 100f,              // 法力最大

            bloodRecover   = 1f,                // 血量恢复
            powerRecover   = 1f,                // 耐力恢复
            staminaRecover = 1f,                // 法力恢复
        };

        public static AttackAttr DefaultAttackAttr = new()
        {
            damage         = 5f,                // 伤害
            attackSpeed    = 5f,                // 攻击速度（/秒）
            critiOdds      = 0.1f,              // 暴击几率（0，1）
            critiCoeff     = 1.5f,              // 暴击伤害倍率（正常你得大于1）
        };
    }
}
