﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MeltdownChance.Patches;
using System;
using System.Reflection;
using UnityEngine;
using MeltdownChance.Configs;

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

        public static new MeltdownChanceConfig MyConfig { get; internal set; }

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
            MyConfig = new(base.Config);
            if (instance == null) instance = this;
            else return;

            NetcodePatcher();
            InitializePrefabs();
            

            configChanceValue = Math.Max(0, Math.Min(MeltdownChanceConfig.configChance.Value, 100));
            configMessageValue = MeltdownChanceConfig.configMessage.Value;

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

#if DEBUG
        internal void DebugDisplay()
        {
            string readMeltdown = ""; 
            if (MeltdownChanceBehaviour.Instance is { } meltdownChanceBehaviourInstance)
            {
                readMeltdown = meltdownChanceBehaviourInstance.IsMeltdown.ToString();
            }
            else
            {
                readMeltdown = "Couldn't read from NetworkVariable (null)";
            }

            DialogueSegment[] dialogue = new DialogueSegment[1];
            dialogue[0] = new DialogueSegment { bodyText = readMeltdown, speakerText = "NetworkVariable bool:IsMeltdown" };
            HUDManager.Instance.ReadDialogue(dialogue);
        }
#endif
    }
}