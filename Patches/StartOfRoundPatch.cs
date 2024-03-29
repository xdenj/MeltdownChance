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


        [HarmonyPatch("Start")]
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
                MeltdownChanceBase.logger.LogInfo("Spawning MeltdownChanceBehaviour.");
            }
            catch (Exception e)
            {
                MeltdownChanceBase.logger.LogError($"Failed to spawn MeltdownChanceBehaviour:\n{e}");
            }
        }



        [HarmonyPatch("OnShipLandedMiscEvents")]
        [HarmonyPostfix]
        static void OnShipLandedMiscEventsPatch()
        {
            MeltdownChanceBase.ResetMeltdownChance();
            MeltdownChanceBase.FirstPickUp = true;
            MeltdownChanceBase.isHost = GameNetworkManager.Instance.isHostingGame;

            rand = random.Next(0, 100);

            if (rand <= MeltdownChanceBase.configChanceValue)
            {
                MeltdownChanceBase.EnableMeltdown = true;
                MeltdownChanceBase.logger.LogDebug($"Expect a meltdown this round! Chance: {MeltdownChanceBase.configChanceValue}, RNG {rand}");
            }
            else
            {
                MeltdownChanceBase.EnableMeltdown = false;
                MeltdownChanceBase.logger.LogDebug($"Expect no meltdown this round! Chance: {MeltdownChanceBase.configChanceValue}, RNG {rand}");
            }
        }


        [HarmonyPatch("ShipHasLeft")]
        [HarmonyPrefix]
        static void ShipHasLeftPatch()
        {
            MeltdownChanceBase.ResetMeltdownChance();
            MeltdownChanceBase.logger.LogInfo("Ship has left, resetting.");
        }

    }
}