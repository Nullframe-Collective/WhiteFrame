using Il2Cpp;
using Il2CppCodeStage.AntiCheat.Storage;
using Il2CppFishNet;
using Il2CppFishNet.Managing.Server;
using MelonLoader;

[assembly: MelonInfo(typeof(WhiteFrame.Core), "WhiteFrame", "1.0.0", "Nullframe Collective", null)]
[assembly: MelonGame("ZeoWorks", "Slendytubbies 3")]

namespace WhiteFrame
{
    public class Core : MelonMod
    {
        public static string Whitelist;
        public static bool IsHost;
        private Config _config;
        private string _nickName;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
            _config = ConfigManager.Load();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "MainMenu" || sceneName == "Updater")
            {
                SetupConfigAsync().Wait();
                _nickName = ObscuredPrefs.GetString("ZWName0001");
            }

            if (RoomProperties.Current != null)
            {
                IsHost = RoomProperties.Current.HostName == _nickName ? true : false;
            }
        }

        private async Task SetupConfigAsync()
        {
            if (!File.Exists(_config.LocalWhitelistPath))
            {
                File.Create(_config.LocalWhitelistPath).Dispose();

                return;
            }

            Whitelist = File.ReadAllText(_config.LocalWhitelistPath);

            if (_config.GlobalWhitelist)
            {
                Whitelist = await DownloadFile(_config.LinkToGlobalWhitelist);
            }
        }

        private static async Task<string> DownloadFile(string uri)
        {
            using (HttpClient httpClient = new())
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }

            return null;
        }

        public static bool CheckPlayer(string nickName)
        {
            if (string.IsNullOrEmpty(Whitelist)) return false;

            string[] blacklist = Whitelist.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

            foreach (string player in blacklist)
            {
                if (player.Equals(nickName, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

        public static void Kick(PlayerInfo playerInfo)
        {
            InstanceFinder.ServerManager.Kick(playerInfo.OwnerId, KickReason.Unset);
        }

        public static void Log(object msg)
        {
            MelonLogger.Msg(msg);
        }
    }
}