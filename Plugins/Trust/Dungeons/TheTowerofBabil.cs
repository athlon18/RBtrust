using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Navigation;
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
    /// Lv. 83 The Tower of Babil dungeon logic.
    /// </summary>
    public class TheTowerOfBabil : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheTowerOfBabil;

        // SPELLIDS

        // BARNABAS (B1)
        // Ground and Pound    25159
        // Ground and Pound    25322
        // Dynamic Pound       25326
        // Dynamic Pound       25157
        // Shocking Force (St) 25324
        // Dynamic Scrapline   25158
        // Dynamic Scrapline   25328
        // Thundercall         25325
        // Rolling Scrapline   25323

        // LUGAE (B2)
        // Thermal Suppression 25338
        // Magitek Missile     25334
        // Magitek Ray         25340
        // Magitek Chakram     25331
        // Magitek Explosive   25336
        // Downpour            25333

        // ANIMA (B3)
        // Lunar Nail         25342
        // Phantom Pain       21182
        // Mega Graviton      25344
        // Pater Patriae      25350
        // Boundless Pain     25347
        // Imperatum          25353
        // Obliviating Claw   25355
        // Obliviating Claw 2 25354
        // Erupting Pain      25351
        private readonly HashSet<uint> follow = new HashSet<uint>()
        {
            21182, 25324,
        };

        private readonly HashSet<uint> magnet = new HashSet<uint>()
        {
            25326, 25157, 25158, 25328,
        };

        private readonly HashSet<uint> toad = new HashSet<uint>()
        {
            25333,
        };

        private readonly HashSet<uint> mini = new HashSet<uint>()
        {
            25331,
        };

        private readonly HashSet<uint> boundlessPain = new HashSet<uint>()
        {
            25347,
        };

        private readonly HashSet<uint> spread = new HashSet<uint>()
        {
            25351,
        };

        private readonly HashSet<uint> claw2 = new HashSet<uint>()
        {
            25354,
        };

        private readonly Stopwatch followTimer = new Stopwatch();
        private readonly Stopwatch magnetTimer = new Stopwatch();
        private readonly Stopwatch miniTimer = new Stopwatch();
        private readonly Stopwatch toadTimer = new Stopwatch();
        private readonly Stopwatch boundlessPainTimer = new Stopwatch();
        private readonly Stopwatch claw2Timer = new Stopwatch();
        private readonly Stopwatch spreadTimer = new Stopwatch();

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheTowerOfBabil;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            if (follow.IsCasting() || followTimer.IsRunning)
            {
                if (!followTimer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    followTimer.Restart();
                }

                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();

                if (!follow.IsCasting())
                {
                    SidestepPlugin.Enabled = true;
                    followTimer.Reset();
                }
            }

            if (magnet.IsCasting() || magnetTimer.IsRunning)
            {
                if (!magnetTimer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 12000, "Magnet Spell In Progress");
                    magnetTimer.Restart();
                }

                if (magnetTimer.ElapsedMilliseconds < 12000)
                {
                    Vector3 location = new Vector3("-314.4527, -175, 70.98297");

                    if (Core.Me.Distance(location) > 1f)
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                    else
                    {
                        MovementManager.MoveStop();
                    }
                }

                if (magnetTimer.ElapsedMilliseconds >= 12000)
                {
                    SidestepPlugin.Enabled = true;
                    magnetTimer.Reset();
                }
            }

            if (toad.IsCasting() || toadTimer.IsRunning)
            {
                if (!toadTimer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 30000, "Shapeshift Mechanic In Progress");
                    toadTimer.Restart();
                }

                if (toadTimer.ElapsedMilliseconds < 12000)
                {
                    Vector3 location = new Vector3("214.2467, 0.9999993, 306.0189");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (toadTimer.ElapsedMilliseconds >= 12000 && toadTimer.ElapsedMilliseconds < 30000)
                {
                    await MovementHelpers.GetClosestAlly.Follow();
                }

                if (toadTimer.ElapsedMilliseconds >= 30000)
                {
                    SidestepPlugin.Enabled = true;
                    toadTimer.Reset();
                }
            }

            if (mini.IsCasting() || miniTimer.IsRunning)
            {
                if (!miniTimer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 24000, "Shapeshift Mechanic In Progress");
                    miniTimer.Restart();
                }

                if (miniTimer.ElapsedMilliseconds < 12000)
                {
                    Vector3 location = new Vector3("227.0484, 1.00001, 305.9774");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (miniTimer.ElapsedMilliseconds >= 12000 && miniTimer.ElapsedMilliseconds < 24000)
                {
                    Vector3 location = new Vector3("220.9772, 1, 305.9483");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (miniTimer.ElapsedMilliseconds >= 24000)
                {
                    SidestepPlugin.Enabled = true;
                    miniTimer.Reset();
                }
            }

            if (claw2.IsCasting() || claw2Timer.IsRunning)
            {
                if (!claw2Timer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 12000, "Obliviating Claw 2 In Progress");
                    claw2Timer.Restart();
                }

                if (claw2Timer.ElapsedMilliseconds < 6000)
                {
                    Vector3 location = new Vector3("16.74083, 120, -406.9069");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (claw2Timer.ElapsedMilliseconds >= 6000 && claw2Timer.ElapsedMilliseconds < 12000)
                {
                    Vector3 location = new Vector3("-15.15774, 120, -408.2812");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (claw2Timer.ElapsedMilliseconds >= 12000)
                {
                    SidestepPlugin.Enabled = true;
                    claw2Timer.Reset();
                }
            }

            if (boundlessPain.IsCasting() || boundlessPainTimer.IsRunning)
            {
                if (!boundlessPainTimer.IsRunning)
                {
                    SidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 18000, "Boundless Pain Avoid");
                    boundlessPainTimer.Restart();
                }

                if (boundlessPainTimer.ElapsedMilliseconds >= 8000 && boundlessPainTimer.ElapsedMilliseconds < 18000)
                {
                    Vector3 location = new Vector3("11.11008, 479.9997, -199.1336");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        Navigator.Stop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }

                if (boundlessPainTimer.ElapsedMilliseconds >= 18000)
                {
                    SidestepPlugin.Enabled = true;
                    boundlessPainTimer.Reset();
                }
            }

            if (spread.IsCasting() || spreadTimer.IsRunning)
            {
                if (!spreadTimer.IsRunning)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 5000, "Spread");
                    spreadTimer.Start();
                }

                if (spreadTimer.ElapsedMilliseconds < 5000)
                {
                    await MovementHelpers.Spread(5000);
                }

                if (spreadTimer.ElapsedMilliseconds >= 5000)
                {
                    spreadTimer.Reset();
                    AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
                }
            }

            // Avoid Claw Game in final zone only if in combat, otherwise frequent stucks
            if (WorldManager.SubZoneId == 4133)
            {
                SidestepPlugin.Enabled = Core.Me.InCombat;
            }

            return false;
        }
    }
}
