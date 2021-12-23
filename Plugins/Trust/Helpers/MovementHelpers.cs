using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to movement.
    /// </summary>
    internal static class MovementHelpers
    {
        /// <summary>
        /// Gets the nearest Ally to the Player.
        /// </summary>
        public static BattleCharacter GetClosestAlly
        {
            get
            {
#if RB_CN
                HashSet<string> partyMemberNames = new HashSet<string>() { /*"桑克瑞德",*/ "雅·修特拉", "于里昂热", "阿尔菲诺", "阿莉塞", "雅·修特拉", /*"水晶公",*/ "琳", "敏菲利亚", "莱楠" };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberNames.Contains(obj.Name) && !obj.IsDead).OrderBy(r => r.Distance()).FirstOrDefault();
#else

                HashSet<uint> partyMemberIds = new HashSet<uint>()
                {
                    // 713,  // Thancred       :: 桑克瑞德
                    729,  // Y'shtola       :: 雅·修特拉
                    1492, // Urianger       :: 于里昂热
                    4130, // Alphinaud      :: 阿尔菲诺
                    5239, // Alisaie        :: 阿莉塞
                    8378, // Y'shtola       :: 雅·修特拉

                    // 8650, // Crystal Exarch :: 水晶公
                    8889, // Ryne           :: 琳
                    8917, // Minfilia       :: 敏菲利亚
                    8919, // Lyna           :: 莱楠
                };

                return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberIds.Contains(obj.NpcId) && !obj.IsDead).OrderBy(r => r.Distance()).FirstOrDefault();
#endif
            }
        }

        /// <summary>
        /// Gets the furthest Ally from the Player.
        /// </summary>
        public static BattleCharacter GetFurthestAlly
        {
            get
            {
#if RB_CN
                HashSet<string> partyMemberNames = new HashSet<string>() { /*"桑克瑞德",*/ "雅·修特拉", "于里昂热", "阿尔菲诺", "阿莉塞", "雅·修特拉", /*"水晶公",*/ "琳", "敏菲利亚", "莱楠" };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberNames.Contains(obj.Name) && !obj.IsDead).OrderBy(r => r.Distance()).Reverse().FirstOrDefault();
#else
                HashSet<uint> partyMemberIds = new HashSet<uint>()
                {
                    // 713,  // Thancred       :: 桑克瑞德
                    729,  // Y'shtola       :: 雅·修特拉
                    1492, // Urianger       :: 于里昂热
                    4130, // Alphinaud      :: 阿尔菲诺
                    5239, // Alisaie        :: 阿莉塞
                    8378, // Y'shtola       :: 雅·修特拉

                    // 8650, // Crystal Exarch :: 水晶公
                    8889, // Ryne           :: 琳
                    8917, // Minfilia       :: 敏菲利亚
                    8919, // Lyna           :: 莱楠
                };

                return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberIds.Contains(obj.NpcId) && !obj.IsDead).OrderBy(r => r.Distance()).Reverse().FirstOrDefault();
#endif
            }
        }
    }
}
