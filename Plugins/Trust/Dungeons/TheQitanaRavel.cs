using Buddy.Coroutines;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Trust.Data;
using Trust.Helpers;
using Trust.Extensions;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 75 The Qitana Ravel dungeon logic.
    /// </summary>
    public class TheQitanaRavel : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheQitanaRavel;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheQitanaRavel;

        /// <inheritdoc/>
        ///
        private HashSet<uint> spellCastIds = new HashSet<uint>()
            {
                15918, 15916, 15917, 17223, 15498, 
                15500, 15725, 15501, 15503, 15504,
                15509, 15510, 15511, 15512, 17213, 
                15570, 16263, 14730, 15514, 15516,
                15517, 15518, 15519, 15520, 16923, 
                15523, 15527, 15522, 15526, 15525,
                15524
            };

            // removed trash mob / no stack boss spells
            //          15926 Forgiven Violence - SinSpitter
            //          16260 Echo of Qitana - Self-destruct
            //          15502 Lozatl - Heat Up
            //          15499 Lozatl - Lozatl's Scorn

            //          not sure if can detect these two as separate spells?
            //          15524 Eros - Confession of Faith (Stack)
            //          15521 Eros - Confession of Faith (Spread)

        CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();
        public override async Task<bool> RunAsync()
        {      
             
            if (spellCastIds.IsCasting())
            {
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }
            return false;
        }
    }
}
