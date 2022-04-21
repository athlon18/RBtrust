using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.Pathing.Avoidance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 85 Vanaspati dungeon logic.
    /// </summary>
    public class Vanaspati : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.Vanaspati;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.Vanaspati;

        private HashSet<uint> follow = new HashSet<uint>()
        {
            25139 ,25140
        };

        private HashSet<uint> follow1 = new HashSet<uint>()
        {
            25151,
        };

        private HashSet<uint> follow2 = new HashSet<uint>()
        {
            25160, 25166
        };

        private HashSet<uint> magnet = new HashSet<uint>()
        {
            25143 ,25146 ,25148 ,27852
        };

        private HashSet<uint> magnet1 = new HashSet<uint>()
        {
            25145
        };

        private HashSet<uint> magnet2 = new HashSet<uint>()
        {
            25147
        };

        private HashSet<uint> magnet3 = new HashSet<uint>()
        {
           25142 , 25168
        };

        private HashSet<uint> magnet4 = new HashSet<uint>()
        {
            25169,
        };


        private static AvoidInfo AvoidNull = AvoidanceManager.AddAvoidLocation(() => false, 0, () => new Vector3("0,0,1"));


        private CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();
        private PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();
        private Stopwatch followSW = new Stopwatch();

        private Stopwatch follow1SW = new Stopwatch();

        private Stopwatch follow2SW = new Stopwatch();

        private Stopwatch followxSW = new Stopwatch();

        private Stopwatch magnetSW = new Stopwatch();

        private Stopwatch magnet1SW = new Stopwatch();

        private Stopwatch magnet2SW = new Stopwatch();

        private Stopwatch magnet3SW = new Stopwatch();

        private Stopwatch magnet3fW = new Stopwatch();

        private bool magnet3SWrun;

        private bool magnet3SWhaifrun;

        private Stopwatch magnet3xSW = new Stopwatch();

        private Stopwatch magnet3xsSW = new Stopwatch();

        private Stopwatch magnet4SW = new Stopwatch();

        private Stopwatch magnet4xSW = new Stopwatch();

        private Stopwatch magnetxSW = new Stopwatch();

        private Stopwatch hastargetSW = new Stopwatch();

        private Stopwatch hastargetxSW = new Stopwatch();

        private static DateTime resetTime = DateTime.Now;
        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {

            if (WorldManager.SubZoneId != 4012 && WorldManager.SubZoneId != 4013 && WorldManager.SubZoneId != 4014)
            {
                var target = GameObjectManager.Attackers?.OrderByDescending(e => e.CurrentHealthPercent).FirstOrDefault();

                if (target != null && Core.Player.CurrentTarget != target)
                {
                    if (DateTime.Now > resetTime)
                    {
                        resetTime = DateTime.Now.AddSeconds(3);

                        target.Target();
                    }

                    if (!sidestepPlugin.Enabled)
                    {
                        sidestepPlugin.Enabled = true;
                    }
                }

                if (magnet3SWhaifrun) magnet3SWhaifrun = false;

                magnet3.Remove(25160);
                magnet3.Remove(25169);
                hastargetxSW.Reset();
                hastargetSW.Reset();
                magnet3SW.Reset();
            }

            if (Core.Player.HasAura("Temporary Misdirection"))
            {
                sidestepPlugin.Enabled = true;
                CapabilityManager.Clear();
            }


            if (WorldManager.SubZoneId == 4014 && !Core.Player.InCombat)
            {
                if (magnet3SWhaifrun) magnet3SWhaifrun = false;
                magnet3.Remove(25160);
                magnet3.Remove(25169);
                hastargetxSW.Reset();
                hastargetSW.Reset();
                magnet3SW.Reset();
            }


            var hastarget = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).
                    Where(r => r.CanAttack && r.IsTargetable && r.IsValid && r.IsVisible).Any();

            if (WorldManager.SubZoneId == 4014 && Core.Player.InCombat)
            {
                if (!hastarget && !hastargetxSW.IsRunning)
                {
                    hastargetxSW.Restart();
                }
                if (hastarget)
                {
                    hastargetxSW.Restart();
                }

                if (!hastarget && hastargetxSW.ElapsedMilliseconds > 3000 || hastargetSW.IsRunning)
                {
                    if (!hastargetSW.IsRunning)
                    {
                        Logging.Write(Colors.Yellow, $@" 自动躲闪开启 hastarget {hastarget}");
                        sidestepPlugin.Enabled = false;
                        AvoidanceManager.RemoveAllAvoids(i => true);
                        AvoidanceManager.AddAvoid(AvoidNull);
                        AvoidanceManager.Pulse();
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 3000, "自动跟随");
                        hastargetSW.Restart();
                        followxSW.Reset();
                        follow2SW.Reset();
                    }

                    if (hastarget)
                    {
                        hastargetxSW.Reset();

                        Logging.Write(Colors.Yellow, $@" 自动躲闪开启 hastarget {hastarget}");
                        sidestepPlugin.Enabled = true;
                        CapabilityManager.Clear();
                        hastargetSW.Reset();
                        magnet3.Add(25169);
                        magnet3SWhaifrun = true;
                    }
                    else
                    {
                        await MovementHelpers.GetClosestAlly.Follow();
                    }
                }

            }

            if (follow.IsCasting() && !magnet3SW.IsRunning || followSW.IsRunning)
            {
                if (!followSW.IsRunning)
                {
                    Logging.Write(Colors.Yellow, $@" 自动跟随队友 followSW {follow.IsCasting()}");
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();

                    CapabilityManager.Clear();
                    followSW.Restart();

                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 15000, "自动跟随");
                }


                if (!follow.IsCasting())
                {

                    Logging.Write(Colors.Yellow, $@" 自动躲闪开启 followSW {followSW.ElapsedMilliseconds}");
                    sidestepPlugin.Enabled = true;
                    CapabilityManager.Clear();
                    followSW.Reset();

                }
                else
                {

                    await MovementHelpers.GetClosestAlly.Follow();
                }
            }

            if (follow1.IsCasting() || follow1SW.IsRunning)
            {
                if (!follow1SW.IsRunning)
                {
                    Logging.Write(Colors.Yellow, $@" 自动跟随队友 follow1SW {follow1.IsCasting()}");
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();

                    CapabilityManager.Clear();
                    follow1SW.Restart();

                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 15000, "自动跟随");
                }


                if (!follow1.IsCasting())
                {

                    Logging.Write(Colors.Yellow, $@" 自动躲闪开启 follow1SW {follow1SW.ElapsedMilliseconds}");
                    sidestepPlugin.Enabled = true;
                    CapabilityManager.Clear();
                    follow1SW.Reset();

                }
                else
                {
                    if (follow1SW.ElapsedMilliseconds > 2000)
                    {
                        await MovementHelpers.GetClosestLocal(new Vector3("-294.9383, 41.5, -354.0579")).Follow();
                    }
                }
            }


            if (followxSW.ElapsedMilliseconds > 3000)
            {
                Logging.Write(Colors.Yellow, $@" 自动躲闪开启 followxSW {followxSW.ElapsedMilliseconds}");
                sidestepPlugin.Enabled = true;
                CapabilityManager.Clear();
                followxSW.Reset();
            }



            if (!hastargetSW.IsRunning && !magnet3SW.IsRunning && !magnet3xsSW.IsRunning && (follow2.IsCasting() || follow2SW.IsRunning))
            {
                if (!follow2SW.IsRunning)
                {
                    Logging.Write(Colors.Yellow, $@" 自动跟随队友 follow2SW {follow2.IsCasting()}");
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();

                    CapabilityManager.Clear();
                    follow2SW.Restart();
                    followxSW.Reset();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 15000, "自动跟随");
                }


                if (!follow2.IsCasting())
                {
                    Logging.Write(Colors.Yellow, $@"  followxSW 开启");
                    if (!followxSW.IsRunning)
                    {
                        followxSW.Restart();
                        follow2SW.Reset();
                    }
                }
                else
                {

                    await MovementHelpers.GetClosestLocal(new Vector3("300.0752, 55.00583, -156.6629")).Follow();
                }


            }

            if (magnet.IsCasting() || magnetSW.IsRunning)
            {

                if (!magnetSW.IsRunning)
                {
                    if (WorldManager.SubZoneId == 4013)
                    {
                        sidestepPlugin.Enabled = false;
                        AvoidanceManager.RemoveAllAvoids(i => true);
                        AvoidanceManager.AddAvoid(AvoidNull);
                        AvoidanceManager.Pulse();

                        CapabilityManager.Clear();
                    }
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 5000, "正在移动");

                    magnetSW.Restart();
                }

                if (!magnet.IsCasting())
                {
                    CapabilityManager.Clear();

                    if (!sidestepPlugin.Enabled) sidestepPlugin.Enabled = true;

                    magnetSW.Reset();
                }
                else
                {
                    if (WorldManager.SubZoneId == 4012)
                    {
                        Vector3 location = new Vector3("-375.0211, 14.5, 84.97742");

                        if (Core.Me.Distance2D(location) > 1f)
                        {
                            Navigator.PlayerMover.MoveTowards(location);
                        }

                        else MovementManager.MoveStop();
                    }

                    if (WorldManager.SubZoneId == 4013)
                    {
                        Vector3 location = new Vector3("-294.9383, 41.5, -354.0579");

                        if (Core.Me.Distance2D(location) > 1f)
                        {
                            Navigator.PlayerMover.MoveTowards(location);
                        }

                        else MovementManager.MoveStop();
                    }
                }
            }

            if (magnet1.IsCasting() || magnet1SW.IsRunning)
            {

                if (!magnet1SW.IsRunning)
                {
                    if (WorldManager.SubZoneId == 4013)
                    {
                        sidestepPlugin.Enabled = false;
                        AvoidanceManager.RemoveAllAvoids(i => true);
                        AvoidanceManager.AddAvoid(AvoidNull);
                        AvoidanceManager.Pulse();

                        CapabilityManager.Clear();


                        magnetxSW.Restart();
                    }
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 3000, "正在移动");

                    magnet1SW.Restart();

                }

                await MovementHelpers.Spread(3000, 4, false, 1383);
            }

            if (magnet2.IsCasting() || magnet2SW.IsRunning)
            {

                if (!magnet2SW.IsRunning)
                {
                    if (WorldManager.SubZoneId == 4013)
                    {
                        sidestepPlugin.Enabled = false;
                        AvoidanceManager.RemoveAllAvoids(i => true);
                        AvoidanceManager.AddAvoid(AvoidNull);
                        AvoidanceManager.Pulse();

                        CapabilityManager.Clear();
                        magnet1SW.Reset();
                        magnetxSW.Reset();
                    }
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 5000, "正在移动");

                    magnet2SW.Restart();

                }

                if (!magnet2.IsCasting())
                {
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();

                    CapabilityManager.Clear();

                    if (!sidestepPlugin.Enabled) sidestepPlugin.Enabled = true;

                    magnet2SW.Reset();
                }
                else
                {
                    if (WorldManager.SubZoneId == 4013)
                    {
                        var usets = (Vector3)GameObjectManager.GetObjectsOfType<BattleCharacter>()?.Where(obj => obj.NpcId == 1383).OrderBy(obj => Core.Player.Distance2D(obj)).FirstOrDefault().Location;

                        if (Core.Me.Distance2D(usets) > 1f)
                        {
                            Navigator.PlayerMover.MoveTowards(usets);
                        }

                        else MovementManager.MoveStop();
                    }

                }

            }

            if (magnet3.IsCastingtwo() || magnet3SW.IsRunning)
            {
                if (!magnet3SW.IsRunning)
                {
                    followSW.Reset();
                    followxSW.Reset();
                    follow2SW.Reset();
                    Logging.Write(Colors.SkyBlue, $@"  magnet3 运行以开始 {magnet3.IsCastingtwo()}");
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();

                    CapabilityManager.Clear();
                    magnet3.Add(25160);


                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 15000, "正在移动");

                    magnet3SW.Restart();

                    ReceiveMessageHelpers.SkillsdeterminationOverStatus = false;
                }

                ReceiveMessageHelpers.SkillsdeterminationOverStr = "地脉失控";



                if (magnet3SWhaifrun && WorldManager.SubZoneId == 4014)
                {
                    if (magnet3SW.ElapsedMilliseconds < 7000 && !ReceiveMessageHelpers.SkillsdeterminationOverStatus)
                    {
                        await MovementHelpers.GetClosestLocal(new Vector3("300.0752, 55.00583, -156.6629")).Follow();
                    }
                    else
                    {

                        if (MovementHelpers.GetClosestAlly.Distance(new Vector3("300.0752, 55.00583, -156.6629")) - 2f < Core.Player.Distance(new Vector3("300.0752, 55.00583, -156.6629")))
                        {
                            Navigator.PlayerMover.MoveTowards(new Vector3("300.0752, 55.00583, -156.6629"));
                        }
                        else
                        {
                            if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                await MovementHelpers.SpreadSpLoc(3000, new Vector3("300.0752, 55.00583, -156.6629"), 6.5f, false);
                            }
                        }
                    }
                }
                else
                {
                    if (WorldManager.SubZoneId == 4014)
                    {
                        if (magnet3SW.ElapsedMilliseconds < 4000)
                        {
                            await MovementHelpers.GetClosestLocal(new Vector3("300.0752, 55.00583, -156.6629")).Follow();
                        }
                        else
                        {
                            if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                await MovementHelpers.SpreadSp(3000, new Vector3("300.0752, 55.00583, -156.6629"), 6.5f, false);
                            }
                        }
                    }

                    if (WorldManager.SubZoneId == 4012)
                    {
                        if (magnet3SW.ElapsedMilliseconds < 3000)
                        {
                            await MovementHelpers.GetClosestAlly.Follow();
                        }
                        else
                        {
                            if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                await MovementHelpers.Spread(3000, 6.5f, false);
                            }
                        }
                    }
                }



                if (!magnet3.IsCastingtwo())
                {
                    magnet3SW.Reset();
                    CapabilityManager.Clear();
                    AvoidanceManager.RemoveAllAvoids(i => true);
                    AvoidanceManager.AddAvoid(AvoidNull);
                    AvoidanceManager.Pulse();
                    sidestepPlugin.Enabled = true;
                    magnet3.Remove(25160);
                    Logging.Write(Colors.SkyBlue, $@"  magnet3SWrun 运行结束");
                    ReceiveMessageHelpers.SkillsdeterminationOverStatus = false;
                }
            }


            return false;
        }
    }
}
