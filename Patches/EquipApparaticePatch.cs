using HarmonyLib;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(LungProp))]
    internal class EquipApparaticePatch
    {
        [HarmonyPatch("EquipItem")]
        [HarmonyPostfix]
        //[HarmonyAfter("me.loaforc.facilitymeltdown")]
        internal static void DisplayMessage(LungProp __instance)
        {
            bool isInFactory = __instance.isInFactory;
            if (MeltdownChanceBehaviour.Instance == null)
            {
                MeltdownChanceBase.logger.LogWarning("MeltdownChanceBehaviour.Instance is not yet available. (NULL)");
                return;
            }
            else
            {
                bool hasMeltdownStarted;
                if (MeltdownChanceBase.isHost)
                {
                    hasMeltdownStarted = MeltdownChanceBase.EnableMeltdown;
                    MeltdownChanceBehaviour.Instance.IsMeltdown = MeltdownChanceBase.EnableMeltdown;
                }
                else
                {
                    hasMeltdownStarted = (bool)MeltdownChanceBehaviour.Instance.IsMeltdown;
                }

                MeltdownChanceBase.logger.LogDebug($"Is player host: {MeltdownChanceBase.isHost} | Is apparatice inside Facility?: {isInFactory} | Has meltdown started?: {hasMeltdownStarted}.");

                if (MeltdownChanceBase.FirstPickUp && isInFactory)
                {
                    MeltdownChanceBase.FirstPickUp = false;
                    if (hasMeltdownStarted)
                    {
                        HUDManager.Instance.DisplayTip("<color=red>Reactor unstable!</color>", "Meltdown imminent!", true, false, "LC_Tip1");
                    }
                    else
                    {
                        HUDManager.Instance.DisplayTip("Reactor stable!", "Radiation leaks detected!", false, false, "LC_Tip1");
                    }
                }
            }

        }
    }
}