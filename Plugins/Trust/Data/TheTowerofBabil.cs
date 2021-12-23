using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Trust
{
    public class TheTowerofBabil
    {
        public static async Task<bool> Run()
        {

            /// SideStep (回避)
            Helpers.BossIds.ToggleSideStep();

            return false;
        }
    }
}
