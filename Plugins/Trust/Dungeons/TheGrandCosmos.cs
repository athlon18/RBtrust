using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80 The Grand Cosmos dungeon logic.
    /// </summary>
    public class TheGrandCosmos : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheGrandCosmos;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheGrandCosmos;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
