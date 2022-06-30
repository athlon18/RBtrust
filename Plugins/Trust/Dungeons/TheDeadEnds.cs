﻿using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using RBTrust.Plugins.Trust.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 90.1 The Dead Ends dungeon logic.
    /// </summary>
    public class TheDeadEnds : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheDeadEnds;

        private const int CausticGrebuloff = 10313;
        private const int Peacekeeper = 10315;
        private const int Rala = 10316;

        private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            CausticGrebuloff, Peacekeeper, Rala,
        };

        /// <summary>
        /// Collection of Pestilent Sands environmental traps.
        /// </summary>
        private readonly List<(float Radius, Vector3 Location)> pestilentSandsTraps = new List<(float Radius, Vector3 Location)>()
        {
            (10f, new Vector3(384.6537f, 499.6f, 134.6268f)),
            (6f, new Vector3(393.4057f, 500.5982f, 108.7927f)),
            (6f, new Vector3(383.599f, 500.4645f, 84.56992f)),
            (6f, new Vector3(349.5155f, 499.6613f, 55.23549f)),
            (5f, new Vector3(428.6128f, 499.2374f, 102.9493f)),
            (5f, new Vector3(340.2103f, 499.5605f, 65.34068f)),
            (4f, new Vector3(385.912f, 499.8732f, 60.55911f)),
            (4f, new Vector3(394.0793f, 499.9395f, 69.48797f)),
            (4f, new Vector3(400.2813f, 499.5386f, 78.03534f)),
            (4f, new Vector3(403.9248f, 500.1888f, 91.52982f)),
            (4f, new Vector3(411.6979f, 500.2933f, 122.0323f)),
            (4f, new Vector3(414.9597f, 499.6505f, 89.00001f)),
            (4f, new Vector3(409.712f, 500.2364f, 129.654f)),
            (4f, new Vector3(419.4437f, 499.6318f, 131.0557f)),
            (4f, new Vector3(391.057f, 499.2928f, 163.2473f)),
            (4f, new Vector3(383.4154f, 499.6458f, 160.7675f)),
            (4f, new Vector3(373.9166f, 500.1277f, 152.5391f)),
            (4f, new Vector3(377.9803f, 500.3969f, 74.72343f)),
            (3f, new Vector3(424.1697f, 499.1639f, 90.62242f)),
            (3f, new Vector3(419.0435f, 500.0604f, 97.24624f)),
            (3f, new Vector3(407.203f, 499.472f, 136.9477f)),
            (3f, new Vector3(345.3004f, 499.6697f, 175.295f)),
            (3f, new Vector3(372.0362f, 499.2906f, 169.7142f)),
            (3f, new Vector3(364.4807f, 499.2977f, 186.0001f)),
            (2f, new Vector3(349.9047f, 499.3683f, 188.2401f)),

            // 2nd area
            (5f, new Vector3(276.1789f, 500.5079f, -100.7027f)),
            (5f, new Vector3(292.7461f, 500.5103f, -111.7281f)),
            (5f, new Vector3(299.21f, 500.5f, -81.65562f)),
            (5f, new Vector3(314.4836f, 502.5079f, -41.21803f)),
            (5f, new Vector3(296.3872f, 502.5f, -25.8502f)),
            (5f, new Vector3(351.1416f, 500.6152f, 22.12455f)),
            (5f, new Vector3(351.6766f, 500.5081f, 5.411625f)),
        };

        private readonly List<(float Radius, Vector3 Location)> peacekeeperRing = new List<(float Radius, Vector3 Location)>()
        {
            (5f, new Vector3(-93.2f, 0.1995f, -193.8f)),
            (5f, new Vector3(-94.4f, 0.1995f, -193.0f)),
            (5f, new Vector3(-95.6f, 0.1995f, -192.3f)),
            (5f, new Vector3(-96.9f, 0.1995f, -191.7f)),
            (5f, new Vector3(-98.2f, 0.1995f, -191.2f)),
            (5f, new Vector3(-99.5f, 0.1995f, -190.8f)),
            (5f, new Vector3(-100.8f, 0.1995f, -190.4f)),
            (5f, new Vector3(-102.2f, 0.1995f, -190.2f)),
            (5f, new Vector3(-103.6f, 0.1995f, -190.0f)),
            (5f, new Vector3(-85.3f, 0.1995f, -213.5f)),
            (5f, new Vector3(-85.1f, 0.1995f, -212.1f)),
            (5f, new Vector3(-85.0f, 0.1995f, -210.7f)),
            (5f, new Vector3(-85.1f, 0.1995f, -207.9f)),
            (5f, new Vector3(-85.3f, 0.1995f, -206.5f)),
            (5f, new Vector3(-85.6f, 0.1995f, -205.2f)),
            (5f, new Vector3(-86.0f, 0.1995f, -203.8f)),
            (5f, new Vector3(-86.5f, 0.1995f, -202.5f)),
            (5f, new Vector3(-87.0f, 0.1995f, -201.2f)),
            (5f, new Vector3(-87.7f, 0.1995f, -200.0f)),
            (5f, new Vector3(-88.4f, 0.1995f, -198.8f)),
            (5f, new Vector3(-89.2f, 0.1995f, -197.7f)),
            (5f, new Vector3(-90.1f, 0.1995f, -196.6f)),
            (5f, new Vector3(-91.1f, 0.1995f, -195.6f)),
            (5f, new Vector3(-92.1f, 0.1995f, -194.7f)),
            (5f, new Vector3(-106.4f, 0.1995f, -230.0f)),
            (5f, new Vector3(-105.0f, 0.1995f, -230.0f)),
            (5f, new Vector3(-103.6f, 0.1995f, -230.0f)),
            (5f, new Vector3(-102.2f, 0.1995f, -229.8f)),
            (5f, new Vector3(-100.8f, 0.1995f, -229.6f)),
            (5f, new Vector3(-99.5f, 0.1995f, -229.2f)),
            (5f, new Vector3(-98.2f, 0.1995f, -228.8f)),
            (5f, new Vector3(-96.9f, 0.1995f, -228.3f)),
            (5f, new Vector3(-95.6f, 0.1995f, -227.7f)),
            (5f, new Vector3(-94.4f, 0.1995f, -227.0f)),
            (5f, new Vector3(-93.2f, 0.1995f, -226.2f)),
            (5f, new Vector3(-92.1f, 0.1995f, -225.3f)),
            (5f, new Vector3(-91.1f, 0.1995f, -224.4f)),
            (5f, new Vector3(-90.1f, 0.1995f, -223.4f)),
            (5f, new Vector3(-89.2f, 0.1995f, -222.3f)),
            (5f, new Vector3(-88.4f, 0.1995f, -221.2f)),
            (5f, new Vector3(-87.7f, 0.1995f, -220.0f)),
            (5f, new Vector3(-87.0f, 0.1995f, -218.8f)),
            (5f, new Vector3(-86.5f, 0.1995f, -217.5f)),
            (5f, new Vector3(-86.0f, 0.1995f, -216.2f)),
            (5f, new Vector3(-85.6f, 0.1995f, -214.8f)),
            (5f, new Vector3(-124.4f, 0.1995f, -214.8f)),
            (5f, new Vector3(-124.0f, 0.1995f, -216.2f)),
            (5f, new Vector3(-123.5f, 0.1995f, -217.5f)),
            (5f, new Vector3(-123.0f, 0.1995f, -218.8f)),
            (5f, new Vector3(-122.3f, 0.1995f, -220.0f)),
            (5f, new Vector3(-121.6f, 0.1995f, -221.2f)),
            (5f, new Vector3(-120.8f, 0.1995f, -222.3f)),
            (5f, new Vector3(-119.9f, 0.1995f, -223.4f)),
            (5f, new Vector3(-118.9f, 0.1995f, -224.4f)),
            (5f, new Vector3(-117.9f, 0.1995f, -225.3f)),
            (5f, new Vector3(-116.8f, 0.1995f, -226.2f)),
            (5f, new Vector3(-115.6f, 0.1995f, -227.0f)),
            (5f, new Vector3(-114.4f, 0.1995f, -227.7f)),
            (5f, new Vector3(-113.1f, 0.1995f, -228.3f)),
            (5f, new Vector3(-111.8f, 0.1995f, -228.8f)),
            (5f, new Vector3(-110.5f, 0.1995f, -229.2f)),
            (5f, new Vector3(-109.2f, 0.1995f, -229.6f)),
            (5f, new Vector3(-107.8f, 0.1995f, -229.8f)),
            (5f, new Vector3(-105.0f, 0.1995f, -190.0f)),
            (5f, new Vector3(-106.4f, 0.1995f, -190.0f)),
            (5f, new Vector3(-107.8f, 0.1995f, -190.2f)),
            (5f, new Vector3(-109.2f, 0.1995f, -190.4f)),
            (5f, new Vector3(-110.5f, 0.1995f, -190.8f)),
            (5f, new Vector3(-111.8f, 0.1995f, -191.2f)),
            (5f, new Vector3(-113.1f, 0.1995f, -191.7f)),
            (5f, new Vector3(-114.4f, 0.1995f, -192.3f)),
            (5f, new Vector3(-115.6f, 0.1995f, -193.0f)),
            (5f, new Vector3(-116.8f, 0.1995f, -193.8f)),
            (5f, new Vector3(-117.9f, 0.1995f, -194.7f)),
            (5f, new Vector3(-118.9f, 0.1995f, -195.6f)),
            (5f, new Vector3(-119.9f, 0.1995f, -196.6f)),
            (5f, new Vector3(-120.8f, 0.1995f, -197.7f)),
            (5f, new Vector3(-121.6f, 0.1995f, -198.8f)),
            (5f, new Vector3(-122.3f, 0.1995f, -200.0f)),
            (5f, new Vector3(-123.0f, 0.1995f, -201.2f)),
            (5f, new Vector3(-123.5f, 0.1995f, -202.5f)),
            (5f, new Vector3(-124.0f, 0.1995f, -203.8f)),
            (5f, new Vector3(-124.4f, 0.1995f, -205.2f)),
            (5f, new Vector3(-124.7f, 0.1995f, -206.5f)),
            (5f, new Vector3(-124.9f, 0.1995f, -207.9f)),
            (5f, new Vector3(-125.0f, 0.1995f, -209.3f)),
            (5f, new Vector3(-125.0f, 0.1995f, -210.7f)),
            (5f, new Vector3(-124.9f, 0.1995f, -212.1f)),
            (5f, new Vector3(-124.7f, 0.1995f, -213.5f)),
        };

        // Misc Spells
        //  Peacekeeper     25936   Decimation (AoE)
        //                  28360   Electromagnetic Repellant (danger zones on boss and edges)
        //                  28351   Order to Fire (Lasers AoE)
        //                  25925   No Future (targeting circles AoE)
        //                  25933   Peacefire (Clockwise AoE)
        //                  25931   Eclipsing Exhaust
        //                  25935   Elimination (Tankbuster)
        //                  25940,    Ra-La              Lifesbreath
        //                  25945, //  Ra-La              Benevolence
        //                  25943, //  Ra-La               Loving Embrace
        //                  25944, //  Ra-La               Loving Embrace

        // GENERIC MECHANICS
        private readonly HashSet<uint> spread = new HashSet<uint>()
        {
            25923, // Caustic Grebuloff   Befoulment (Spread Mechanic)
            25947, // Ra-La               Still Embrace
        };

        private readonly HashSet<uint> stack = new HashSet<uint>()
        {
            25921, // Caustic Grebuloff   Blighted Water

            // 28360,
            25931,

            // 25940,  // Ra-La - Lifesbreath
            25945,  // Ra-La - Benevolence
            25943,  // Ra-La - Loving Embrace
            25944,  // Ra-La - Loving Embrace
            27717,  // Xenoflora - Creeping Hush
        };

        private readonly HashSet<uint> tankBusters = new HashSet<uint>()
        {
            25935, // Peacekeeper - Elimination (Tankbuster)
        };

        // CAUSTIC GREBULOFF SPELLS (B1)
        private readonly HashSet<uint> miasmata = new HashSet<uint>()
        {
            25916, // Caustic Grebuloff - Miasmata
        };

        private readonly HashSet<uint> nausea = new HashSet<uint>()
        {
            28347, // Caustic Grebuloff - Wave of Nausea
        };

        private readonly HashSet<uint> coughUp = new HashSet<uint>()
        {
            25917, // Cough Up
        };

        // PEACEKEEPER SPELLS (B2)
        private readonly HashSet<uint> peacefire = new HashSet<uint>()
        {
            25933,
        };

        private readonly HashSet<uint> noFuture = new HashSet<uint>()
        {
            25925,
        };

        private readonly HashSet<uint> electrorep = new HashSet<uint>()
        {
            28360,
        };

        private readonly HashSet<uint> orderToFire = new HashSet<uint>()
        {
            28351,
        };

        // RA-LA SPELLS (B3)
        private readonly HashSet<uint> lifesbreath = new HashSet<uint>()
        {
            25940, // Ra-La Lifesbreath
        };

        private readonly HashSet<uint> prance = new HashSet<uint>()
        {
            25937, // Ra-La Prance
        };

        private readonly Stopwatch noFutureTimer = new Stopwatch();
        private readonly Stopwatch erTimer = new Stopwatch();
        private readonly Stopwatch miasmataTimer = new Stopwatch();
        private readonly Stopwatch peacefireTimer = new Stopwatch();
        private readonly Stopwatch coughUpTimer = new Stopwatch();
        private readonly Stopwatch orderToFireTimer = new Stopwatch();
        private readonly Stopwatch spreadTimer = new Stopwatch();
        private readonly Stopwatch lifesbreathTimer = new Stopwatch();
        private readonly Stopwatch pranceTimer = new Stopwatch();

        /// <summary>
        /// Tracks sub-zone since last tick for environmental decision making.
        /// </summary>
        private SubZoneId lastSubZoneId = SubZoneId.NONE;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheDeadEnds;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            if (WorldManager.SubZoneId == (uint)SubZoneId.JudgmentDay)
            {
                SidestepPlugin.Enabled = Core.Player.InCombat;
            }

            if (!Core.Me.InCombat)
            {
                CapabilityManager.Clear();
                spreadTimer.Reset();
                noFutureTimer.Reset();
                erTimer.Reset();
                miasmataTimer.Reset();
                peacefireTimer.Reset();
                coughUpTimer.Reset();
                lifesbreathTimer.Reset();
                pranceTimer.Reset();
            }

            // GENERIC MECHANICS
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
                }
            }

            if (stack.IsCasting())
            {
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            if (nausea.IsCasting() && !miasmataTimer.IsRunning)
            {
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Wave of Nausea In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            if (tankBusters.IsCasting())
            {
                if (Core.Player.IsTank())
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Tankbuster Spread In Progress");
                    Vector3 location = new Vector3("-105.0078, 0.1995358, -218.126");

                    if (Core.Me.Distance(location) < 1f)
                    {
                        MovementManager.MoveStop();
                    }
                    else
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }
                }
                else
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Tankbuster Spread In Progress");
                    await MovementHelpers.GetClosestAlly.Follow();
                }
            }

            // CAUSTIC GREBULOFF (B1)
            if (miasmata.IsCasting() || miasmataTimer.IsRunning)
            {
                if (!miasmataTimer.IsRunning)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 19000, "Miasmata Avoid");
                    miasmataTimer.Restart();
                }

                if (miasmataTimer.ElapsedMilliseconds < 19000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(miasmataTimer, 19000);
                }

                if (miasmataTimer.ElapsedMilliseconds >= 19000)
                {
                    miasmataTimer.Reset();
                }
            }

            if (coughUp.IsCasting() || coughUpTimer.IsRunning)
            {
                if (!coughUpTimer.IsRunning)
                {
                    coughUpTimer.Start();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 15000, "Cough Up Avoid");
                }

                if (coughUpTimer.ElapsedMilliseconds < 12000)
                {
                    if (!AvoidanceManager.IsRunningOutOfAvoid)
                    {
                        MovementManager.MoveStop();
                    }

                    SidestepPlugin.Enabled = true;
                }

                if (coughUpTimer.ElapsedMilliseconds >= 12000 && coughUpTimer.ElapsedMilliseconds < 15000)
                {
                    SidestepPlugin.Enabled = false;
                    await MovementHelpers.GetClosestAlly.Follow2(coughUpTimer, 15000);
                }

                if (coughUpTimer.ElapsedMilliseconds > 15000)
                {
                    coughUpTimer.Reset();
                }
            }

            // PEACEKEEPER (B2)
            if (electrorep.IsCasting())
            {
                SidestepPlugin.Enabled = false;
                if (!erTimer.IsRunning || erTimer.ElapsedMilliseconds > 25000)
                {
                    erTimer.Restart();
                }
            }

            if ((orderToFire.IsCasting() || orderToFireTimer.IsRunning) && !peacefireTimer.IsRunning)
            {
                if (!orderToFireTimer.IsRunning)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 8000, "Order To Fire Avoid");
                    orderToFireTimer.Restart();
                }

                if (orderToFireTimer.ElapsedMilliseconds < 4500)
                {
                    await MovementHelpers.GetClosestDps.Follow2(orderToFireTimer, 4500);
                }

                if (orderToFireTimer.ElapsedMilliseconds > 4500 && orderToFireTimer.ElapsedMilliseconds < 8000)
                {
                    if (!AvoidanceManager.IsRunningOutOfAvoid)
                    {
                        MovementManager.MoveStop();
                    }

                    await MovementHelpers.Spread(3500);
                }

                if (orderToFireTimer.ElapsedMilliseconds >= 8000)
                {
                    orderToFireTimer.Reset();
                }
            }

            if (noFuture.IsCasting() || noFutureTimer.IsRunning)
            {
                if (!noFutureTimer.IsRunning)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 19000, "No Future 1 Avoid");
                    noFutureTimer.Start();
                }

                if (noFutureTimer.ElapsedMilliseconds < 16000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(noFutureTimer, 16000);
                }

                if (noFutureTimer.ElapsedMilliseconds >= 16000 && noFutureTimer.ElapsedMilliseconds < 19000)
                {
                    await MovementHelpers.Spread(3000);

                    if (!AvoidanceManager.IsRunningOutOfAvoid)
                    {
                        MovementManager.MoveStop();
                    }
                }

                if (noFutureTimer.ElapsedMilliseconds >= 84000 && noFutureTimer.ElapsedMilliseconds < 85000)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 21000, "No Future 2 Avoid");
                }

                if (noFutureTimer.ElapsedMilliseconds >= 84000 && noFutureTimer.ElapsedMilliseconds < 100500)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(noFutureTimer, 100500);
                }

                if (noFutureTimer.ElapsedMilliseconds >= 100500 && noFutureTimer.ElapsedMilliseconds < 103500)
                {
                    if (!AvoidanceManager.IsRunningOutOfAvoid)
                    {
                        MovementManager.MoveStop();
                    }

                    SidestepPlugin.Enabled = true;
                    await MovementHelpers.Spread(3000);
                }

                if (noFutureTimer.ElapsedMilliseconds >= 103500 && noFutureTimer.ElapsedMilliseconds < 104500)
                {
                    SidestepPlugin.Enabled = false;
                }
            }

            if (peacefire.IsCasting() || (peacefireTimer.IsRunning && peacefireTimer.ElapsedMilliseconds < 30000))
            {
                if (!peacefireTimer.IsRunning || peacefireTimer.ElapsedMilliseconds >= 30000)
                {
                    ReinitPeacekeeperMechanics();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 30000, "Peacefire Avoid");
                    peacefireTimer.Restart();
                }

                if (peacefireTimer.ElapsedMilliseconds < 30000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(peacefireTimer, 30000, useMesh: true);
                }
            }

            // RA-LA (B3)
            if (lifesbreath.IsCasting() || lifesbreathTimer.IsRunning)
            {
                if (!lifesbreathTimer.IsRunning)
                {
                    lifesbreathTimer.Start();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 18000, "Lifesbreath Avoid");
                }

                if (lifesbreathTimer.ElapsedMilliseconds < 18000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(lifesbreathTimer, 18000);
                }

                if (lifesbreathTimer.ElapsedMilliseconds >= 18000)
                {
                    lifesbreathTimer.Reset();
                }
            }

            if (prance.IsCasting() || pranceTimer.IsRunning)
            {
                if (!pranceTimer.IsRunning)
                {
                    pranceTimer.Restart();
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 18000, "Prance Avoid");
                }

                if (pranceTimer.ElapsedMilliseconds < 14000)
                {
                    if (pranceTimer.ElapsedMilliseconds > 13000 && ActionManager.IsSprintReady)
                    {
                        if (Core.Me.IsCasting)
                        {
                            ActionManager.StopCasting();
                        }

                        await Coroutine.Sleep(100);
                        ActionManager.Sprint();
                    }

                    await MovementHelpers.GetClosestAlly.Follow2(pranceTimer, 14000);
                }

                if (pranceTimer.ElapsedMilliseconds >= 14500 && pranceTimer.ElapsedMilliseconds < 15000)
                {
                    Navigator.Stop();
                }

                while (pranceTimer.ElapsedMilliseconds >= 15250 && pranceTimer.ElapsedMilliseconds < 16000)
                {
                    Navigator.PlayerMover.MoveTowards(CalculateLine(Core.Me.Location, MovementHelpers.GetFurthestAlly.Location, 10f));

                    await Coroutine.Yield();
                }

                if (pranceTimer.ElapsedMilliseconds >= 16000)
                {
                    await Coroutine.Sleep(2000);
                    pranceTimer.Reset();
                }
            }

            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            switch (currentSubZoneId)
            {
                // Ostrakon Hexi
                case SubZoneId.PestilentSands:
                    AvoidPestilentSandsTraps();
                    break;
                case SubZoneId.GrebuloffPillars:
                    AvoidGrebuloffPillarsTraps();
                    break;
                case SubZoneId.ShellMound:
                    HandleCausticGrebuloffMechanics();
                    break;

                // Ostrakon Okto
                case SubZoneId.JudgmentDay:
                    AvoidJudgmentDayTraps();
                    break;
                case SubZoneId.DeterrenceGrounds:
                    HandlePeacekeeperMechanics();
                    break;

                // Ostrakon Deka-hepta
                case SubZoneId.ThePlenty:
                    break;
                case SubZoneId.TheWorldTree:
                    HandleRalaMechanics();
                    break;
            }

            lastSubZoneId = currentSubZoneId;

            return false;
        }

        private void AvoidPestilentSandsTraps()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            if (lastSubZoneId != currentSubZoneId)
            {
                Logging.Write(Colors.Aquamarine, $"Adding avoids for sub-zone: {SubZoneId.PestilentSands} Pestilent Sands.");

                foreach ((float radius, Vector3 location) in pestilentSandsTraps)
                {
                    AvoidanceManager.AddAvoidLocation(
                        () => (SubZoneId)WorldManager.SubZoneId == SubZoneId.PestilentSands && Core.Me.InCombat,  // Call WorldManager directly
                        radius: radius,
                        () => location,
                        ignoreIfBlocking: true);
                }
            }
        }

        private void AvoidGrebuloffPillarsTraps()
        {
        }

        private void HandleCausticGrebuloffMechanics()
        {

        }

        private void AvoidJudgmentDayTraps()
        {

        }

        private void HandlePeacekeeperMechanics()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            if (lastSubZoneId != currentSubZoneId)
            {
                BattleCharacter boss = GameObjectManager.GetObjectByNPCId<BattleCharacter>(Peacekeeper);

                if (boss != null)
                {
                    Vector3 bosslocation = boss.Location;
                    Logging.Write(Colors.Aquamarine, $"Adding avoid for {boss.Name} (NpcId:{boss.NpcId}, ObjectId:{boss.ObjectId}).");

                    AvoidanceManager.AddAvoidObject<BattleCharacter>(
                       () => erTimer.IsRunning && erTimer.ElapsedMilliseconds < 25000,
                       radius: 9f,
                       boss.ObjectId);
                }

                foreach ((float radius, Vector3 location) in peacekeeperRing)
                {
                    AvoidanceManager.AddAvoidLocation(
                       () => erTimer.IsRunning,
                       radius: radius,
                       () => location,
                       ignoreIfBlocking: false);
                }
            }
        }

        private void ReinitPeacekeeperMechanics()
        {
            AvoidanceManager.RemoveAllAvoids(i => i.CanRun);
            BattleCharacter boss = GameObjectManager.GetObjectByNPCId<BattleCharacter>(Peacekeeper);

            if (boss != null)
            {
                Vector3 bosslocation = boss.Location;
                Logging.Write(Colors.Aquamarine, $"Adding avoid for {boss.Name} (NpcId:{boss.NpcId}, ObjectId:{boss.ObjectId}).");

                AvoidanceManager.AddAvoidObject<BattleCharacter>(
                    () => erTimer.IsRunning && erTimer.ElapsedMilliseconds < 25000,
                    radius: 9f,
                    boss.ObjectId);
            }

            foreach ((float radius, Vector3 location) in peacekeeperRing)
            {
                AvoidanceManager.AddAvoidLocation(
                   () => erTimer.IsRunning,
                   radius: radius,
                   () => location,
                   ignoreIfBlocking: false);
            }
        }

        private void HandleRalaMechanics()
        {

        }

        // the point of these functions is to take your location and a 2nd location, then calculate a point a distance
        // behind the 2nd location, allowing you to guess the location of a trust member avoiding mechanics who is too
        // slow to follow safely.
        private Vector3 CalculateLine(Vector3 x1, Vector3 x2, float distance)
        {
            double length = Math.Sqrt(Squared(x2.X - (double)x1.X) + Squared(x2.Y - (double)x1.Y) + Squared(x2.Z - (double)x1.Z));

            double unitSlopeX = (x2.X - x1.X) / length;
            double unitSlopeY = (x2.Y - x1.Y) / length;
            double unitSlopeZ = (x2.Z - x1.Z) / length;

            double x = x1.X + (unitSlopeX * (double)distance);
            double y = x1.Y + (unitSlopeY * (double)distance);
            double z = x1.Z + (unitSlopeZ * (double)distance);

            return new Vector3((float)x, (float)y, (float)z);
        }

        private double Squared(double x)
        {
            return x * x;
        }
    }
}
