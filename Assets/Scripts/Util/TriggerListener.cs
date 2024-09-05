using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : UnityEvent<GameObject> { }

public class TriggerListener : MonoBehaviour
{
    public static TriggerListener Get(GameObject go)
    {
        TriggerListener listener = go.GetComponent<TriggerListener>();
        if(listener == null)
        {
            listener = go.AddComponent<TriggerListener>();
        }
        return listener;
    }

    public TriggerEvent onTriggerEnter = new TriggerEvent();
    //public TriggerEvent onTriggerStay  = new TriggerEvent();
    public TriggerEvent onTriggerExit  = new TriggerEvent();

    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnter != null)
        {
            onTriggerEnter.Invoke(other.gameObject);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    onTriggerStay.Invoke(other.gameObject);
    //}

    private void OnTriggerExit(Collider other)
    {
        if (onTriggerExit != null)
        {
            onTriggerExit.Invoke(other.gameObject);
        } 
    }
}
