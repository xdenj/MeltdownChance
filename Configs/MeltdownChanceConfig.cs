using BepInEx.Configuration;

namespace MeltdownChance.Configs
{
    public class MeltdownChanceConfig
    {
        public static ConfigEntry<int> configChance;
        public static ConfigEntry<bool> configMessage;

        public MeltdownChanceConfig(ConfigFile cfg)
        {
            configChance = cfg.Bind("General", "MeltdownChance", 100, "Chance in percent (0 - 100) at which Meltdowns should occur");
            configMessage = cfg.Bind("General", "DisplayPopup", true, "Display Meltdown Chance popup when picking up the Apparatus");
        }
    }
}
