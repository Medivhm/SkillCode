using Steamworks;
using System.Collections.Generic;

namespace Util
{
    public static class SteamUtil
    {
        // 获取自己steamID
        public static CSteamID GetSteamID()
        {
            return SteamUser.GetSteamID();
        } 

        // 获取自己steam名称
        public static string GetSteamName()
        {
            return SteamFriends.GetPersonaName();
        }

        /// <summary>
        /// 获取steam朋友的steamID列表
        /// </summary>
        /// <returns></returns>
        public static List<CSteamID> GetFriends()
        {
            List<CSteamID> list = new List<CSteamID>();
            int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
            for (int i = 0; i < friendCount; i++)
            {
                list.Add(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate));

                //string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
                //DebugTool.Log($"Friend {i}: {friendName} (ID: {friendSteamID})");
            }
            return list;
        }

        /// <summary>
        /// 获取朋友中steamID对应steam名称
        /// </summary>
        /// <param name="friendSteamID"></param>
        /// <returns></returns>
        public static string GetFriendPersonName(CSteamID friendSteamID)
        {
            return SteamFriends.GetFriendPersonaName(friendSteamID);
        }

        /// <summary>
        /// 云存储数据，覆写整个文件
        /// </summary>
        /// <param name="fileName">文件名，带后缀</param>
        /// <param name="data">内容</param>
        /// <returns>bool 是否存储成功</returns>
        public static bool SaveToCloud(string fileName, byte[] data)
        {
            int fileSize = data.Length;
            return SteamRemoteStorage.FileWrite(fileName, data, fileSize);
        }

        /// <summary>
        /// 云读取数据
        /// </summary>
        /// <param name="fileName">文件名，带后缀</param>
        /// <returns>内容 or null</returns>
        public static byte[] ReadFromCloud(string fileName)
        {
            if (SteamRemoteStorage.FileExists(fileName))
            {
                int fileSize = SteamRemoteStorage.GetFileSize(fileName);
                byte[] fileData = new byte[fileSize];

                // 从云中读取文件
                int bytesRead = SteamRemoteStorage.FileRead(fileName, fileData, fileSize);

                if (bytesRead > 0)
                {
                    return fileData;
                }
                else
                {
                    DebugTool.Error("Failed to load the file from Steam Cloud");
                }
            }
            else
            {
                DebugTool.Error("File does not exist in Steam Cloud");
            }
            return null;
        }

        /// <summary>
        /// 检查当前应用是否启用了云存储功能
        /// </summary>
        /// <returns></returns>
        public static bool IsCloudEnabledForApp()
        {
            return SteamRemoteStorage.IsCloudEnabledForApp();
        }

        /// <summary>
        /// 检查玩家账户是否启用了云存储
        /// </summary>
        /// <returns></returns>
        public static bool IsCloudEnabledForAccount()
        {
            return SteamRemoteStorage.IsCloudEnabledForAccount();
        }

        /// <summary>
        /// 不再追踪该文件，减轻存储负担
        /// </summary>
        public static void FileForget(string fileName)
        {
            SteamRemoteStorage.FileForget(fileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public static void FileDelete(string fileName)
        {
            SteamRemoteStorage.FileDelete(fileName);
        }

        /// <summary>
        /// 文件是否已持久化，是否已经在云端同步，好像有一个持久化的过程，与Exist区分
        /// </summary>
        public static void FilePersisted(string fileName)
        {
            SteamRemoteStorage.FilePersisted(fileName);
        }

        /// <summary>
        /// 打开文件写入流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>写入句柄</returns>
        public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string fileName)
        {
            return SteamRemoteStorage.FileWriteStreamOpen(fileName);
        }

        /// <summary>
        /// 关闭文件写入流
        /// </summary>
        /// <param name="writeHandle">写入句柄</param>
        /// <returns>是否成功</returns>
        public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
        {
            return SteamRemoteStorage.FileWriteStreamClose(writeHandle);
        }

        /// <summary>
        /// 写入文件流块，可以多次调用，续写数据
        /// </summary>
        /// <param name="writeHandle">写入句柄</param>
        /// <param name="data">内容</param>
        /// <param name="length">内容长度</param>
        /// <returns>是否成功</returns>
        public static bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] data, int length)
        {
            return SteamRemoteStorage.FileWriteStreamWriteChunk(writeHandle, data, length);
        }
    }
}
