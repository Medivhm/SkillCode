using Constant;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            if(value == isGrounded)
            {
                return;
            }

            isGrounded = value;
            if (GroundChange.IsNotNull())
            {
                GroundChange.Invoke(isGrounded);
            }
        }
    }

    public List<GameObject> grounds;
    Action<bool> GroundChange;

    private void Awake()
    {
        grounds = new List<GameObject>();
    }

    public void Add(Action<bool> groundChange)
    {
        GroundChange += groundChange;
    }

    public void Remove(Action<bool> groundChange)
    {
        GroundChange -= groundChange;
    }

    List<GameObject> removes = new List<GameObject>();
    public void ClearUnActive()
    {
        foreach(var go in grounds)
        {
            if (!go.activeSelf)
            {
                removes.Add(go);
            }
        }
        foreach(var go in removes)
        {
            grounds.Remove(go);
        }
        if (grounds.Count == 0)
        {
            IsGrounded = false;
        }
        removes.Clear();

        //stayOK = true;
        //TimerManager.Add(1f, () =>
        //{
        //    stayOK = false;
        //});
    }

    private void OnTriggerEnter(Collider other)
    {
            if(grounds.Count == 0)
            {
                IsGrounded = true;
            }
            grounds.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
            grounds.Remove(other.gameObject);
            if (grounds.Count == 0)
            {
                IsGrounded = false;
            }
    }

    //bool stayOK = false;
    //private void OnTriggerStay(Collider other)
    //{
    //    if (stayOK)
    //    {
    //        if (other.tag == TagConstant.Ground)
    //        {
    //            if (!grounds.Contains(other.gameObject))
    //            {
    //                grounds.Add(other.gameObject);
    //                IsGrounded = true;
    //            }
    //        }
    //    }
    //}
}
