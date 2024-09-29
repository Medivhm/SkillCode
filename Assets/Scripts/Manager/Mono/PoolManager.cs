using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QEntity;
using Pool;
using Tools;
using UnityEngine;

namespace Manager
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;

        private static Dictionary<string, PoolBase> m_Pools;
        public static Transform poolTrans;

        public void Init()
        {
            m_Pools = new Dictionary<string, PoolBase>();

            // 所有回收对象都放在该结点下
            poolTrans = new GameObject("Pool").transform;
            GameObject.DontDestroyOnLoad(poolTrans);

            //StartCoroutine(PreCreatePool());
        }

        //IEnumerator PreCreatePool()
        //{
        //    BrickPool chunkPool = (BrickPool)GetBrickPool();
        //    int count = 0;
        //    for(int i = 1; i <= 10000; i++)
        //    {
        //        BrickEntity entity = GameObject.Instantiate(LoadTool.LoadBrickPrefab("NormalBrick")).GetComponent<BrickEntity>();
        //        entity.gameObject.name = "NormalBrick";
        //        entity.Destroy();
        //        count++;
        //        if(count > 200)
        //        {
        //            count = 0;
        //            yield return null;
        //        }
        //    }
        //}

        public int Count
        {
            get
            {
                return m_Pools.Count;
            }
        }

        public void Update()
        {
            if (m_Pools.IsNull()) 
            {
                return;
            }

            foreach(var pool in m_Pools.Values)
            {
                pool.Update(Time.deltaTime);
            }
        }

        public static AutoPool GetAutoPool()
        {
            PoolBase pool;
            m_Pools.TryGetValue(Constant.PoolConstant.AutoPoolName, out pool);
            if (pool == null)
            {
                pool = new AutoPool();
                pool.Init(Constant.PoolConstant.AutoPoolName);
                m_Pools.Add(Constant.PoolConstant.AutoPoolName, pool);
            }
            return pool as AutoPool;
        }

        public static BrickPool GetBrickPool()
        {
            PoolBase pool;
            m_Pools.TryGetValue(Constant.PoolConstant.BrickPoolName, out pool);
            if (pool == null)
            {
                pool = new BrickPool();
                pool.Init(Constant.PoolConstant.BrickPoolName);
                m_Pools.Add(Constant.PoolConstant.BrickPoolName, pool);
            }
            return pool as BrickPool;
        }

        public static UIPool GetUIPool()
        {
            PoolBase pool;
            m_Pools.TryGetValue(Constant.PoolConstant.UIPoolName, out pool);
            if (pool == null)
            {
                pool = new UIPool();
                pool.Init(Constant.PoolConstant.UIPoolName);
                m_Pools.Add(Constant.PoolConstant.UIPoolName, pool);
            }
            return pool as UIPool;
        }

        public static CarrierPool GetCarrierPool()
        {
            PoolBase pool;
            m_Pools.TryGetValue(Constant.PoolConstant.CarrierPoolName, out pool);
            if(pool == null)
            {
                pool = new CarrierPool();
                pool.Init(Constant.PoolConstant.CarrierPoolName);
                m_Pools.Add(Constant.PoolConstant.CarrierPoolName, pool);
            }
            return pool as CarrierPool;
        }

        public PoolBase GetPool(string poolName)
        {
            PoolBase pool;
            m_Pools.TryGetValue(poolName, out pool);
            return pool;
        }

        public void DestroyPool(string poolName)
        {
            PoolBase pool = GetPool(poolName);
            if (pool.IsNull()) return;

            pool.Destroy();
        }

        public List<PoolBase> GetAllPools()
        {
            return m_Pools.Values.ToList();
        }

        public bool HasPool(string poolName)
        {
            return m_Pools.ContainsKey(poolName);
        }

        public static void RealeaseAllPool()
        {
            List<PoolBase> pools = m_Pools.Values.ToList();
            foreach(var pool in pools)
            {
                pool.Destroy();
            }
        }

        public static void OnChangeMap()
        {
            RealeaseAllPool();
        }
    }
}
