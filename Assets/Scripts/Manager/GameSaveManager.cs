using Info;
using Newtonsoft.Json;
using System.IO;

namespace Manager
{
    class GameSaveManager : Singleton<GameSaveManager>
    {
        private static string GameSavePath = "C:/Users/a/Desktop/TextDir";
        private static string fileName = "GameJson.txt";

        public static GameSaveInfo gameInfo;

        public void Init()
        {
            //if(!Directory.Exists(GameSavePath))
            //{
            //    Directory.CreateDirectory(GameSavePath);
            //}

            //string path = Path.Combine(GameSavePath, fileName);
            //if (!File.Exists(path))
            //{
            //    File.Create(path).Close();
            //}

            //Read();
        }

        public static void WriteAndSaveExit()
        {
            gameInfo.SaveExitTime();
            gameInfo.SaveAll();
            Save();
        }

        public static void WriteAndSaveNoExit()
        {
            gameInfo.SaveAll();
            Save();
        }

        public static void Save()
        {
            if (gameInfo.IsNull())
            {
                return;
            }

            string path = Path.Combine(GameSavePath, fileName);
            string json = JsonConvert.SerializeObject(gameInfo);
            File.WriteAllText(path, json);
        }

        public static void Read()
        {
            gameInfo = JsonConvert.DeserializeObject<GameSaveInfo>(File.ReadAllText(Path.Combine(GameSavePath, fileName)));
            if(gameInfo.IsNull())
            {
                gameInfo = new GameSaveInfo();
            }
        }

        public static bool IsFirstEnter()
        {
            return gameInfo.lastExitTimestamp < 0;
        }

        public static void SaveExitTime()
        {
            gameInfo.SaveExitTime();
        }

        public static void SaveMapID()
        {
            gameInfo.SaveMapID();
        }

        public static void SavePlayerInfo()
        {
            gameInfo.SavePlayerInfo();
        }
    }
}
