using Fsm;
using QEntity;
using System.Collections.Generic;
using UnityEngine;

public class SkillFsm : FsmBase<SkillFsm>
{
    public override void Init(UnitEntity unit, List<FsmState<SkillFsm>> initStates)
    {
        base.Init(unit, initStates);
        InitKeyboard();
        ChangeState<NoSkill>();
    }

    public void InitKeyboard()
    {
        Ctrl.SetQuickKey(KeyCode.LeftShift, () =>
        {
            ChangeState<Dash>();
        });
    }
}