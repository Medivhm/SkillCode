using Fsm;
using QEntity;
using System.Collections.Generic;

namespace Magic
{
    public class MagicFsm : FsmBase<MagicFsm>
    {
        public bool IsExecute => magicSeq.IsNotNull();
        public MagicFsm():base()
        {
        }

        public override void Init(UnitEntity unit, List<FsmState<MagicFsm>> initStates)
        {
            base.Init(unit, initStates);
            ChangeState<NoMagic>();
        }

        Queue<string> magicSeq= null;
        public void SetMagicSequence(string magicSeq)
        {
            if (this.magicSeq.IsNull())
            {
                //this.magicSeq = magicSeq;
            }
            else
            {
                DebugTool.WarningFormat("上一个技能还没放完");
            }
        }

        void AnalizeCode(string magicSeq)
        {

        }


        public override void ChangeStateByRule()
        {
            base.ChangeStateByRule();

        }
    }
}
