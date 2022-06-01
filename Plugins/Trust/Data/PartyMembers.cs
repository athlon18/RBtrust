using System.Collections.Generic;

namespace RBTrust.Plugins.Trust.Data
{
    /// <summary>
    /// Groupings of NPC party members by role.
    /// </summary>
    internal static class PartyMembers
    {
        /// <summary>
        /// Gets NPC IDs of non-tank party members.
        /// </summary>
        public static readonly HashSet<uint> PartyMemberIds = new HashSet<uint>()
        {
            729,   // Y'shtola  :: 雅·修特拉
            1492,  // Urianger  :: 于里昂热
            4130,  // Alphinaud :: 阿尔菲诺
            5239,  // Alisaie   :: 阿莉塞
            8378,  // Y'shtola  :: 雅·修特拉
            8889,  // Ryne      :: 琳
            8917,  // Minfilia  :: 敏菲利亚
            8919,  // Lyna      :: 莱楠
            11264, // Alphinaud's avatar
            11265, // Alisaie's avatar
            11267, // Urianger's avatar
            11268, // Y'shtola's avatar
            11269, // Ryne's avatar
            11270, // Estinien's avatar
            10586, // Venat
            10587, // Venat's Phantom
            10898, // Emet-Selch
            10899, // Hythlodaeus

            // 713,  // Thancred       :: 桑克瑞德
            // 8650, // Crystal Exarch :: 水晶公
            // 11266, // Thancred's avatar
            // 11271, // G'raha Tia's avatar
        };

        /// <summary>
        /// Gets NPC IDs of all possible party members.
        /// </summary>
        public static readonly HashSet<uint> AllPartyMemberIds = new HashSet<uint>()
        {
            729,   // Y'shtola       :: 雅·修特拉
            1492,  // Urianger       :: 于里昂热
            4130,  // Alphinaud      :: 阿尔菲诺
            5239,  // Alisaie        :: 阿莉塞
            8378,  // Y'shtola       :: 雅·修特拉
            8889,  // Ryne           :: 琳
            8917,  // Minfilia       :: 敏菲利亚
            8919,  // Lyna           :: 莱楠
            11264, // Alphinaud's avatar
            11265, // Alisaie's avatar
            11267, // Urianger's avatar
            11268, // Y'shtola's avatar
            11269, // Ryne's avatar
            11270, // Estinien's avatar
            10586, // Venat
            10587, // Venat's Phantom
            10898, // Emet-Selch
            10899, // Hythlodaeus
            9363,  // G'raha Tia
            713,   // Thancred       :: 桑克瑞德
            8650,  // Crystal Exarch :: 水晶公
            11266, // Thancred's avatar
            11271, // G'raha Tia's avatar
        };

        /// <summary>
        /// Gets NPC IDs of DPS party members.
        /// </summary>
        public static readonly HashSet<uint> PartyDpsIds = new HashSet<uint>()
        {
            729,  // Y'shtola    :: 雅·修特拉

            // 1492, // Urianger  :: 于里昂热
            // 4130, // Alphinaud :: 阿尔菲诺
            5239, // Alisaie     :: 阿莉塞
            8378, // Y'shtola    :: 雅·修特拉
            8889, // Ryne        :: 琳
            8917, // Minfilia    :: 敏菲利亚
            8919, // Lyna        :: 莱楠

            // 11264, // Alphinaud's avatar
            11265, // Alisaie's avatar

            // 11267, // Urianger's avatar
            11268, // Y'shtola's avatar
            11269, // Ryne's avatar
            11270, // Estinien's avatar

            // 10586, // Venat
            // 10587, // Venat's Phantom
            // 10898, // Emet-Selch
            // 10899, // Hythlodaeus
            // 9363,  G'raha Tia
            // 713,  // Thancred       :: 桑克瑞德
            // 8650, // Crystal Exarch :: 水晶公
            // 11266, // Thancred's avatar
            // 11271, // G'raha Tia's avatar
        };

        /// <summary>
        /// Gets NPC IDs of Tank party members.
        /// </summary>
        public static readonly HashSet<uint> PartyTankIds = new HashSet<uint>()
        {
            713,   // Thancred       :: 桑克瑞德
            8650,  // Crystal Exarch :: 水晶公
            11266, // Thancred's avatar
            11271, // G'raha Tia's avatar
            9363,  // G'raha Tia
        };

        /// <summary>
        /// Gets NPC IDs of Healer party members.
        /// </summary>
        public static readonly HashSet<uint> PartyHealerIds = new HashSet<uint>()
        {
        };
    }
}
