//using Entity;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class DetectHelper
//{
//    List<UnitEntity> units;
//    public Action<GameObject> OnAddTarget;
//    public Action<GameObject> OnDelTarget;
//    public Action<GameObject> OnClearTarget;

//    public void Init(UnitEntity entity, float detectRadius, string detectTag)
//    {
//        units = new List<UnitEntity>();
//        InitTrigger(entity, detectRadius, detectTag);
//    }

//    public UnitEntity GetTarget()
//    {
//        if(units.Count > 0)
//        {
//            return units[0];
//        }
//        return null;
//    }

//    public void Destroy()
//    {
//        units.Clear();
//    }

//    private void InitTrigger(UnitEntity entity, float detectRadius, string detectTag)
//    {
//        Collider detectCollider = entity.transform.AddSphereCollider(detectRadius, "Detect");
//        TriggerListener listener = TriggerListener.Get(detectCollider.gameObject);
//        listener.onTriggerEnter.AddListener((GameObject other) =>
//        {
//            if (other.tag == detectTag)
//            {
//                Add(other.GetComponent<UnitEntity>());
//                if (OnAddTarget.IsNotNull())
//                {
//                    OnAddTarget.Invoke(other);
//                }
//            }
//        });
//        listener.onTriggerExit.AddListener((GameObject other) =>
//        {
//            if (other.tag == detectTag)
//            {
//                Remove(other.GetComponent<UnitEntity>());
//                if (OnDelTarget.IsNotNull())
//                {
//                    OnDelTarget.Invoke(other);
//                }
//                if (units.Count == 0)
//                {
//                    if (OnClearTarget.IsNotNull())
//                    {
//                        OnClearTarget.Invoke(other);
//                    }
//                }
//            }
//        });
//    }

//    private void Add(UnitEntity unit)
//    {
//        if (!units.Contains(unit))
//        {
//            units.Add(unit);
//        }
//    }

//    private void Remove(UnitEntity unit)
//    {
//        units.Remove(unit);
//    }
//}
