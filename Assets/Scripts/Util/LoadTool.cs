using Constant;
using Manager;
using Pool;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tools
{
    public class LoadTool : Singleton<LoadTool>
    {
        public static GameObject LoadPrefab(string name)
        {
            return Resources.Load<GameObject>("Prefabs/" + name);
        }

        public static string LoadJson(string name)
        {
            return Resources.Load<TextAsset>("Jsons/" + name).text;
        }

        public static Sprite LoadSprite(string name)
        {
            return Resources.Load<Sprite>("Sprites/" + name);
        }

        public static Material LoadMaterial(string name)
        {
            return Resources.Load<Material>("Maters/" + name);
        }

        public static GameObject LoadGameObjectPrefab(string name)
        {
            return LoadPrefab(name);
        }

        public static GameObject LoadBrickPrefab(string name)
        {
            return LoadPrefab("Bricks/" + name);
        }

        public static GameObject LoadPlayablePrefab(string name)
        {
            return LoadPrefab("Playable/" + name);
        }

        public static GameObject LoadPlayable(string name)
        {
            return GameObject.Instantiate(LoadPlayablePrefab(name));
        }

        public static GameObject LoadUIPrefab(string name)
        {
            return LoadPrefab("UIs/" + name);
        }

        public static GameObject LoadUI(string name)
        {
            return PoolManager.GetUIPool().Spawn(name);
        }

        public static GameObject LoadWidget(string name)
        {
            return GameObject.Instantiate(LoadPrefab("Widgets/" + name));
        }

        public static GameObject LoadWidgetPrefab(string name)
        {
            return LoadPrefab("Widgets/" + name);
        }

        public static GameObject LoadNPC(string name)
        {
            return LoadPrefab("npcs/" + name);
        }

        public static GameObject LoadFx(string name)
        {
            GameObject fxGo = GameObject.Instantiate(LoadPrefab("Fx"));
            fxGo.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("fxs/" + name);
            fxGo.name = "fx_" + name;
            return fxGo;
        }

        public static byte[] LoadLua(string name)
        {
            return Resources.Load<TextAsset>(string.Format("lua/{0}", name)).bytes;
        }

        public static GameObject LoadCarrier(string name)
        {
            return PoolManager.GetCarrierPool().Spawn(name);
        }

        public static AudioClip LoadAudio(string name)
        {
            return Resources.Load<AudioClip>("Audios/" + name);
        }   
    }
}