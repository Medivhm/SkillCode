using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragArea : MonoBehaviour, IDragHandler
{
    public Transform DragTrans;

    Vector3 pos;
    public void OnDrag(PointerEventData eventData)
    {
        pos = DragTrans.position;
        pos.x = pos.x + eventData.delta.x;
        pos.y = pos.y + eventData.delta.y;
        DragTrans.position = pos;
    }
}
