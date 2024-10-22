using QEntity;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Fsm
{

    public class FsmBase<T> where T : FsmBase<T>
    {
        Dictionary<Type, FsmState<T>> states;
        bool isInit = false;

        public UnitEntity unit;
        protected FsmState<T> currState;

        public virtual void Init(UnitEntity unit, List<FsmState<T>> initStates)
        {
            this.unit = unit;
            states = new Dictionary<Type, FsmState<T>>();
            foreach(var state in initStates)
            {
                states.Add(state.GetType(), state);
            }
            isInit = true;
        }

        public FsmState<T> GetCurrState()
        {
            return currState;
        }

        private void SetCurrState<Type>()
        {
            SetCurrState(typeof(Type));
        }

        private void SetCurrState(Type type)
        {
            currState = states[type];
        }

        public void Update(float dt)
        {
            if (isInit)
            {
                if (currState.IsNotNull())
                {
                    currState.Update(dt);
                }
            }
        }

        public virtual void ChangeStateByRule()
        {

        }

        public void ChangeState<Y>()
        {
            ChangeState(typeof(Y));
        }

        public void ChangeState(Type stateType)
        {
            if (!states.ContainsKey(stateType))
            {
                DebugTool.ErrorFormat("Fsm ChangeState -- 类型 [{0}] 未注册", stateType.ToString());
                return;
            }

            if (currState.IsNotNull())
            {
                if (currState.GetType() == stateType) return;

                currState.Leave();
            }
            SetCurrState(stateType);
            currState.Enter();
        }

        public void Destroy()
        {
            isInit = false;
        }
    }
}
