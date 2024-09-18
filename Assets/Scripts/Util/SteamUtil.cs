using Steamworks;

namespace Util
{
    public static class SteamUtil
    {
        public static CSteamID GetSteamID()
        {
            return SteamUser.GetSteamID();
        } 

        public static string GetSteamName()
        {
            return SteamFriends.GetPersonaName();
        }
    }
}
