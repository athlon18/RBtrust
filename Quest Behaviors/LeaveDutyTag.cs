using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Leaves the current duty.
    /// </summary>
    [XmlElement("LeaveDuty")]
    public class LeaveDutyTag : AbstractTaskTag
    {
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            DutyManager.LeaveActiveDuty();
            await Coroutine.Sleep(5000);

            return false;
        }
    }
}
