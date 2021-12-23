using ff14bot.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trust.Dungeons
{
    /// <summary>
    /// Maintains list of available dungeons and exposes dungeon logic handlers.
    /// </summary>
    internal class DungeonManager
    {
        private readonly Dictionary<ZoneId, AbstractDungeon> availableDungeons;

        /// <summary>
        /// Initializes a new instance of the <see cref="DungeonManager"/> class.
        /// </summary>
        public DungeonManager()
        {
            availableDungeons = new Dictionary<ZoneId, AbstractDungeon>()
            {
                { ZoneId.HolminsterSwitch, new HolminsterSwitch() },
                { ZoneId.DohnMheg, new DohnMheg() },
                { ZoneId.TheQitanaRavel, new TheQitanaRavel() },
                { ZoneId.MalikahsWell, new MalikahsWell() },
                { ZoneId.MtGulg, new MtGulg() },
                { ZoneId.Amaurot, new Amaurot() },
                { ZoneId.TheGrandCosmos, new TheGrandCosmos() },
            };
        }

        /// <summary>
        /// Executes dungeon logic for the current dungeon.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public async Task<bool> RunAsync()
        {
            if (availableDungeons.TryGetValue((ZoneId)WorldManager.ZoneId, out AbstractDungeon currentDungeon))
            {
                return await currentDungeon.RunAsync();
            }

            return false;
        }
    }
}
