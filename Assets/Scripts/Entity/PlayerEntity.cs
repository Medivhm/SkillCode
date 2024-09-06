using Constant;
using System;

namespace QEntity
{
    public class PlayerEntity : UnitEntity
    {
        public virtual void InitHUD()
        {
            hud = HUDManager.AddHUD(this.transform, Name, BloodMax, Blood);
        }

        public virtual void InitPropers()
        {
            basicAttr = new BasicAttr(PlayerConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(PlayerConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(PlayerConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(PlayerConstant.DefaultAttackAttr);
            BloodMax = 120f;
            Blood = 100f;
            Name = name;
        }
    }
}
