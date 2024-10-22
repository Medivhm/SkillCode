using Fsm;
using UnityEngine;

public class Dash : SkillStateBase
{
    Vector3 dir;
    float distance;
    float time;
    float speed;

    public Dash(SkillFsm fsm) : base(fsm)
    {
    }

    protected internal override void Enter()
    {
        base.Enter();
        distance = 20f;
        time = 0.2f;
        dir = unit.Dir;
        speed = distance / time;
    }

    protected internal override void Update(float dt)
    {
        base.Update(dt);
        time -= dt;
        if(time > 0f)
        {
            unit.Move(dir.normalized * speed / unit.groundMoveSpeed);
        }
        else
        {
            unit.Move(dir.normalized * speed / unit.groundMoveSpeed);
            Over();
        }
    }
}
