using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 87 Ktisis Hyperboreia dungeon logic.
    /// </summary>
    public class KtisisHyperboreia : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.KtisisHyperboreia;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.KtisisHyperboreia;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
