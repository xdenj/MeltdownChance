using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MeltdownChance.Patches;
using System;
using System.Reflection;
using UnityEngine;

namespace MeltdownChance
{
    [BepInPlugin(MODGUID, MODNAME, MODVERSION)]
    [BepInDependency("me.loaforc.facilitymeltdown")]
    [BepInDependency(LethalLib.Plugin.ModGUID, LethalLib.Plugin.ModVersion)]
    public class MeltdownChanceBase : BaseUnityPlugin
    {
        internal const string MODGUID = "den.meltdownchance";
        internal const string MODNAME = "Meltdown Chance";
        internal const string MODVERSION = "2.5.1";

        private ConfigEntry<int> configChance;
        private ConfigEntry<bool> configMessage;

        public readonly Harmony harmony = new(MODGUID);

        internal static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(MeltdownChanceBase.MODGUID);

        public static bool EnableMeltdown;
        public static bool FirstPickUp;
        public static bool isCompany;
        public static int configChanceValue;
        public static bool configMessageValue;
        public static bool isHost;
  

        internal static MeltdownChanceBase? instance;
        public static GameObject MeltdownChanceManagerPrefab = null!;


        void Awake()
        {
            if (instance == null) instance = this;
            else return;

            BindConfigs();
            NetcodePatcher();
            InitializePrefabs();

            configChanceValue = Math.Max(0, Math.Min(configChance.Value, 100));
            configMessageValue = configMessage.Value;

            ResetMeltdownChance();
            ApplyPatches();
        }

        private void InitializePrefabs()
        {
            MeltdownChanceManagerPrefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab("MeltdownChance Manager");
            MeltdownChanceManagerPrefab.AddComponent<MeltdownChanceBehaviour>();
        }

        private static void NetcodePatcher()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {

                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }

        internal static void ResetMeltdownChance()
        {
            EnableMeltdown = true;
            FirstPickUp = false;
            isCompany = false;
        }

        internal void ApplyPatches()
        {
            TryPatches(typeof(StartOfRoundPatch), "StartOfRound");
            TryPatches(typeof(MeltdownHandlerPatch), "FacilityMeltdown");
            TryPatches(typeof(EquipApparaticePatch), "EquipApparatice");
#if DEBUG
            TryPatches(typeof(ItemDropshipPatch), "ItemDropship");
#endif
        }

        internal void TryPatches(Type patchType, string name)
        {
            try
            {
                harmony.PatchAll(patchType);
                logger.LogInfo($"{name} successfully patched!");
            }
            catch (Exception e)
            {
                logger.LogError($"Couldn't patch {name}!!!:\n{e}");
            }
        }

        internal void BindConfigs()
        {
            configChance = Config.Bind(
        "General",                          // Config section
        "MeltdownChance",                     // Key of this config
        100,                    // Default value
        "Chance in percent (0 - 100) at which Meltdowns should occur"    // Description
);

            configMessage = Config.Bind(
                    "General",                  // Config subsection
                    "DisplayPopup",                  // Key of this config
                    true,                               // Default value
                    "Display Meltdown Chance popup when picking up the Apparatus"         // Description
            );
        }
    }
}