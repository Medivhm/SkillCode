using QEntity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectHelper
{
    UnitEntity self;
    List<UnitEntity> units;
    public Action<GameObject> OnAddTarget;
    public Action<GameObject> OnDelTarget;
    public Action<GameObject> OnClearTarget;

    public void Init(UnitEntity entity, float detectRadius, string detectTag)
    {
        self = entity;
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
                dists[i] = Vector3.Distance(self.Position, units[i].Position);
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
                dists[i] = Vector3.Distance(self.Position, units[i].Position);
            }
            for (int i = 0; i < dists.Length; i++)
            {
                if (!self.camp.Equals(units[QUtil.QuickSort(dists)[i]].camp))
                {
                    return units[QUtil.QuickSort(dists)[i]];
                }
            }
        }
        return null;
    }

    public void Destroy()
    {
        units.Clear();
    }

    private void InitTrigger(UnitEntity entity, float detectRadius, string detectTag)
    {
        Collider detectCollider = entity.transform.AddSphereCollider(detectRadius, "Detect");
        TriggerListener listener = TriggerListener.Get(detectCollider.gameObject);
        listener.onTriggerEnter.AddListener((GameObject other) =>
        {
            if (self.gameObject.IsNotMe(other) && other.tag == detectTag)
            {
                Add(other.GetComponent<UnitEntity>());
                if (OnAddTarget.IsNotNull())
                {
                    OnAddTarget.Invoke(other);
                }
            }
        });
        listener.onTriggerExit.AddListener((GameObject other) =>
        {
            if (self.gameObject.IsNotMe(other) && other.tag == detectTag)
            {
                Remove(other.GetComponent<UnitEntity>());
                if (OnDelTarget.IsNotNull())
                {
                    OnDelTarget.Invoke(other);
                }
                if (units.Count == 0)
                {
                    if (OnClearTarget.IsNotNull())
                    {
                        OnClearTarget.Invoke(other);
                    }
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

    public void AddLastTargetLeaveEvent(Action<GameObject> action)
    {
        OnClearTarget += action;
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
