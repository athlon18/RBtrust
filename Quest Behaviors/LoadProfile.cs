using Clio.XmlEngine;
using TreeSharp;
using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles
{
    [XmlElement("LoadProfile")]

    public class LoadProfileTag : ProfileBehavior
    {
        private bool _done;

        public override bool IsDone => _done;

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => TreeRoot.IsRunning,
                    new Action(r =>
                    {
                        NeoProfileManager.Load(NeoProfileManager.CurrentProfile.Path, true);
                        NeoProfileManager.UpdateCurrentProfileBehavior();
                        _done = true;
                    })
                )
            );
        }

        protected override void OnStart()
        {

        }

        /// <summary>
        /// This gets called when a while loop starts over so reset anything that is used inside the IsDone check
        /// </summary>
        protected override void OnResetCachedDone()
        {
            _done = false;
        }

        protected override void OnDone()
        {

        }
    }
}