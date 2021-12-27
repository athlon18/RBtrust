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
                { ZoneId.HolminsterSwitch, new HolminsterSwitch() },
                { ZoneId.DohnMheg, new DohnMheg() },
                { ZoneId.TheQitanaRavel, new TheQitanaRavel() },
                { ZoneId.MalikahsWell, new MalikahsWell() },
                { ZoneId.MtGulg, new MtGulg() },
                { ZoneId.Amaurot, new Amaurot() },
                { ZoneId.TheGrandCosmos, new TheGrandCosmos() },

                // TODO: { ZoneId.AnamnesisAnyder, new AnamnesisAnyder() },
                // TODO: { ZoneId.TheHeroesGauntlet, new TheHeroesGauntlet() },
                // TODO: { ZoneId.MatoyasRelict, new MatoyasRelict() },
                // TODO: { ZoneId.Paglthan, new Paglthan() },
                // TODO: { ZoneId.TheTowerOfZot, new TheTowerOfZot() },
                { ZoneId.TheTowerOfBabil, new TheTowerOfBabil() },

                // TODO: { ZoneId.Vanaspati, new Vanaspati() },
                // TODO: { ZoneId.KtisisHyperboreia, new KtisisHyperboreia() },
                // TODO: { ZoneId.TheAitiascope, new TheAitiascope() },
                // TODO: { ZoneId.TheMothercrystal, new TheMothercrystal() },
                // TODO: { ZoneId.TheDeadEnds, new TheDeadEnds() },
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
