using HarmonyLib;
using System;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        private static readonly System.Random random = new();
        private static int rand;


        [HarmonyPatch(nameof(StartOfRound.Start))]
        [HarmonyPostfix]
        public static void OnSessionStart(StartOfRound __instance)
        {
            if (!__instance.IsOwner) return;
            try
            {
                var MeltdownChanceManager = Object.Instantiate(MeltdownChanceBase.MeltdownChanceManagerPrefab, __instance.transform);
                MeltdownChanceManager.hideFlags = HideFlags.None;
                //MeltdownChanceManager.SetActive(true);
                MeltdownChanceManager.GetComponent<NetworkObject>().Spawn();
            }
            catch (Exception e)
            {
                MeltdownChanceBase.logger.LogError($"Failed to spawn MeltdownChanceBehaviour:\n{e}");
            }
        }


        [HarmonyPatch(nameof(StartOfRound.OnShipLandedMiscEvents))]
        [HarmonyPostfix]
        static void OnShipLandedMiscEventsPatch(StartOfRound __instance)
        {
            MeltdownChanceBase.ResetMeltdownChance();
            MeltdownChanceBase.isCompany = __instance.currentLevel.levelID == 3;
            MeltdownChanceBase.FirstPickUp = true;
            MeltdownChanceBase.isHost = GameNetworkManager.Instance.isHostingGame;

            if (MeltdownChanceBase.isHost && !MeltdownChanceBase.isCompany)
            {
                rand = random.Next(0, 100);
                bool isMeltdown = rand <= MeltdownChanceBase.configChanceValue;
                MeltdownChanceBase.EnableMeltdown = isMeltdown;

                // Refactored logging message to handle both cases
                MeltdownChanceBase.logger.LogDebug($"Expect {(isMeltdown ? "a " : "no ")}meltdown this round! Meltdown Threshold: {MeltdownChanceBase.configChanceValue}, Random Roll: {rand}");

                if (MeltdownChanceBehaviour.Instance is not { } meltdownChanceBehaviourInstance)
                {
                    MeltdownChanceBase.logger.LogWarning("MeltdownChanceBehaviour instance is null, it might not have been instantiated or is otherwise unavailable. IsMeltdown flag cannot be set. Client players won't see the correct message when the apparatus is pulled.");
                }
                else
                {
                    meltdownChanceBehaviourInstance.IsMeltdown = isMeltdown;
                }
            }
        }


        [HarmonyPatch(nameof(StartOfRound.ShipHasLeft))]
        [HarmonyPrefix]
        static void ShipHasLeftPatch()
        {
            MeltdownChanceBase.ResetMeltdownChance();
            MeltdownChanceBase.logger.LogInfo("Ship has left, resetting flags.");
        }

    }
}