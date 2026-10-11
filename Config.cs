using System.Text.Json;

namespace WhiteFrame
{
    public struct Config
    {
        public Config()
        {
            GlobalWhitelist = false;
            LinkToGlobalWhitelist = "";
            LocalWhitelistPath = "UserData/WhiteFrame/Whitelist.txt";
        }

        public bool GlobalWhitelist { get; set; }
        public string LinkToGlobalWhitelist { get; set; }
        public string LocalWhitelistPath { get; set; }
    }

    public static class ConfigManager
    {
        private const string ConfigPath = "UserData/WhiteFrame/Config.json";

        public static Config Load()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));

            if (File.Exists(ConfigPath))
            {
                try
                {
                    var json = File.ReadAllText(ConfigPath);

                    return JsonSerializer.Deserialize<Config?>(json) ?? new Config();
                }
                catch { }
            }

            return Save(new Config());
        }

        public static Config Save(Config config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);

            return config;
        }
    }
}
