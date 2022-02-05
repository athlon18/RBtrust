using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 81 The Tower of Zot dungeon logic.
    /// </summary>
    public class TheTowerOfZot : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheTowerOfZot;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheTowerOfZot;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
