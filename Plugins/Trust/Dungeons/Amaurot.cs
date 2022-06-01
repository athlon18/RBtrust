using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using RBTrust.Plugins.Trust.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 80.1 Amaurot dungeon logic.
    /// </summary>
    public class Amaurot : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.Amaurot;

        /// <summary>
        /// Set of boss-related monster IDs.
        /// </summary>
        private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            4385, 7864, 8925, // Brightsphere          :: 光明晶球
            8201,             // The First Beast       :: 第一之兽
            8210,             // Therion               :: 至大灾兽
        };

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.Amaurot;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            // NOT TESTED

            // 532, 1837, 2794, 5445, 7931, 9076, 9338, 9490,
            // 9493, 10256, 10257, 11573, 11582, 12377, 12486,
            // 12589, 12590, 12591, 12648, 12654, 12681, 12688,
            // 12805, 12809, 12823, 12824, 12825, 13251, 13336,
            // 13337, 13344, 13345, 13346, 15565, 18157                    :: Earthquake
            // 15559, 15560                                                :: The Burning Sky
            // 15561, 15562                                                :: The Falling Sky
            // 15566                                                       :: Venomous Breath
            // 15579, 15580, 15581, 15582, 15583, 15585, 16785, 16786      :: Deathly Ray
            // 15586                                                       :: Misfortune
            // 17996                                                       :: Starstorm
            HashSet<uint> spells = new HashSet<uint>()
            {
                532, 1837, 2794, 5445, 7931, 9076, 9338, 9490, 9493, 10256, 10257, 11573,
                11582, 12377, 12486, 12589, 12590, 12591, 12648, 12654, 12681, 12688, 12805,
                12809, 12823, 12824, 12825, 13251, 13336, 13337, 13344, 13345, 13346, 15561,
                15559, 15560, 15562, 15565, 15566, 15579, 15580, 15581, 15582, 15583, 15585,
                15586, 16785, 16786, 18157, 17996,
            };

            // The First Beast (第一之兽)
            // 15556, 15558                                 :: Meteor Rain
            HashSet<uint> meteorRain = new HashSet<uint>() { 15556, 15558 };
            if (meteorRain.IsCasting())
            {
                Vector3 location = new Vector3(-99.49644f, 748.2327f, 101.4963f);

                while (Core.Me.Distance(location) > 1f)
                {
                    await CommonTasks.MoveTo(location);
                    await Coroutine.Yield();
                }

                await Coroutine.Sleep(3000);

                if (ActionManager.IsSprintReady)
                {
                    ActionManager.Sprint();
                    await Coroutine.Wait(1000, () => !ActionManager.IsSprintReady);
                }

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 5000)
                {
                    await MovementHelpers.GetClosestAlly.Follow();
                    await Coroutine.Yield();
                }

                sw.Stop();
            }

            // Therion (至大灾兽)
            // 15575, 15577                                 :: Apokalypsis
            HashSet<uint> apokalypsis = new HashSet<uint>() { 15575, 15577 };
            if (apokalypsis.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 10000)
                {
                    await MovementHelpers.GetClosestAlly.Follow(useMesh: true);
                    await Coroutine.Yield();
                }

                sw.Stop();
            }

            // Therion (至大灾兽)
            // 15578                                        :: Therion Charge
            HashSet<uint> therionCharge = new HashSet<uint>() { 15578 };
            if (therionCharge.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 8000)
                {
                    await MovementHelpers.GetClosestAlly.Follow(useMesh: true);
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
            if (WorldManager.SubZoneId != 2996)
            {
                BossIds.ToggleSideStep(new uint[] { 8210 });
            }
            else
            {
                BossIds.ToggleSideStep();
            }

            return false;
        }
    }
}
