using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 83 The Tower of Babil dungeon logic.
    /// </summary>
    public class TheTowerOfBabil : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheTowerOfBabil;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheTowerOfBabil;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
