using System.Threading.Tasks;

namespace Trust.Dungeons
{
    /// <summary>
    /// Abstract starting point for implementing specialized dungeon logic.
    /// </summary>
    public abstract class AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public const ZoneId ZoneId = Dungeons.ZoneId.NONE;

        /// <summary>
        /// Gets <see cref="DungeonId"/> for this dungeon.
        /// </summary>
        public abstract DungeonId DungeonId { get; }

        /// <summary>
        /// Executes dungeon logic.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public abstract Task<bool> RunAsync();
    }
}
