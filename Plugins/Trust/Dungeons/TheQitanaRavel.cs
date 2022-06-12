using ff14bot.Managers;
using RBTrust.Plugins.Trust.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

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

        private readonly HashSet<uint> spellCastIds = new HashSet<uint>()
        {
            15918, 15916, 15917, 17223, 15498,
            15500, 15725, 15501, 15503, 15504,
            15509, 15510, 15511, 15512, 17213,
            15570, 16263, 14730, 15514, 15516,
            15517, 15518, 15519, 15520, 16923,
            15523, 15527, 15522, 15526, 15525,
            15524,
        };

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheQitanaRavel;

        // removed trash mob / no stack boss spells
        //          15926 Forgiven Violence - SinSpitter
        //          16260 Echo of Qitana - Self-destruct
        //          15502 Lozatl - Heat Up
        //          15499 Lozatl - Lozatl's Scorn

        // not sure if can detect these two as separate spells?
        //          15524 Eros - Confession of Faith (Stack)
        //          15521 Eros - Confession of Faith (Spread)

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            if (spellCastIds.IsCasting())
            {
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            return false;
        }
    }
}
