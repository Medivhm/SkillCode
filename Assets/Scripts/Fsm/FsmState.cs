using QEntity;
using System;
using UnityEngine;

namespace Fsm
{
    public class FsmState<T> where T : FsmBase<T>
    {
        protected T fsm;
        protected UnitEntity unit => fsm.unit;

        public FsmState(T fsm)
        {
            this.fsm = fsm;
        }

        internal protected virtual void Enter()
        {
        }

        internal protected virtual void Update(float dt)
        {
        }

        internal protected virtual void Leave()
        {
        }

        internal protected virtual void Destroy()
        {
        }

        internal protected virtual void ChangeState<Y>()
        {
            fsm.ChangeState<Y>();
        }

        internal protected virtual void ChangeState(FsmState<T> state)
        {
            fsm.ChangeState(state.GetType());
        }
    }
}
