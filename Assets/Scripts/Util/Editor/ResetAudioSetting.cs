using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetAudioSetting : MonoBehaviour
{
    [MenuItem("QTools/重新初始化 FMOD 引擎")]
    public static void ResetFMODSetting()
    {
        AudioSettings.Reset(UnityEngine.AudioSettings.GetConfiguration());
    }
}
