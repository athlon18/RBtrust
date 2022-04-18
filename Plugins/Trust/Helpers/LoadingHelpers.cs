using Buddy.Coroutines;
using ff14bot.Behavior;
using ff14bot.Directors;
using ff14bot.Managers;
using ff14bot.RemoteAgents;
using ff14bot.RemoteWindows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to loading, cutscenes, and other waiting situations.
    /// </summary>
    public static class LoadingHelpers
    {
        /// <summary>
        /// Gets a value indicating whether in instance, regardless of stage of loading (black screen, barrier, duty commenced, etc).
        /// </summary>
        public static bool IsInInstance => InstanceDirector != null;

        /// <summary>
        /// Gets instanced content-specific Director.  <see langword="null"/> if not inside a relevant instance.
        /// </summary>
        private static InstanceContentDirector InstanceDirector => DirectorManager.ActiveDirector as InstanceContentDirector;

        /// <summary>
        /// Gets a value indicating whether instance is fully loaded, dungeon barrier is down, and "DUTY COMMENCED" has displayed.
        /// </summary>
        private static bool IsDutyCommenced
        {
            get
            {
                if (InstanceDirector != null)
                {
                    InstanceFlags flags = (InstanceFlags)InstanceDirector.InstanceFlags;
                    return flags.HasFlag(InstanceFlags.DUTY_COMMENCED);
                }

                return false;
            }
        }

        /// <summary>
        /// Waits for the game to finish loading.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task WaitForLoadingAsync()
        {
            if (CommonBehaviors.IsLoading)
            {
                await Coroutine.Wait(Timeout.Infinite, () => !CommonBehaviors.IsLoading);
            }
        }

        /// <summary>
        /// Skips any pending cutscenes.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task SkipCutsceneAsync()
        {
            if (QuestLogManager.InCutscene)
            {
                AgentCutScene cutscene = AgentCutScene.Instance;
                if (cutscene != null && cutscene.CanSkip)
                {
                    cutscene.PromptSkip();

                    await Coroutine.Wait(TimeSpan.FromSeconds(5), () => SelectString.IsOpen);
                    if (SelectString.IsOpen)
                    {
                        SelectString.ClickSlot(0);
                    }
                }
            }
        }

        /// <summary>
        /// Waits for duty to commence.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task WaitForDutyCommencedAsync()
        {
            if (IsInInstance)
            {
                if (!IsDutyCommenced)
                {
                    await Coroutine.Wait(TimeSpan.FromMinutes(5), () => IsDutyCommenced);
                }
            }
        }
    }
}
