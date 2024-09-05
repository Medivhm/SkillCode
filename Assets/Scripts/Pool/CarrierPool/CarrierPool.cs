using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tools;
using UnityEngine;

namespace Pool
{
    public class CarrierPool : PoolBase
    {
        private Dictionary<string, List<CarrierObject>> carriers;

        public override void Init(string name)
        {
            base.Init(name);
            carriers = new Dictionary<string, List<CarrierObject>>();
        }
       
        public override int Count
        {
            get { return carriers.Count; }
        }

        public override void ReleaseAll()
        {
            List<List<CarrierObject>> objLList = carriers.Values.ToList();
            foreach (var objList in objLList)
            {
                foreach(var obj in objList)
                {
                    obj.Release();
                }
            }
            carriers.Clear();
        }

        public override GameObject Spawn(string name)
        {                                
            List<CarrierObject> objList;
            carriers.TryGetValue(name, out objList);
            if(objList != null && objList.Count > 0)
            {
                var obj = objList[0];
                objList.Remove(obj);
                return obj.Spawn();
            }
            else
            {
                GameObject carrierPrefab = GameObject.Instantiate(LoadTool.LoadPrefab("Carriers/" + name));
                carrierPrefab.name = name;
                return carrierPrefab;
            }
        }

        public override void UnSpawn(GameObject go, string name)
        {
            base.UnSpawn(go, name);
            List<CarrierObject> objList;
            carriers.TryGetValue(name, out objList);
            if (objList == null)
            {
                objList = new List<CarrierObject>();
                carriers.Add(name, objList);
            }
            CarrierObject obj = new CarrierObject();
            obj.Unspawn(name, go);
            objList.Add(obj);
        }

        public override void Update(float dt)
        {
            Dictionary<string, List<CarrierObject>> caches = new Dictionary<string, List<CarrierObject>>();

            foreach (var objList in carriers)
            {
                foreach (var obj in objList.Value)
                {
                    if (obj.Sustain) continue;

                    obj.StayTime += dt;
                    if(obj.StayTime > Constant.PoolConstant.defaultCarrierPoolRealeaseObjectTime)
                    {
                        obj.Release();

                        // 放到cache里，待会释放
                        List<CarrierObject> carrierList;
                        caches.TryGetValue(objList.Key, out carrierList);
                        if (carrierList.IsNull())
                        {
                            carrierList = new List<CarrierObject>();
                            caches.Add(objList.Key, carrierList);
                        }
                        carrierList.Add(obj);
                    }
                }
            }

            List<CarrierObject> objLCache;
            foreach (var keyValuePair in caches)
            {
                objLCache = carriers[keyValuePair.Key];

                foreach (var obj in keyValuePair.Value)
                {
                    objLCache.Remove(obj);
                }

                if (objLCache.Count == 0)
                {
                    carriers.Remove(keyValuePair.Key);
                }
            }
        }

        public override void Destroy()
        {
            ReleaseAll();
        }
    }
}
