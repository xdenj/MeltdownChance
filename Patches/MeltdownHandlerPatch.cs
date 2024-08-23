using FacilityMeltdown.MeltdownSequence.Behaviours;
using HarmonyLib;
using UnityEngine;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(MeltdownHandler))]
    internal class MeltdownHandlerPatch
    {

        [HarmonyPatch("OnNetworkSpawn")]
        [HarmonyPrefix]
        private static bool OnNetworkSpawnPatch()
        {
            if (MeltdownChanceBase.isHost)
            {
                return MeltdownChanceBase.EnableMeltdown;
            }
            return true;

        }

        [HarmonyPatch("StartMeltdownClientRpc")]
        [HarmonyPrefix]
        private static bool StartMeltdownClientRpcPatch()
        {
            if (MeltdownChanceBase.isHost)
            {
                return MeltdownChanceBase.EnableMeltdown;
            }
            return true;
        }
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(AudioSource ___meltdownMusic)
        {

            if (MeltdownChanceBehaviour.Instance is { } meltdownChanceBehaviourInstance)
            {
                //MeltdownChanceBase.logger.LogDebug($"isMeltdown: {meltdownChanceBehaviourInstance.IsMeltdown}");
                if (!meltdownChanceBehaviourInstance.IsMeltdown)
                {
                    MeltdownChanceBase.SupressMusic(___meltdownMusic);
                }

            }
            else
            {
                MeltdownChanceBase.logger.LogDebug($"MeltdownChanceBehaviourInstance not found!");
            }
        }
    }
}