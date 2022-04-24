using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using GreyMagic;
using System;
using System.Threading.Tasks;
using Trust.Data;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Queues for the specified Trust dungeon.
    /// </summary>
    [XmlElement("QueueTrust")]
    public partial class QueueTrustTag : AbstractTaskTag
    {
        private readonly string windowName = "Dawn";
        private readonly string agentOffset = "Search 48 8D 05 ? ? ? ? 48 C7 43 ? ? ? ? ? 48 8D 4B ? 48 89 03 66 C7 43 ? ? ? Add 3 TraceRelative";

        /// <summary>
        /// Gets or sets Trust dungeon to queue for.
        /// </summary>
        [XmlAttribute("Dungeon")]
        public DungeonId Dungeon { get; set; }


        [Clio.XmlEngine.XmlAttribute("AutoChooseDungeon")]
        private bool AutoChooseDungeon { get; set; }


        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            AtkAddonControl window = RaptureAtkUnitManager.GetWindowByName(windowName);

            if (window == null)
            {
                PatternFinder patternFinder = new PatternFinder(Core.Memory);
                IntPtr agentVtable = patternFinder.Find(agentOffset);
                int agentId = AgentModule.FindAgentIdByVtable(agentVtable);

                AgentModule.GetAgentInterfaceById(agentId).Toggle();
                await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetWindowByName(windowName) != null);

                window = RaptureAtkUnitManager.GetWindowByName(windowName);
            }

            if (window != null && window.IsVisible)
            {
                await Coroutine.Sleep(3000);
                // Choose Dungeon
                if (AutoChooseDungeon)
                {
                    window.SendAction(2, 3, 15, 4, (ulong)Dungeon - 1);
                    await Coroutine.Sleep(250);
                }          

                // Register for Duty
                window.SendAction(1, 3, 14);
            }

            return false;
        }
    }
}
