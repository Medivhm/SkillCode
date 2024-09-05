using System;
using Tools;
using UnityEngine;

namespace Manager
{
    class AudioManager : Singleton<AudioManager>
    {
        public void Init()
        {

        }

        public static void PlayAudio(string name, Action callback = null, GameObject target = null)
        {
            GameObject go = target;
            if (go == null)
            {
                go = new GameObject(name);
            }
            go.name = "audio:" + name;
            AudioSource source = go.GetComponent<AudioSource>();
            if(source == null) source =  go.AddComponent<AudioSource>();
            AudioClip clip = LoadTool.LoadAudio(name);
            float clipLength = clip.length;
            source.clip = clip;
            source.spatialBlend = target == null ? 0.0f: 1.0f;
            source.Play();
            TimerManager.Add(clipLength, () =>
            {
                GameObject.Destroy(go);
                if(callback != null)
                {
                    callback.Invoke();
                }
            });
        }
    }
}
