using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using GreyMagic;
using System;
using System.Threading.Tasks;

namespace Trust.Windows
{
    /// <summary>
    /// "Recommended Gear" in character sheet.
    /// </summary>
    internal static class RecommendEquip
    {
        private static readonly string WindowName = "RecommendEquip";
        private static readonly string AgentOffset = "Search 48 8D 05 ? ? ? ? C6 43 ? ? 48 89 03 48 8B C3 C7 43 ? ? ? ? ? Add 3 TraceRelative";

        /// <summary>
        /// Uses the in-game "Equip Recommended" feature to update current character equipment.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task EquipAsync()
        {
            AtkAddonControl window = RaptureAtkUnitManager.GetWindowByName(WindowName);

            if (window == null)
            {
                using (PatternFinder patternFinder = new PatternFinder(Core.Memory))
                {
                    IntPtr agentVtable = patternFinder.Find(AgentOffset);
                    int agentId = AgentModule.FindAgentIdByVtable(agentVtable);

                    AgentModule.GetAgentInterfaceById(agentId).Toggle();
                }

                await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetWindowByName(WindowName) != null);

                window = RaptureAtkUnitManager.GetWindowByName(WindowName);
            }

            if (window != null)
            {
                window.SendAction(1, 3, 0);
                await Coroutine.Sleep(1000);
            }
        }
    }
}
