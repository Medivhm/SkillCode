using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateUI : MonoBehaviour
{
    public Transform LeftDownNode;
    public Transform LeftMiddleNode;
    public Transform LeftUpNode;
    public Transform MiddleDownNode;
    public Transform CenterNode;
    public Transform MiddleUpNode;
    public Transform RightDownNode;
    public Transform RightMiddleNode;
    public Transform RightUpNode;

    private void Awake()
    {
        Main.LocateUI = this;
    }

    private void OnDestroy()
    {
        Main.LocateUI = null;
    }
}
