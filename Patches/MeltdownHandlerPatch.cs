﻿using FacilityMeltdown.MeltdownSequence.Behaviours;
using HarmonyLib;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(MeltdownHandler))]
    internal class MeltdownHandlerPatch
    {

        [HarmonyPatch("MeltdownReadyServerRpc")]
        [HarmonyPrefix]
        private static bool MeltdownReadyServerRpcPatch()
        {
            if (MeltdownChanceBase.isHost)
            {
                return MeltdownChanceBase.EnableMeltdown;
            }
            return true;

        }

        [HarmonyPatch("StartMeltdownClientRpc")]
        [HarmonyPrefix]
        private static bool StartMeltdownClientRpcPrePatch()
        {
            if (MeltdownChanceBase.isHost)
            {
                return MeltdownChanceBase.EnableMeltdown;
            }
            return true;
        }
    }
}
