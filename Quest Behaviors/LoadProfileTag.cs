using Clio.XmlEngine;
using TreeSharp;
using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles
{
    /// <summary>
    /// Loads the specified OrderBot profile.
    /// </summary>
    [XmlElement("LoadProfile")]
    public class LoadProfileTag : ProfileBehavior
    {
        private bool done;

        /// <inheritdoc/>
        public override bool IsDone => done;

        /// <inheritdoc/>
        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => TreeRoot.IsRunning,
                    new Action(r =>
                    {
                        NeoProfileManager.Load(NeoProfileManager.CurrentProfile.Path, true);
                        NeoProfileManager.UpdateCurrentProfileBehavior();
                        done = true;
                    })));
        }

        /// <inheritdoc/>
        protected override void OnStart()
        {
        }

        /// <summary>
        /// This gets called when a while loop starts over so reset anything that is used inside the IsDone check.
        /// </summary>
        protected override void OnResetCachedDone()
        {
            done = false;
        }

        /// <inheritdoc/>
        protected override void OnDone()
        {
        }
    }
}
