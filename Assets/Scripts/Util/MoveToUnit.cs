using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToUnit : MonoBehaviour
{
    private bool init = false;
    private bool over = false;
    private Transform Target;
    private Action OverEvent;
    private float speed = 20f;
    private float minDis = 3f;

    public void Init(Transform target, Action overEvent = null)
    {
        init = true;
        Target = target;
        OverEvent = overEvent;
    }

    Vector3 dir;
    private void Update()
    {
        if (!init) return;
        if (over)  return;

        if(Vector3.Distance(Target.position, this.transform.position) < minDis)
        {
            Over();
            return;
        }

        dir = (Target.position - this.transform.position).normalized;
        this.transform.Translate(dir * Time.deltaTime * speed);
    }

    private void Over()
    {
        over = true;
        if (OverEvent.IsNotNull())
        {
            OverEvent.Invoke();
        }
        Destroy();
    }

    private void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
