﻿using Manager;
using System.Numerics;
using UnityEngine;

namespace Info
{
    public class GameSaveInfo
    {
        public MapSaveData mapSaveData;
        public PlayerSaveData playerSaveData;
        public long lastExitTimestamp = -1;

        public GameSaveInfo()
        {
            mapSaveData = new MapSaveData();
            playerSaveData = new PlayerSaveData();
        }

        public void SaveExitTime()
        {
            lastExitTimestamp = Util.GetNowTimestamp();
        }

        public void SaveAll()
        {
            SaveMapID();
            SavePlayerInfo();
        }

        public void SaveMapID()
        {
            mapSaveData.Save();
        }

        public void SavePlayerInfo()
        {
            playerSaveData.Save();
        }
    }

    public class MapSaveData
    {
        public string mapName;

        public void Save()
        {
            mapName = SceneManager.currScene;
        }
    }

    public class PlayerSaveData
    {
        public System.Numerics.Vector3 playerPos;
        public float Blood;

        public void Save()
        {
            if (Main.MainPlayerCtrl.IsNotNull())
            {

            }
        }
    }
}
