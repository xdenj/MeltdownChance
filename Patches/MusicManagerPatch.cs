using PizzaTowerEscapeMusic;
using HarmonyLib;

namespace MeltdownChance.Patches
{
    [HarmonyPatch(typeof(PizzaTowerEscapeMusic.MusicManager))]
    internal class MusicManagerPatch
    {

        [HarmonyPatch("PlayMusic")]
        [HarmonyPrefix]
        private static bool PlayMusicPatch()
        {
            bool hasMeltdownStarted = false; // Default to false to ensure variable is always initialized.

            // Simplify determination of hasMeltdownStarted
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

            return hasMeltdownStarted;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch()
        {
            bool hasMeltdownStarted = false; // Default to false to ensure variable is always initialized.

            // Simplify determination of hasMeltdownStarted
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

            if (hasMeltdownStarted)
            {
                PizzaTowerEscapeMusic.MusicManager musicManager = UnityEngine.Object.FindAnyObjectByType<PizzaTowerEscapeMusic.MusicManager>();
                if (musicManager != null)
                {
                    musicManager.StopMusic();
                }
            }
        }
    }
}
