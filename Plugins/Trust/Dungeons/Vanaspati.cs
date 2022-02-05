using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 85 Vanaspati dungeon logic.
    /// </summary>
    public class Vanaspati : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.Vanaspati;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.Vanaspati;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
