using System;
using UnityEngine;

namespace Pool
{

    public abstract class ObjectBase
    {
        private string     m_Name;
        private GameObject m_Target;
        private float      m_StayTime;
        // 可能存在物体同时被到时销毁和取出，结果取出后在下一帧被销毁报错，所以加了这个字段
        private bool       m_InUse = false;
        // 是否维持，即不可销毁
        private bool       m_Sustain = false;

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public GameObject Target
        {
            get
            {
                return m_Target;
            }
        }

        public bool InUse
        {
            get
            {
                return m_InUse;
            }
        }

        public float StayTime
        {
            get
            {
                return m_StayTime;
            }
            set
            {
                m_StayTime = value;
            }
        }

        public bool Sustain
        {
            get
            {
                return m_Sustain;
            }
            set
            {
                m_Sustain = value;
            }
        }

        public virtual void Clear()
        {
            m_Name = null;
            m_Target = null;
            m_StayTime = 0f;
        }

        public virtual GameObject Spawn()
        {
            m_InUse = true;
            return Target;
        }

        public virtual void Unspawn(string name, GameObject target)
        {
            if (target == null)
            {
                DebugTool.ErrorFormat(string.Format("Target '{0}' is invalid.", name));
            }

            m_Name = name ?? string.Empty;
            m_Target = target;
            m_StayTime = 0f;
        }

        public abstract void Release();
    }
}
