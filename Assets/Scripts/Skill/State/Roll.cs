using Constant;
using Fsm;
using UnityEngine;

public class Roll : SkillStateBase
{
    Vector3 dir;
    float speed;
    float time;

    public Roll(SkillFsm fsm) : base(fsm)
    {
    }

    protected internal override void Enter()
    {
        base.Enter();

        Debug.Log("Enter Roll");

        unit.SetControl(false);
        speed = 100f;
        dir = unit.Dir;
        time = QUtil.GetAnimationClip(unit.animator, Anim.RollAnim).length + 0.1f;
        unit.PlayAnim(Anim.RollAnim);
        Main.MainPlayerCtrl.UnitAct = UnitAct.Skill;
    }

    protected internal override void Update(float dt)
    {
        base.Update(dt);
        if(time < 1f)
        {
            Over();
            unit.SetControl(true);
            if (Main.MainPlayerCtrl.leftCtrlPress)
            {
                Main.MainPlayerCtrl.UnitAct = UnitAct.Squat;
            }
            else
            {
                Main.MainPlayerCtrl.UnitAct = UnitAct.Stand;
            }
        }
        else
        {
            time -= dt;
            unit.MoveTo(dir * speed * dt);
        }
    }
}
