using Info;
using QEntity;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Manager
{
    class FxManager : Singleton<FxManager>
    {
        public void Init()
        {
        }

        public static FxInfo GetFxInfoByID(int fxID)
        {
            FxInfo info;
            DataManager.Instance.FxInfos.TryGetValue(fxID, out info);
            return info;
        }

        public static void Add(int id, Vector3 startPos, Transform parent = null, Vector3? dir = null, float speed = 0f, float destroyTime = -1f)
        {
            string name = GetFxInfoByID(id).name;
            GameObject fxGo = LoadTool.LoadFx(name);
            FxEntity fxEntity = fxGo.GetComponent<FxEntity>();
            fxEntity.Init(name, dir, speed);
            if (parent != null)
            { 
                fxGo.transform.SetParent(parent);
            }
            fxGo.transform.localPosition = startPos;
            
            if(destroyTime > 0f)
            {
                TimerManager.Add(destroyTime, () =>
                {
                    fxEntity.Destroy();
                });
            }
        }
    }
}
