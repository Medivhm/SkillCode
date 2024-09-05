using Fsm;

public class NoSkill : SkillStateBase
{
    public NoSkill(SkillFsm fsm) : base(fsm)
    {
    }

    protected internal override void Enter()
    {
        base.Enter();
        unit.SetControl(true);
    }

    protected internal override void Leave()
    {
        base.Leave();
        unit.SetControl(false);
    }
}
