using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace Pool
{
    public class UIPool : PoolBase
    {
        private Dictionary<string, List<UIObject>> uis;

        public override void Init(string name)
        {
            base.Init(name);
            uis = new Dictionary<string, List<UIObject>>();
        }
       
        public override int Count
        {
            get { return uis.Count; }
        }

        public override void ReleaseAll()
        {
            List<List<UIObject>> objLList = uis.Values.ToList();
            foreach (var objList in objLList)
            {
                foreach(var obj in objList)
                {
                    obj.Release();
                }
            }
            uis.Clear();
        }

        public override GameObject Spawn(string name)
        {
            List<UIObject> objList;
            uis.TryGetValue(name, out objList);
            if(objList != null && objList.Count > 0)
            {
                var obj = objList[0];
                objList.Remove(obj);
                return obj.Spawn();
            }
            else
            {
                GameObject uiPrefab = GameObject.Instantiate(LoadTool.LoadUIPrefab(name));
                uiPrefab.name = name;
                return uiPrefab;
            }
        }

        public override void UnSpawn(GameObject go, string name)
        {
            base.UnSpawn(go, name);
            List<UIObject> objList;
            uis.TryGetValue(name, out objList);
            if (objList == null)
            {
                objList = new List<UIObject>();
                uis.Add(name, objList);
            }
            UIObject obj = new UIObject();
            obj.Unspawn(name, go);
            objList.Add(obj);
        }

        public override void Update(float dt)
        {
            Dictionary<string, List<UIObject>> caches = new Dictionary<string, List<UIObject>>();

            foreach (var objList in uis)
            {
                foreach (var obj in objList.Value)
                {
                    if (obj.Sustain) continue;

                    obj.StayTime += dt;
                    if(obj.StayTime > Constant.PoolConstant.defaultUIPoolRealeaseObjectTime)
                    {
                        obj.Release();

                        // 放到cache里，待会释放
                        List<UIObject> uiList;
                        caches.TryGetValue(objList.Key, out uiList);
                        if (uiList.IsNull())
                        {
                            uiList = new List<UIObject>();
                            caches.Add(objList.Key, uiList);
                        }
                        uiList.Add(obj);
                    }
                }
            }

            List<UIObject> objLCache;
            foreach (var keyValuePair in caches)
            {
                objLCache = uis[keyValuePair.Key];

                foreach (var obj in keyValuePair.Value)
                {
                    objLCache.Remove(obj);
                }

                if (objLCache.Count == 0)
                {
                    uis.Remove(keyValuePair.Key);
                }
            }
        }

        public override void Destroy()
        {
            ReleaseAll();
        }
    }
}
