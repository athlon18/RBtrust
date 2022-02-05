using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80.6 Paglth'an dungeon logic.
    /// </summary>
    public class Paglthan : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.Paglthan;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.Paglthan;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
