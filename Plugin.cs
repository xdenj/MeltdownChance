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
        internal const string MODVERSION = "2.5.0";

        public readonly Harmony harmony = new(MODGUID);

        private ConfigEntry<int>? configChance;
        internal static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(MeltdownChanceBase.MODGUID);

        public static bool EnableMeltdown;
        public static bool FirstPickUp;
        public static int configChanceValue;
        public static bool isHost;

        internal static MeltdownChanceBase? instance;
        public static GameObject MeltdownChanceManagerPrefab = null!;


        void Awake()
        {
            if (instance == null) instance = this;
            else return;

            NetcodePatcher();
            InitializePrefabs();

            configChance = Config.Bind("General", "MeltdownChance", 100, "Chance in % at which meltdowns occur when picking up the apparatice");
            configChanceValue = Math.Max(0, Math.Min(configChance.Value, 100));

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
            EnableMeltdown = false;
            FirstPickUp = false;
        }

        internal void ApplyPatches()
        {
            PatchAllSafely(typeof(StartOfRoundPatch), "StartOfRound");
            PatchAllSafely(typeof(MeltdownHandlerPatch), "FacilityMeltdown");
            PatchAllSafely(typeof(EquipApparaticePatch), "EquipApparatice");
        }

        private void PatchAllSafely(Type patchType, string name)
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
    }
}