using Manager;
using System;
using UnityEngine;

namespace Pool
{
    public abstract class PoolBase
    {
        private string m_Name;

        public virtual void Init(string poolName)
        {
            m_Name = poolName;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public abstract int Count
        {
            get;
        }

        public abstract void ReleaseAll();

        public virtual void UnSpawn(GameObject go, string name)
        {
            go.transform.SetParent(PoolManager.poolTrans);
        }

        public abstract GameObject Spawn(string name);

        public abstract void Update(float dt);

        public abstract void Destroy();
    }
}
