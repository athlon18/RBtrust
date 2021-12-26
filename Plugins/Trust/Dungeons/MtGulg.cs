using Buddy.Coroutines;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 79 Mt. Gulg dungeon logic.
    /// </summary>
    public class MtGulg : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Dungeons.ZoneId.MtGulg;

        /// <summary>
        /// Set of boss-related monster IDs.
        /// </summary>
        private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            4385, 7864, 8925, // Brightsphere          :: 光明晶球
            8260,             // Forgiven Cruelty      :: 得到宽恕的残忍
            8261,             // Forgiven Whimsy       :: 得到宽恕的无常
            8262,             // Forgiven Obscenity    :: 得到宽恕的猥亵
            8270,             // Forgiven Revelry      :: 得到宽恕的放纵
            8299,             // Forgiven Dissonance   :: 得到宽恕的失调
        };

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.MtGulg;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            // 15614, 15615, 15616, 15617, 15618, 17153     :: Typhoon Wing
            // 15622, 15623, 16987, 16988, 16989            :: Exegesis
            // 15638, 15640, 15641, 15649, 18025            :: Divine Diminuendo
            // 15642, 15643, 15648                          :: Conviction Marcato
            // 15644                                        :: Penance Pianissimo
            // 15645                                        :: Feather Marionette
            // 16247, 16248                                 :: Right Palm
            // 16249, 16250                                 :: Left Palm
            // 16521                                        :: Glittering Emerald
            // 16818                                        :: Lumen Infinitum
            HashSet<uint> spells = new HashSet<uint>()
            {
                15614, 15615, 15616, 15617, 15618, 15622, 15623, 15638,
                15640, 15641, 15642, 15643, 15644, 15645, 15648, 15649,
                16247, 16248, 16249, 16250, 16521, 16818, 16987, 16988,
                16989, 17153, 18025,
            };

            // Forgiven Obscenity (得到宽恕的猥亵)
            // 15652                                        :: Ringsmith
            // 15653                                        :: Gold Chaser
            // 17066                                        :: Solitaire Ring
            HashSet<uint> goldChaser = new HashSet<uint>() { 15652, 15653, 17066 };
            if (goldChaser.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 20000)
                {
                    await MovementHelpers.GetClosestAlly.Follow();
                    await Coroutine.Yield();
                }

                sw.Stop();
            }

            // Default (缺省)
            if (spells.IsCasting())
            {
                await MovementHelpers.GetClosestAlly.Follow();
            }

            // SideStep (回避)
            if (WorldManager.SubZoneId != 3000)
            {
                BossIds.ToggleSideStep(new uint[] { 8262 });
            }
            else
            {
                BossIds.ToggleSideStep();
            }

            return false;
        }
    }
}
