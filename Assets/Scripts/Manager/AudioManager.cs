using System;
using Tools;
using UnityEngine;

namespace Manager
{
    class AudioManager : Singleton<AudioManager>
    {
        public void Init()
        {
            if (Main.BackgroundAudio)
            {
                AudioClip clip = LoadTool.LoadAudio("NoOut/flowersea");
                Main.BackgroundAudio.clip = clip;
                Main.BackgroundAudio.loop = true;
                if (Main.PlayBackgroundMusic)
                {
                    Main.BackgroundAudio.Play();
                }
            }
        }

        public static void Play(string audioName, Action callback = null, GameObject target = null)
        {
            GameObject go = target;
            if (go.IsNull())
            {
                go = Main.MainCamera.gameObject;
                if (go.IsNull()) return;
            }

            AudioSource source = go.GetOrAddComponent<AudioSource>();
            AudioClip clip = LoadTool.LoadAudio(audioName);
            float clipLength = clip.length;
            source.clip = clip;
            source.spatialBlend = target == null ? 0.0f: 1.0f;
            source.Play();
            TimerManager.Add(clipLength, () =>
            {
                if(callback.IsNotNull())
                {
                    callback.Invoke();
                }
            });
        }

        public static void PlayLoop(string audioName, GameObject target = null)
        {
            GameObject go = target;
            if (go.IsNull())
            {
                go = Main.MainCamera.gameObject;
                if (go.IsNull()) return;
            }

            AudioSource source = go.GetOrAddComponent<AudioSource>();
            AudioClip clip = LoadTool.LoadAudio(audioName);
            source.clip = clip;
            source.loop = true;
            source.spatialBlend = target == null ? 0.0f : 1.0f;
            source.Play();
        }

        public static void PlayOneShot(string audioName, float volumeScale = 1f)
        {
            if (Main.BackgroundAudio)
            {
                Main.BackgroundAudio.PlayOneShot(LoadTool.LoadAudio(audioName), volumeScale);
            }
        }
    }
}
