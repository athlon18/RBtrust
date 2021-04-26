using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using GreyMagic;
using System;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("QueueTrust")]
    public class QueueTrustTag : AbstractTaskTag
    {
        [XmlAttribute("Dungeon")]
        public DungeonId Dungeon { get; set; }

        private readonly string _windowName = "Dawn";
        private readonly string _agentOffset = "Search 48 8D 05 ? ? ? ? 48 C7 43 ? ? ? ? ? 48 8D 4B ? 48 89 03 66 C7 43 ? ? ? Add 3 TraceRelative";

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
                // Choose Dungeon
                window.SendAction(2, 3, 15, 4, (ulong)Dungeon - 1);
                await Coroutine.Sleep(250);

                // Register for Duty
                window.SendAction(1, 3, 14);
            }

            return false;
        }

        /// <summary>
        /// Static map of dungeon names to IDs.
        /// </summary>
        public enum DungeonId : uint
        {
            /// <summary>
            /// Lv. 71: Holminster Switch
            /// </summary>
            HolminsterSwitch = 1,

            /// <summary>
            /// Lv. 73: Dohn Mheg
            /// </summary>
            DohnMheg = 2,

            /// <summary>
            /// Lv. 75: The Qitana Ravel
            /// </summary>
            TheQitanaRavel = 3,

            /// <summary>
            /// Lv. 77: Malikah's Well
            /// </summary>
            MalikahsWell = 4,

            /// <summary>
            /// Lv. 79: Mt. Gulg
            /// </summary>
            MtGulg = 5,

            /// <summary>
            /// Lv. 80: Amaurot
            /// </summary>
            Amaurot = 6,

            /// <summary>
            /// Lv. 80: The Grand Cosmos
            /// </summary>
            TheGrandCosmos = 7,

            /// <summary>
            /// Lv. 80: Anamnesis Anyder
            /// </summary>
            AnamnesisAnyder = 8,

            /// <summary>
            /// Lv. 80: The Heroes' Gauntlet
            /// </summary>
            TheHeroesGauntlet = 9,

            /// <summary>
            /// Lv. 80: Matoya's Relict
            /// </summary>
            MatoyasRelict = 10,

            /// <summary>
            /// Lv. 80: Paglth'an
            /// </summary>
            Paglthan = 11,
        }
    }
}