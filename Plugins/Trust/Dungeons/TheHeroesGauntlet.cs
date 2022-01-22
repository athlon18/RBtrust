using System.Threading.Tasks;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80.4 The Heroes' Gauntlet dungeon logic.
    /// </summary>
    public class TheHeroesGauntlet : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheHeroesGauntlet;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheHeroesGauntlet;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            return false;
        }
    }
}
