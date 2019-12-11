using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Trust
{
    public class Amaurot
    {
        public static async Task<bool> Run()
        {
            // NOT TESTED

            #region Spell Filters
            /// 532, 1837, 2794, 5445, 7931, 9076, 9338, 9490,
            /// 9493, 10256, 10257, 11573, 11582, 12377, 12486,
            /// 12589, 12590, 12591, 12648, 12654, 12681, 12688,
            /// 12805, 12809, 12823, 12824, 12825, 13251, 13336,
            /// 13337, 13344, 13345, 13346, 15565, 18157                    :: Earthquake
            /// 15559, 15560                                                :: The Burning Sky
            /// 15561, 15562                                                :: The Falling Sky
            /// 15566                                                       :: Venomous Breath
            /// 15579, 15580, 15581, 15582, 15583, 15585, 16785, 16786      :: Deathly Ray
            /// 15586                                                       :: Misfortune
            /// 17996                                                       :: Starstorm
            HashSet<uint> Spells = new HashSet<uint>()
            {
                532, 1837, 2794, 5445, 7931, 9076, 9338, 9490, 9493, 10256, 10257, 11573,
                11582, 12377, 12486, 12589, 12590, 12591, 12648, 12654, 12681, 12688, 12805,
                12809, 12823, 12824, 12825, 13251, 13336, 13337, 13344, 13345, 13346, 15561,
                15559, 15560, 15562, 15565, 15566, 15579, 15580, 15581, 15582, 15583, 15585,
                15586, 16785, 16786, 18157, 17996
            };
            #endregion

            #region Custom Mechanics
            /// The First Beast (第一之兽)
            /// 15556, 15558                                 :: Meteor Rain
            HashSet<uint> MeteorRain = new HashSet<uint>() { 15556, 15558 };
            if (MeteorRain.IsCasting())
            {
                Vector3 _loc = new Vector3(-99.49644f, 748.2327f, 101.4963f);

                while (Core.Me.Distance(_loc) > 1f)
                {
                    await CommonTasks.MoveTo(_loc);
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
                    await Helpers.GetClosestAlly.Follow();
                    await Coroutine.Yield();
                }
                sw.Stop();
            }

            /// Therion (至大灾兽)
            /// 15575, 15577                                 :: Apokalypsis
            HashSet<uint> Apokalypsis = new HashSet<uint>() { 15575, 15577 };
            if (Apokalypsis.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 10000)
                {
                    await Helpers.GetClosestAlly.Follow(useMesh: true);
                    await Coroutine.Yield();
                }
                sw.Stop();
            }

            /// Therion (至大灾兽)
            /// 15578                                        :: Therion Charge
            HashSet<uint> TherionCharge = new HashSet<uint>() { 15578 };
            if (TherionCharge.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 8000)
                {
                    await Helpers.GetClosestAlly.Follow(useMesh: true);
                    await Coroutine.Yield();
                }
                sw.Stop();
            }
            #endregion

            /// Default (缺省)
            if (Spells.IsCasting()) { await Helpers.GetClosestAlly.Follow(); }

            /// SideStep (回避)
            if (WorldManager.SubZoneId != 2996) { Helpers.BossIds.ToggleSideStep(new uint[] { 8210 }); } else { Helpers.BossIds.ToggleSideStep(); }

            return false;
        }
    }
}
