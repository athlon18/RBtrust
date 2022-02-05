using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80.5 Matoya's Relict dungeon logic.
    /// </summary>
    public class MatoyasRelict : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.MatoyasRelict;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.MatoyasRelict;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
