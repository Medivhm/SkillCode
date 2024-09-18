using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetAudioSetting : MonoBehaviour
{
    [MenuItem("QTools/���³�ʼ�� FMOD ����")]
    public static void ResetFMODSetting()
    {
        AudioSettings.Reset(UnityEngine.AudioSettings.GetConfiguration());
    }
}
