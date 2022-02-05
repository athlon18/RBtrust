using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Managers;
using System.Threading.Tasks;
using TreeSharp;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Accepts pending duty finder commence windows.
    /// </summary>
    [XmlElement("JoinDutyFinder")]
    internal class JoinDutyFinderTag : ProfileBehavior
    {
        private bool done = false;

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
            await ExtendedContentsFinder.Open();

            await Coroutine.Yield();

            if (await ExtendedContentsFinder.Join())
            {
                done = true;
                return true;
            }

            Log("Could not join duty finder. Stopping bot.");

            return false;
        }
    }
}
