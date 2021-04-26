using System.Threading.Tasks;
using TreeSharp;

namespace ff14bot.NeoProfiles.Tags
{
    public abstract class AbstractTaskTag : ProfileBehavior
    {
        protected bool _isDone;
        public override bool IsDone => _isDone;

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(r => TreeRoot.IsRunning,
                    new PrioritySelector(
                        new ActionRunCoroutine(r => RunAsync()),
                    new TreeSharp.Action(r => _isDone = true)
                        )
                )
            );
        }
        protected override void OnResetCachedDone()
        {
            _isDone = false;
        }

        /// <summary>
        /// Executes OrderBot tag's logic.
        /// </summary>
        protected abstract Task<bool> RunAsync();
    }
}
