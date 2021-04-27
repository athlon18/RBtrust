using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using GreyMagic;
using System;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("EquipRecommended")]
    public class EquipRecommendedTag : AbstractTaskTag
    {
        private readonly string _windowName = "RecommendEquip";
        private readonly string _agentOffset = "Search 48 8D 05 ? ? ? ? C6 43 ? ? 48 89 03 48 8B C3 C7 43 ? ? ? ? ? Add 3 TraceRelative";

        protected override async Task<bool> RunAsync()
        {
            AtkAddonControl window = RaptureAtkUnitManager.GetWindowByName(_windowName);

            if (window == null)
            {
                PatternFinder patternFinder = new PatternFinder(Core.Memory);
                IntPtr agentVtable = patternFinder.Find(_agentOffset);
                int agentId = AgentModule.FindAgentIdByVtable(agentVtable);

                AgentModule.GetAgentInterfaceById(agentId).Toggle();
                await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetWindowByName(_windowName) != null);

                window = RaptureAtkUnitManager.GetWindowByName(_windowName);
            }

            if (window != null)
            {
                window.SendAction(1, 3, 0);
                await Coroutine.Sleep(1000);
            }

            return false;
        }
    }
}