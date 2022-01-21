using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 90.1 The Dead Ends dungeon logic.
    /// </summary>
    public class TheDeadEnds : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheDeadEnds;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheDeadEnds;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
