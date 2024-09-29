using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Pool
{
    public class AutoPool : PoolBase
    {
        Dictionary<string, AutoSinglePool> autoPools;

        public override int Count => autoPools.Count;

        public override void Init(string poolName)
        {
            base.Init(poolName);
            autoPools = new Dictionary<string, AutoSinglePool>();
        }

        public override void ReleaseAll()
        {
            foreach(var autoPool in autoPools.Values.ToList())
            {
                autoPool.ReleaseAll();
            }
        }

        public GameObject Spawn(string poolName, string objectName)
        {
            AutoSinglePool pool;
            autoPools.TryGetValue(poolName, out pool);
            if (pool.IsNull())
            {
                pool = new AutoSinglePool();
                pool.Init(poolName);
                autoPools.Add(poolName, pool);
            }
            return pool.Spawn(objectName);
        }

        public override void Update(float dt)
        {
            foreach (var autoPool in autoPools.Values.ToList())
            {
                autoPool.Update(dt);
            }
        }

        public override void Destroy()
        {
            foreach (var autoPool in autoPools.Values.ToList())
            {
                autoPool.ReleaseAll();
            }
        }

        // 废弃
        public override GameObject Spawn(string _)
        {
            throw new NotImplementedException();
        }
    }

    class AutoSinglePool : PoolBase
    {
        private Dictionary<string, List<AutoObject>> commons;
        private string poolName;

        public override void Init(string name)
        {
            base.Init(name);
            poolName = name;
            commons = new Dictionary<string, List<AutoObject>>();
        }

        public override int Count
        {
            get { return commons.Count; }
        }

        public override void ReleaseAll()
        {
            List<List<AutoObject>> objLList = commons.Values.ToList();
            foreach (var objList in objLList)
            {
                foreach (var obj in objList)
                {
                    obj.Release();
                }
            }
            commons.Clear();
        }

        public override GameObject Spawn(string name)
        {
            List<AutoObject> objList;
            commons.TryGetValue(name, out objList);
            if (objList != null && objList.Count > 0)
            {
                var obj = objList[0];
                objList.Remove(obj);
                return obj.Spawn();
            }
            else
            {
                DebugTool.Warning(string.Format("{0}/{1}", poolName, name));
                GameObject autoPrefab = GameObject.Instantiate(LoadTool.LoadPrefab(string.Format("{0}/{1}", poolName, name)));
                autoPrefab.name = name;
                return autoPrefab;
            }
        }

        public override void UnSpawn(GameObject go, string name)
        {
            base.UnSpawn(go, name);
            List<AutoObject> objList;
            commons.TryGetValue(name, out objList);
            if (objList == null)
            {
                objList = new List<AutoObject>();
                commons.Add(name, objList);
            }
            AutoObject obj = new AutoObject();
            obj.Unspawn(name, go);
            objList.Add(obj);
        }

        public override void Update(float dt)
        {
            Dictionary<string, List<AutoObject>> caches = new Dictionary<string, List<AutoObject>>();

            foreach (var objList in commons)
            {
                foreach (var obj in objList.Value)
                {
                    if (obj.Sustain) continue;

                    obj.StayTime += dt;
                    if (obj.StayTime > Constant.PoolConstant.defaultUIPoolRealeaseObjectTime)
                    {
                        obj.Release();

                        // 放到cache里，待会释放
                        List<AutoObject> autoList;
                        caches.TryGetValue(objList.Key, out autoList);
                        if (autoList.IsNull())
                        {
                            autoList = new List<AutoObject>();
                            caches.Add(objList.Key, autoList);
                        }
                        autoList.Add(obj);
                    }
                }
            }

            List<AutoObject> objLCache;
            foreach (var keyValuePair in caches)
            {
                objLCache = commons[keyValuePair.Key];

                foreach (var obj in keyValuePair.Value)
                {
                    objLCache.Remove(obj);
                }

                if (objLCache.Count == 0)
                {
                    commons.Remove(keyValuePair.Key);
                }
            }
        }

        public override void Destroy()
        {
            ReleaseAll();
        }
    }
}
