using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TreeSharp;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Accepts Duty Finder queue and waits for loading to finish.
    /// </summary>
    [XmlElement("CommenceDuty")]
    internal class CommenceDutyTag : ProfileBehavior
    {
        private bool done = false;

        /// <summary>
        /// Gets or sets maximum time to wait for Commence Duty window to appear, in seconds.
        /// </summary>
        [XmlAttribute("WaitTime")]
        [DefaultValue(60)]
        public int WaitTime { get; set; }

        /// <inheritdoc/>
        public override bool IsDone => done;

        /// <inheritdoc/>
        protected override void OnResetCachedDone()
        {
            done = false;
        }

        /// <inheritdoc/>
        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                CommonBehaviors.HandleLoading,
                new Decorator(ret => QuestLogManager.InCutscene, new ActionAlwaysSucceed()),
                new ActionRunCoroutine(r => TheTask()));
        }

        private async Task<bool> TheTask()
        {
            if (await Coroutine.Wait(WaitTime * 1000, () => ContentsFinderConfirm.IsOpen))
            {
                await Coroutine.Yield();
                while (ContentsFinderConfirm.IsOpen)
                {
                    DutyManager.Commence();
                    await Coroutine.Yield();
                    if (await Coroutine.Wait(30000, () => CommonBehaviors.IsLoading))
                    {
                        await Coroutine.Yield();
                        await Coroutine.Wait(Timeout.Infinite, () => !CommonBehaviors.IsLoading);
                        done = true;
                        return true;
                    }
                }

                Log("After commencing, load screen never occurred. Stopping Bot");

                done = true;
                return true;
            }

            Log("Duty finder didn't pop within {0} seconds.", WaitTime);

            done = true;

            return true;
        }
    }
}
