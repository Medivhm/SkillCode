

using Constant;
using Info;
using UnityEngine;

namespace QEntity
{

    public class NPCEntity : UnitEntity
    {
        NPCInfo info;

        public NPCInfo Info
        {
            get { return info; }
        }

        public virtual void Init(NPCInfo info)
        {
            basicAttr = new BasicAttr(EnemyConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(EnemyConstant.DefaultAttackAttr);
            this.info = info;
            BloodMax = info.blood;
            Blood = info.blood;
            Name = info.name;
            InitHUD();
        }

        public void SetCenter(Vector3 center)
        {
            Center = center;
        }

        public void SetCenter(float cX, float cY, float cZ)
        {
            Center = new Vector3(cX, cY, cZ);
        }

        public virtual void InitHUD()
        {
            //hud = HUDManager.AddHUD(this, Name, BloodMax, Blood);
        }

        public override void Dead()
        {
            base.Dead();
            if (Main.ActionInfoUI != null)
            {
                GUI.ActionInfoLog(string.Format("你打败了 <color=red>[{0}]</color>", Name));
            }
        }

        public override void Destroy()
        {
            DestroyTimers();
        }

        void OnPointerClick()
        {
            GUI.ActionInfoLog(string.Format("你点击了NPC [{0}]", name));
        }
    }
}
