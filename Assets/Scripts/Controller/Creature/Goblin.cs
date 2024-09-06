using Constant;
using Info;
using QEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    public class Goblin : CreatureEntity
    {
        private void Start()
        {
            gameObject.name = "兽人";
            camp = Camp.Camp1;


            basicAttr = new BasicAttr(CreatureConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            BloodMax = 120f;
            Blood = 100f;
            Name = name;
            hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "兽人"), 100f, 50f);
        }
    }
}
