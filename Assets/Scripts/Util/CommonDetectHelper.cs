using QEntity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonDetectHelper
{
    GameObject self;
    Camp selfCamp;
    [SerializeField]
    public UnitEntity Target;
    List<UnitEntity> units;
    public Action<GameObject> OnAddTarget;
    public Action<GameObject> OnDelTarget;

    public void Init(GameObject entity, float detectRadius, string detectTag, Camp camp)
    {
        self = entity;
        selfCamp = camp;
        units = new List<UnitEntity>();
        InitTrigger(entity, detectRadius, detectTag);
    }

    public UnitEntity GetClosest()
    {
        if(units.Count > 0)
        {
            float[] dists = new float[units.Count];
            for(int i = 0; i < dists.Length; i++)
            {
                dists[i] = Vector3.Distance(self.transform.position, units[i].transform.position);
            }
            return units[QUtil.QuickSort(dists)[0]];
        }
        return null;
    }

    public UnitEntity GetClosestOtherCamp()
    {
        if (units.Count > 0)
        {
            float[] dists = new float[units.Count];
            for (int i = 0; i < dists.Length; i++)
            {
                dists[i] = Vector3.Distance(self.transform.position, units[i].transform.position);
            }
            for (int i = 0; i < dists.Length; i++)
            {
                if (!selfCamp.Equals(units[QUtil.QuickSort(dists)[i]].camp))
                {
                    return units[QUtil.QuickSort(dists)[i]];
                }
            }
        }
        return null;
    }

    public UnitEntity RefreshTarget()
    {
        if (units.Count > 0)
        {
            Target = GetClosestOtherCamp();
        }
        return Target;
    }

    public void Destroy()
    {
        units.Clear();
        self = null;
        Target = null;
        OnAddTarget = null;
        OnDelTarget = null;
        if (detectCollider.gameObject)
        {
            GameObject.Destroy(detectCollider.gameObject);
        }
    }

    Collider detectCollider;
    private void InitTrigger(GameObject go, float detectRadius, string detectTag)
    {
        detectCollider = go.transform.AddSphereCollider(detectRadius, "Detect");
        TriggerListener listener = TriggerListener.Get(detectCollider.gameObject);
        listener.onTriggerEnter.AddListener((GameObject other) =>
        {
            UnitEntity entity = other.GetComponent<UnitEntity>();
            if (self.gameObject.IsNotMe(other) && other.tag == detectTag && !selfCamp.Equals(entity.camp))
            {
                if (Target.IsNull()) Target = entity;
                Add(entity);
                if (OnAddTarget.IsNotNull())
                {
                    OnAddTarget.Invoke(other);
                }
            }
        });
        listener.onTriggerExit.AddListener((GameObject other) =>
        {
            UnitEntity entity = other.GetComponent<UnitEntity>();
            if (entity && units.Contains(entity))
            {
                Remove(entity);
                if (OnDelTarget.IsNotNull())
                {
                    OnDelTarget.Invoke(other);
                }
            }
        });
    }

    public void AddTargetEnterEvent(Action<GameObject> action)
    {
        OnAddTarget += action;
    }

    public void AddTargetLeaveEvent(Action<GameObject> action)
    {
        OnDelTarget += action;
    }

    private void Add(UnitEntity unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    private void Remove(UnitEntity unit)
    {
        units.Remove(unit);
    }
}
