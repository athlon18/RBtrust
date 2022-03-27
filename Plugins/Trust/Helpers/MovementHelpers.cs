using Buddy.Coroutines;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Trust.Helpers;


namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to movement.
    /// </summary>
    internal static class MovementHelpers
    {
        public static readonly HashSet<uint> PartyMemberIds = new HashSet<uint>()
        {
            729,  // Y'shtola       :: 雅·修特拉
            1492, // Urianger       :: 于里昂热
            4130, // Alphinaud      :: 阿尔菲诺
            5239, // Alisaie        :: 阿莉塞
            8378, // Y'shtola       :: 雅·修特拉
            8889, // Ryne           :: 琳
            8917, // Minfilia       :: 敏菲利亚
            8919, // Lyna           :: 莱楠
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

         public static readonly HashSet<uint> AllPartyDpsIds = new HashSet<uint>()
        {
            729,  // Y'shtola       :: 雅·修特拉
            //1492, // Urianger       :: 于里昂热
            //4130, // Alphinaud      :: 阿尔菲诺
            5239, // Alisaie        :: 阿莉塞
            8378, // Y'shtola       :: 雅·修特拉
            8889, // Ryne           :: 琳
            8917, // Minfilia       :: 敏菲利亚
            8919, // Lyna           :: 莱楠
            //11264, // Alphinaud's avatar
            11265, // Alisaie's avatar
            //11267, // Urianger's avatar
            11268, // Y'shtola's avatar
            11269, // Ryne's avatar
            11270, // Estinien's avatar
            //10586, // Venat
            //10587, // Venat's Phantom
            //10898, // Emet-Selch
            //10899, // Hythlodaeus

            //713,  // Thancred       :: 桑克瑞德
            //8650, // Crystal Exarch :: 水晶公
            //11266, // Thancred's avatar
            //11271, // G'raha Tia's avatar
        };

        public static readonly HashSet<uint> AllPartyTankIds = new HashSet<uint>()
        {
            713,  // Thancred       :: 桑克瑞德
            8650, // Crystal Exarch :: 水晶公
            11266, // Thancred's avatar
            11271, // G'raha Tia's avatar
        };

        public static readonly HashSet<uint> AllPartyMemberIds = new HashSet<uint>()
        {
            729,  // Y'shtola       :: 雅·修特拉
            1492, // Urianger       :: 于里昂热
            4130, // Alphinaud      :: 阿尔菲诺
            5239, // Alisaie        :: 阿莉塞
            8378, // Y'shtola       :: 雅·修特拉
            8889, // Ryne           :: 琳
            8917, // Minfilia       :: 敏菲利亚
            8919, // Lyna           :: 莱楠
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

            713,  // Thancred       :: 桑克瑞德
            8650, // Crystal Exarch :: 水晶公
            11266, // Thancred's avatar
            11271, // G'raha Tia's avatar
        };

        /// <summary>
        /// Gets the nearest Ally to the Player.
        /// </summary>
        public static BattleCharacter GetClosestAlly => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMemberIds.Contains(obj.NpcId))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        public static BattleCharacter GetClosestDps => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && AllPartyDpsIds.Contains(obj.NpcId))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        public static BattleCharacter GetClosestTank => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && AllPartyTankIds.Contains(obj.NpcId))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the furthest Ally from the Player.
        /// </summary>
        public static BattleCharacter GetFurthestAlly => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMemberIds.Contains(obj.NpcId))
            .OrderByDescending(r => r.Distance())
            .FirstOrDefault();     

        public static async Task<bool> Spread(double TimeToSpread, float spreadDistance = 6.5f, bool IsSpreading = false)
        {
            if (IsSpreading)
            {
                return true;
            }

            double CurrentMS = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double EndMS = CurrentMS + (TimeToSpread);

            //if (sidestepPlugin != null)
            //    { 
            //        sidestepPlugin.Enabled = true;
            //    }
                        
            foreach (var npc in GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                .Where(obj => AllPartyMemberIds.Contains(obj.NpcId)))
                {
                    AvoidanceManager.AddAvoidObject<BattleCharacter>(
                        () => DateTime.Now.TimeOfDay.TotalMilliseconds <= EndMS,
                        radius: spreadDistance,
                        npc.ObjectId);
                }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                MovementManager.MoveStop();
                            }

            return true;
        }


    }

   
}
