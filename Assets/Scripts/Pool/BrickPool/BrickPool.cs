using Manager;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace Pool
{
    public class BrickPool : PoolBase
    {
        private Dictionary<string, List<BrickObject>> chunks;

        public override void Init(string name)
        {
            base.Init(name);
            chunks = new Dictionary<string, List<BrickObject>>();
        }
       
        public override int Count
        {
            get { return chunks.Count; }
        }

        public override void ReleaseAll()
        {
            List<List<BrickObject>> objLList = chunks.Values.ToList();
            foreach (var objList in objLList)
            {
                foreach(var obj in objList)
                {
                    obj.Release();
                }
            }
            chunks.Clear();
        }

        public override GameObject Spawn(string name)
        {
            List<BrickObject> objList;
            chunks.TryGetValue(name, out objList);
            if(objList != null && objList.Count > 0)
            {
                var obj = objList[0];
                objList.Remove(obj);
                return obj.Spawn();
            }
            else
            {
                GameObject uiPrefab = GameObject.Instantiate(LoadTool.LoadBrickPrefab(name));
                uiPrefab.name = name;
                return uiPrefab;
            }
        }

        public override void UnSpawn(GameObject go, string name)
        {
            base.UnSpawn(go, name); 
            List<BrickObject> objList;
            chunks.TryGetValue(name, out objList);
            if (objList == null)
            {
                objList = new List<BrickObject>();
                chunks.Add(name, objList);
            }
            BrickObject obj = new BrickObject();
            if(objList.Count < 40000)
            {
                obj.Sustain = true;
            }
            else
            {
                obj.Sustain = false;
            }
            obj.Unspawn(name, go);
            objList.Add(obj);
        }

        Dictionary<string, List<BrickObject>> caches = new Dictionary<string, List<BrickObject>>();
        public override void Update(float dt)
        {
            caches.Clear();

            foreach (var objList in chunks)
            {
                foreach (var obj in objList.Value)
                {
                    if (obj.Sustain) continue;

                    obj.StayTime += dt;
                    if(obj.StayTime > Constant.PoolConstant.defaultUIPoolRealeaseObjectTime)
                    {
                        obj.Release();

                        // 放到cache里，待会释放
                        List<BrickObject> uiList;
                        caches.TryGetValue(objList.Key, out uiList);
                        if (uiList.IsNull())
                        {
                            uiList = new List<BrickObject>();
                            caches.Add(objList.Key, uiList);
                        }
                        uiList.Add(obj);
                    }
                }
            }

            List<BrickObject> objLCache;
            foreach (var keyValuePair in caches)
            {
                objLCache = chunks[keyValuePair.Key];

                foreach (var obj in keyValuePair.Value)
                {
                    objLCache.Remove(obj);
                }

                if (objLCache.Count == 0)
                {
                    chunks.Remove(keyValuePair.Key);
                }
            }
        }

        public override void Destroy()
        {
            ReleaseAll();
        }
    }
}
