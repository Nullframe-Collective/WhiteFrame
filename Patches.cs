using HarmonyLib;
using Il2Cpp;

namespace WhiteFrame
{
    [HarmonyPatch(typeof(WhoKilledWho), "OnPlayerAddedHandler", [typeof(PlayerInfo)])]
    public static class OnPlayerJoinedRoomPatch
    {
        [HarmonyPrefix]
        public static void Prefix(PlayerInfo otherPlayer)
        {
            string nickName = otherPlayer.EKINODNOMKE;
            bool playerInWhitelist = Core.CheckPlayer(nickName);

            if (playerInWhitelist) return;

            if (Core.IsHost)
            {
                Core.Kick(otherPlayer);
            }
        }
    }
}
