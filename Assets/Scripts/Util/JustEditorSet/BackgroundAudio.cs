using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        Main.BackgroundAudio = audioSource;
    }

    private void OnDestroy()
    {
        Main.BackgroundAudio = null;
    }
}
