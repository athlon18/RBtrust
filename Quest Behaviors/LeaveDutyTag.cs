using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("LeaveDuty")]
    public class LeaveDutyTag : AbstractTaskTag
    {
        protected override async Task<bool> RunAsync()
        {
            DutyManager.LeaveActiveDuty();
            await Coroutine.Sleep(5000);

            return false;
        }
    }
}