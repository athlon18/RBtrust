using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 89.1 The Aitiascope dungeon logic.
    /// </summary>
    public class TheAitiascope : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheAitiascope;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheAitiascope;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
