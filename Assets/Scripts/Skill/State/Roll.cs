using Constant;
using Fsm;
using UnityEngine;

public class Roll : SkillStateBase
{
    Vector3 dir;
    float speed;
    float time;
    float timePass;
    float preAct = 0.01f;
    float afterAct = 0.01f;

    public Roll(SkillFsm fsm) : base(fsm)
    {
    }

    protected internal override void Enter()
    {
        base.Enter();

        Debug.Log("Enter Roll");

        unit.SetControl(false);
        speed = 80f;
        dir = unit.Dir;
        time = QUtil.GetAnimationClip(unit.animator, Anim.RollAnim).length;
        timePass = 0f;
        unit.PlayAnim(Anim.RollAnim, null, 0f, (time - 0.1f) / time);
        time = time - 0.1f;
        Main.MainPlayerCtrl.UnitAct = UnitAct.Skill;
        Main.MainPlayerCtrl.SetColliderSquat();
    }

    protected internal override void Update(float dt)
    {
        base.Update(dt);
        if(timePass > time)
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
            if(timePass > preAct && timePass + afterAct < time)
            {
                unit.MoveTo(dir * speed * dt);
            }
            timePass += dt;
        }
    }
}
