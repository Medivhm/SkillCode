using Fsm;

public class SkillStateBase : FsmState<SkillFsm>
{
    public SkillStateBase(SkillFsm fsm) : base(fsm)
    {
    }

    protected virtual void Over()
    {
        ChangeState<NoSkill>();
    }
}
