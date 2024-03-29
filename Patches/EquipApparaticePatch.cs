using HarmonyLib;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(LungProp))]
    internal class EquipApparaticePatch
    {
        [HarmonyPatch(nameof(LungProp.EquipItem))]
        [HarmonyPostfix]
        //[HarmonyAfter("me.loaforc.facilitymeltdown")]
        internal static void DisplayMessage(LungProp __instance)
        {
            bool isInFactory = __instance.isInFactory;
            bool hasMeltdownStarted = false; // Default to false to ensure variable is always initialized.

            // Simplify determination of hasMeltdownStarted by using conditional logic.
            if (MeltdownChanceBase.isHost)
            {
                hasMeltdownStarted = MeltdownChanceBase.EnableMeltdown;
            }
            else if (MeltdownChanceBehaviour.Instance is { } meltdownChanceBehaviourInstance)
            {
                hasMeltdownStarted = meltdownChanceBehaviourInstance.IsMeltdown;
            }
            else
            {
                MeltdownChanceBase.logger.LogWarning("MeltdownChanceBehaviour instance is null, it might not have been instantiated or is otherwise unavailable. IsMeltdown flag cannot be read. Client players won't see the correct message when the apparatus is pulled.");
            }

            // Improved logging with clearer context.
            MeltdownChanceBase.logger.LogDebug($"Is player host: {MeltdownChanceBase.isHost} | Is apparatus inside Facility?: {isInFactory} | Has meltdown started?: {hasMeltdownStarted}.");

            if (MeltdownChanceBase.FirstPickUp && isInFactory)
            {
                MeltdownChanceBase.FirstPickUp = false;
                string tipTitle = hasMeltdownStarted ? "<color=red>Reactor unstable!</color>" : "<color=green>Reactor stable!</color>";
                string tipMessage = hasMeltdownStarted ? "Meltdown imminent! Evacuate facility immediately!" : "Leaks detected. Radiation levels rising!";
                HUDManager.Instance.DisplayTip(tipTitle, tipMessage, hasMeltdownStarted, false, "LC_Tip1");
            }
        }
    }
}