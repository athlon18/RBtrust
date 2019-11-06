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
    public class ExtendedContentsFinder
    {
        public static bool IsOpen
        {
            get
            {
                return RaptureAtkUnitManager.GetWindowByName("ContentsFinder") != null;
            }
        }
        public static async Task<bool> Open()
        {
            while (!IsOpen)
            {
                ChatManager.SendChat("/dutyfinder");
                await Coroutine.Wait(3000, () => IsOpen);
            }

            return true;
        }

        public static async Task<bool> Join()
        {
            if (!IsOpen) return false;

            RaptureAtkUnitManager.GetWindowByName("ContentsFinder").SendAction(1, 1, 3);
            if (await Coroutine.Wait(5000, () => !IsOpen)) return true;

            return false;
        }
    }

    [XmlElement("JoinDutyFinder")]
    class DutyFinderJoinTag : ProfileBehavior
    {
        private bool _done = false;
        public override bool IsDone { get { return _done; } }
        protected override void OnResetCachedDone() { _done = false; }

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                CommonBehaviors.HandleLoading,
                new Decorator(ret => QuestLogManager.InCutscene, new ActionAlwaysSucceed()),
                new ActionRunCoroutine(r => TheTask())
            );
        }

        private async Task<bool> TheTask()
        {
            await ExtendedContentsFinder.Open();

            await Coroutine.Yield();

            if (await ExtendedContentsFinder.Join())
            {
                _done = true;
                return true;
            }

            Log("Could not join duty finder. Stopping bot.");
            //await TreeRoot.StopGently();
            return false;
        }
    }

    [XmlElement("CommenceDuty")]
    class CommenceDutyTag : ProfileBehavior
    {
        [XmlAttribute("WaitTime")]
        [DefaultValue(60)]
        public int WaitTime { get; set; }

        private bool _done = false;
        public override bool IsDone { get { return _done; } }
        protected override void OnResetCachedDone() { _done = false; }

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                CommonBehaviors.HandleLoading,
                new Decorator(ret => QuestLogManager.InCutscene, new ActionAlwaysSucceed()),
                new ActionRunCoroutine(r => TheTask())
            );
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
                        _done = true;
                        return true;
                    }
                }

                Log("After commencing, load screen never occurred. Stopping Bot");
                //await TreeRoot.StopGently();
                _done = true;
				return true;
            }
            Log("Duty finder didn't pop within {0} seconds.", WaitTime);
            //await TreeRoot.StopGently();
			_done = true;
            return true;
        }
    }
}