using HarmonyLib;

namespace MeltdownChance.Patches
{
#if DEBUG
    [HarmonyPatch(typeof(ItemDropship))]
    internal class ItemDropshipPatch
    {
        [HarmonyPatch(nameof(ItemDropship.TryOpeningShip))]
        [HarmonyPostfix]
        internal static void TryOpeningShipPatch(ItemDropship __instance)
        {
            if (MeltdownChanceBehaviour.Instance is { } meltdownChanceBehaviourInstance)
            {
                MeltdownChanceBase.logger.LogDebug($"Read from IsMeltdown: {meltdownChanceBehaviourInstance.IsMeltdown}");
            }
            else
            {
                MeltdownChanceBase.logger.LogDebug("MeltdownChanceBehaviour instance is null, it might not have been instantiated or is otherwise unavailable. IsMeltdown flag cannot be read. Client players won't see the correct message when the apparatus is pulled.");
            }

        }
    }
#endif
}