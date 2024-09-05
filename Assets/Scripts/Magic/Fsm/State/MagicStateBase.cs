using Fsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic
{
    public class MagicStateBase : FsmState<MagicFsm>
    {
        public MagicStateBase(MagicFsm fsm) : base(fsm)
        {
        }

        protected virtual void Over()
        {
            ChangeState<NoMagic>();
        }

        protected virtual void DoNextStep()
        {
            fsm.ChangeStateByRule();
        }
    }
}
