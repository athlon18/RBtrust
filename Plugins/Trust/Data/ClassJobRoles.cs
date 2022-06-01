using ff14bot.Enums;
using System.Collections.Generic;

namespace RBTrust.Plugins.Trust.Data
{
    /// <summary>
    /// Groupings of <see cref="ClassJobType"/>s into their roles.
    /// </summary>
    internal static class ClassJobRoles
    {
        /// <summary>
        /// Gets all <see cref="ClassJobType"/>s in the Tank role.
        /// </summary>
        public static readonly HashSet<ClassJobType> Tanks = new HashSet<ClassJobType>()
        {
            ClassJobType.Gladiator,
            ClassJobType.Marauder,
            ClassJobType.Paladin,
            ClassJobType.Gunbreaker,
            ClassJobType.Warrior,
            ClassJobType.DarkKnight,
        };

        /// <summary>
        /// Gets all <see cref="ClassJobType"/>s in the DPS role.
        /// </summary>
        public static readonly HashSet<ClassJobType> DPS = new HashSet<ClassJobType>()
        {
            ClassJobType.Lancer,
            ClassJobType.Archer,
            ClassJobType.Thaumaturge,
            ClassJobType.Pugilist,
            ClassJobType.Monk,
            ClassJobType.Dragoon,
            ClassJobType.Bard,
            ClassJobType.BlackMage,
            ClassJobType.Arcanist,
            ClassJobType.Summoner,
            ClassJobType.Rogue,
            ClassJobType.Ninja,
            ClassJobType.Machinist,
            ClassJobType.Samurai,
            ClassJobType.RedMage,
            ClassJobType.Dancer,
            ClassJobType.Reaper,
        };

        /// <summary>
        /// Gets all <see cref="ClassJobType"/>s in the Healer role.
        /// </summary>
        public static readonly HashSet<ClassJobType> Healers = new HashSet<ClassJobType>()
        {
           ClassJobType.Sage,
           ClassJobType.Astrologian,
           ClassJobType.WhiteMage,
           ClassJobType.Scholar,
           ClassJobType.Conjurer,
        };

        /// <summary>
        /// Gets all <see cref="ClassJobType"/>s that primarily fight in melee range.
        /// </summary>
        public static readonly List<ClassJobType> Melee = new List<ClassJobType>()
        {
            ClassJobType.Lancer,
            ClassJobType.Dragoon,
            ClassJobType.Pugilist,
            ClassJobType.Monk,
            ClassJobType.Rogue,
            ClassJobType.Ninja,
            ClassJobType.Samurai,
            ClassJobType.Reaper,
            ClassJobType.DarkKnight,
            ClassJobType.Gladiator,
            ClassJobType.Marauder,
            ClassJobType.Paladin,
            ClassJobType.Warrior,
            ClassJobType.Gunbreaker,
        };
    }
}
