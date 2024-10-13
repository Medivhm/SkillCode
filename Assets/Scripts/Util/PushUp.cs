using QEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushUp : MonoBehaviour
{
    public float bounceForce = 80f;

    string unitLayerName = "Unit";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(unitLayerName))
        {
            UnitEntity unit = other.GetComponent<UnitEntity>();
            if (unit.IsNotNull())
            {
                unit.AddVelocity(new Vector3(0, bounceForce, 0));
            }
        }
    }
}
