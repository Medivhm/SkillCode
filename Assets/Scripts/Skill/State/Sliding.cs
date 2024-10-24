using Constant;
using Fsm;
using UnityEngine;

public class Sliding : SkillStateBase
{
    Vector3 dir;
    float speed;
    float speedDel;

    public Sliding(SkillFsm fsm) : base(fsm)
    {
    }

    protected internal override void Enter()
    {
        base.Enter();

        Debug.Log("Enter Sliding");

        unit.SetControl(false);
        speed = 140f;
        speedDel = 60f;
        unit.PlayAnim(Anim.SlidingAnim);
        dir = unit.transform.forward;
        Main.MainPlayerCtrl.UnitAct = UnitAct.Skill;
        Main.MainPlayerCtrl.SetColliderSquat();
    }

    protected internal override void Update(float dt)
    {
        base.Update(dt);
        if(speed < 50f)
        {
            Over();
            if (Main.MainPlayerCtrl.leftCtrlPress)
            {
                Main.MainPlayerCtrl.UnitAct = UnitAct.Squat;
            }
            else
            {
                Main.MainPlayerCtrl.UnitAct = UnitAct.Stand;
            }
            unit.SetControl(true);
        }
        else
        {
            unit.MoveTo(speed * dir * dt);
            speed -= speedDel * dt;
        }
    }
}
