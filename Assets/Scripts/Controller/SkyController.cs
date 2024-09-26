using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Light WorldLight;

    private float AnglePerSecond;
    private float IntensityPerAngle;   // 每度修改光强度，共90度
    public float time = 0f;

    public float maxIntensity = 2f;
    public float minIntensity = 0f;

    private float minuPerRound = 1f;  // 分钟每圈
    public float MinuPerRound
    {
        get => minuPerRound;
        set
        {
            minuPerRound = value;
            AnglePerSecond = 360f / minuPerRound / 60f;
        }
    }    

    public float AngleX 
    {
        get
        {
            float angle = WorldLight.transform.rotation.eulerAngles.x;
            if(angle < 0f)
            {
                while(angle < 0f)
                {
                    angle += 360f;
                }
            }
            if (angle > 360f)
            {
                while(angle > 360f)
                {
                    angle -= 360f;
                }
            }
            return angle;
        }
    }

    private void Awake()
    {
        Main.SkyController = this;
    }

    private void OnDestroy()
    {
        Main.SkyController = null;
    }

    void Start()
    {
        AnglePerSecond = 360f / MinuPerRound / 60f;
        IntensityPerAngle = (maxIntensity - minIntensity) / 90f;
    }

    void Update()
    {
        ChangeAngle(Time.deltaTime * AnglePerSecond);
        ChangeLightIntensity();
    }

    Vector3 rotateAngle = new Vector3();
    void ChangeAngle(float angle)
    {
        rotateAngle.x = angle;
        WorldLight.transform.Rotate(rotateAngle, Space.Self);
    }

    void ChangeLightIntensity()
    {
        if(AngleX < 180f)
        {
            WorldLight.intensity = IntensityPerAngle * (90f - Mathf.Abs(AngleX - 90f));
            RenderSettings.ambientIntensity = (90f - Mathf.Abs(AngleX - 90f)) / 90f;
        }
    }
}
