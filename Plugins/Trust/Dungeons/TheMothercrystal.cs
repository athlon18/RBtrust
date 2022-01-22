using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 89.2 The Mothercrystal dungeon logic.
    /// </summary>
    public class TheMothercrystal : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheMothercrystal;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheMothercrystal;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
