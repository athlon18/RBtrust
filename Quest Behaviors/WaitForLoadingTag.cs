using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Directors;
using ff14bot.Managers;
using ff14bot.RemoteAgents;
using ff14bot.RemoteWindows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("WaitForLoading")]
    public class WaitForLoadingTag : AbstractTaskTag
    {
        /// <summary>
        /// Instanced content-specific Director.  <see langword="null"/> if not inside a relevant instance.
        /// </summary>
        private static InstanceContentDirector InstanceDirector => DirectorManager.ActiveDirector as InstanceContentDirector;

        /// <summary>
        /// <see langword="true"/> if in instance, regardless of stage of loading (black screen, barrier, duty commenced, etc).
        /// </summary>
        public static bool IsInInstance => InstanceDirector != null;

        /// <summary>
        /// <see langword="true"/> if instance is fully loaded, dungeon barrier is down, and "DUTY COMMENCED" has displayed.
        /// </summary>
        public static bool IsDutyCommenced
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

        protected override async Task<bool> RunAsync()
        {
            if (CommonBehaviors.IsLoading)
            {
                await Coroutine.Wait(Timeout.Infinite, () => !CommonBehaviors.IsLoading);
            }

            if (QuestLogManager.InCutscene)
            {
                AgentCutScene cutscene = AgentCutScene.Instance;
                if (cutscene != null && cutscene.CanSkip)
                {
                    cutscene.PromptSkip();

                    await Coroutine.Wait(1000, () => SelectString.IsOpen);
                    if (SelectString.IsOpen)
                    {
                        SelectString.ClickSlot(0);
                    }
                }
            }

            if (IsInInstance)
            {
                if (!IsDutyCommenced)
                {
                    await Coroutine.Wait(TimeSpan.FromMinutes(5), () => IsDutyCommenced);
                }
            }

            return false;
        }

        /// <summary>
        /// Meanings of individual bits in InstanceFlags.
        /// </summary>
        [Flags]
        private enum InstanceFlags : byte
        {
            LOADED = 0b0000_0100,
            BARRIER_DOWN = 0b0000_1000,
            DUTY_COMMENCED = 0b0001_0000,
        }
    }
}