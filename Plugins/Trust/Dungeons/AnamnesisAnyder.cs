using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80.3 Anamnesis Anyder dungeon logic.
    /// </summary>
    public class AnamnesisAnyder : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.AnamnesisAnyder;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.AnamnesisAnyder;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
