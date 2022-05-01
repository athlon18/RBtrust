using ff14bot.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trust.Data;

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
                // 5.0 Shadowbringers
                { ZoneId.HolminsterSwitch, new HolminsterSwitch() },
                { ZoneId.DohnMheg, new DohnMheg() },
                { ZoneId.TheQitanaRavel, new TheQitanaRavel() },
                { ZoneId.MalikahsWell, new MalikahsWell() },
                { ZoneId.MtGulg, new MtGulg() },
                { ZoneId.Amaurot, new Amaurot() },
                { ZoneId.TheGrandCosmos, new TheGrandCosmos() },
                { ZoneId.AnamnesisAnyder, new AnamnesisAnyder() },
                { ZoneId.TheHeroesGauntlet, new TheHeroesGauntlet() },
                { ZoneId.MatoyasRelict, new MatoyasRelict() },
                { ZoneId.Paglthan, new Paglthan() },

                // 6.0 Endwalker
                { ZoneId.TheTowerOfZot, new TheTowerOfZot() },
                { ZoneId.TheTowerOfBabil, new TheTowerOfBabil() },
                { ZoneId.Vanaspati, new Vanaspati() },
                { ZoneId.KtisisHyperboreia, new KtisisHyperboreia() },
                { ZoneId.TheAitiascope, new TheAitiascope() },
                { ZoneId.TheMothercrystal, new TheMothercrystal() },
                { ZoneId.TheDeadEnds, new TheDeadEnds() },
                { ZoneId.AlzadaalsLegacy, new AlzadaalsLegacy() },
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
