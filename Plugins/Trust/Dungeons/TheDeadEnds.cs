using Clio.Utilities;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.Pathing;
using ff14bot.Pathing.Avoidance;
using ff14bot.Enums;
using ff14bot.NeoProfiles;
using ff14bot.Behavior;
using ff14bot.Helpers;
using System;
using System.Linq;
using System.Runtime;
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

         private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            10313, 10315, 10316 
        };

        private const int CausticGrebuloff = 10313;
        private const int Peacekeeper = 10315;
        private const int Rala = 10316;

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
            //
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
        };
        private readonly List<(float Radius, Vector3 Location)> PeacekeeperRing = new List<(float Radius, Vector3 Location)>()
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


        // the point of these functions is to take your location and a 2nd location, then calculate a point a distance
        // behind the 2nd location, allowing you to guess the location of a trust member avoiding mechanics who is too
        // slow to follow safely.

        private Vector3 calculateLine(Vector3 x1, Vector3 x2, float distance)

        {

            double length = Math.Sqrt(squared((double)x2.X - (double)x1.X) + squared(((double)x2.Y - (double)x1.Y)) + squared(((double)x2.Z - (double)x1.Z)));
            double unitSlopeX = (x2.X - x1.X) / length;
            double unitSlopeY = (x2.Y - x1.Y) / length;
            double unitSlopeZ = (x2.Z - x1.Z) / length;
            double x = (double)x1.X + unitSlopeX * (double)distance;
            double y = (double)x1.Y + unitSlopeY * (double)distance;
            double z = (double)x1.Z + unitSlopeZ * (double)distance;
            var result = new Vector3((float)x, (float)y, (float)z);
            return result;

        }

        private double squared(double x)
        {
            return x * x;
        }


        /// <summary>
        /// Tracks sub-zone since last tick for environmental decision making.
        /// </summary>
        private SubZoneId lastSubZoneId = SubZoneId.NONE;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheDeadEnds;

        /// <inheritdoc/>
        ///
        private Stopwatch sw = new Stopwatch();
        private Stopwatch NFsw = new Stopwatch();
        private Stopwatch ERsw = new Stopwatch();
        private Stopwatch miassw = new Stopwatch();
        private Stopwatch PFsw = new Stopwatch();
        private Stopwatch COsw = new Stopwatch();
        private Stopwatch OTFsw = new Stopwatch();
        private Stopwatch spreadsw = new Stopwatch();
        private Stopwatch LBsw = new Stopwatch();
        private Stopwatch PRsw = new Stopwatch();

        public override async Task<bool> RunAsync()
        {
            CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();
            PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();

            if (!Core.Me.InCombat)
                { 
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 0, "End Combat");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 0, "End Combat");
                    sw.Reset();
                    spreadsw.Reset();
                    NFsw.Reset();
                    ERsw.Reset();
                    miassw.Reset();
                    PFsw.Reset();
                    COsw.Reset();
                    LBsw.Reset();
                    PRsw.Reset();
                }

            // Misc Spells
            //  Peacekeeper     25936   Decimation (AoE)
            //                  28360   Electromagnetic Repellant (danger zones on boss and edges)
            //                  28351   Order to Fire (Lasers AoE)
            //                  25925   No Future (targeting circles AoE)
            //                  25933   Peacefire (Clockwise AoE)
            //                  25931   Eclipsing Exhaust
            //                  25935   Elimination (Tankbuster)
            //                  25940,    Ra-La              Lifesbreath
            //                  25945,  //  Ra-La              Benevolence
            //                  25943,  //  Ra-La               Loving Embrace
            //    25944,  //  Ra-La               Loving Embrace 


            // GENERIC MECHANICS

            HashSet<uint> spread = new HashSet<uint>()
            {
                25923,//    Caustic Grebuloff   Befoulment (Spread Mechanic)
                25947,//    Ra-La               Still Embrace
            };

            if (spread.IsCasting() || spreadsw.IsRunning)
            {
                if (!spreadsw.IsRunning)
                { 
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 5000, "Spread");
                        //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 5000, "Spread");
                        spreadsw.Start();
                }
                if (spreadsw.ElapsedMilliseconds < 5000)
                    { 
                    await MovementHelpers.Spread(5000);
                    }

                if (spreadsw.ElapsedMilliseconds >= 5000)
                    spreadsw.Reset();
            }

            HashSet<uint> stack = new HashSet<uint>()
            { 
                28347, //  Caustic Grebuloff   Wave of Nausea
                25921, //  Caustic Grebuloff   Blighted Water
                //28360,
                25931,
                //25940,    Ra-La              Lifesbreath
                25945,  //  Ra-La              Benevolence
                25943,  //  Ra-La               Loving Embrace
                25944,  //  Ra-La               Loving Embrace
                27717,  //  Xenoflora           Creeping Hush
            };

            

            if (stack.IsCasting())
            {
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            HashSet<uint> TBavoid = new HashSet<uint>()
            { 
                25935, // Peacekeeper      Elimination (Tankbuster)
            };

            if (TBavoid.IsCasting())
            {
                BattleCharacter TankExists = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                        .Where(obj => !obj.IsDead && MovementHelpers.AllPartyTankIds.Contains(obj.NpcId))
                                        .FirstOrDefault();
                //Logging.Write(Colors.Aquamarine, $"TankExists: {TankExists.Name}");
                if (TankExists != null)
                {

                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2500, "Tankbuster Spread In Progress");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 2500, "Tankbuster Spread In Progress");
                    await MovementHelpers.GetClosestAlly.Follow();

                }
            }


            // CAUSTIC GREBULOFF (B1)
            HashSet<uint> miasmata = new HashSet<uint>()
            {
                25916, // Caustic Grebuloff -   Miasmata 
            };

            if (miasmata.IsCasting() || (miassw.IsRunning && miassw.ElapsedMilliseconds < 19000))
            {
                if (!miassw.IsRunning || miassw.ElapsedMilliseconds > 19000)
                {
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 19000, "Miasmata Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 19000, "Miasmata Avoid");
                    miassw.Restart();
                }
                else await MovementHelpers.GetClosestAlly.Follow2(miassw, 19000);
               //sw.Reset();
            }

            HashSet<uint> coughup = new HashSet<uint>()
            {
                25917, //                       Cough Up
            };
            if (coughup.IsCasting() || COsw.IsRunning)
            {

                if (!COsw.IsRunning)
                { 
                    COsw.Start();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 15000, "Cough Up Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 15000, "Cough Up Avoid");
                }

                 
                    if (COsw.ElapsedMilliseconds < 12000)
                    {
                        if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                MovementManager.MoveStop();
                                                            }

                        sidestepPlugin.Enabled = true;
                    }

                    if (COsw.ElapsedMilliseconds >= 12000 && COsw.ElapsedMilliseconds < 15000)
                    {
                        
                        sidestepPlugin.Enabled = false;
                        await MovementHelpers.GetClosestAlly.Follow2(COsw, 15000);
                    }
                    if (COsw.ElapsedMilliseconds > 15000)
                    { 
                       COsw.Reset();
                    }
            }
          

            // PEACEKEEPER (B2)

            
            HashSet<uint> peacefire = new HashSet<uint>()
            {
             25933,
            };           

            HashSet<uint> nofuture = new HashSet<uint>()
            {
             25925,
            };

             HashSet<uint> electrorep = new HashSet<uint>()
            {
             28360,
            };                         

            HashSet<uint> ordertofire = new HashSet<uint>()
            {
                28351,
            };

           

            if (electrorep.IsCasting())
            {
                sidestepPlugin.Enabled = false;
                if (!ERsw.IsRunning || ERsw.ElapsedMilliseconds > 25000)
                    ERsw.Restart();
            }

            if ((ordertofire.IsCasting() || OTFsw.IsRunning) && !NFsw.IsRunning)
            {
                if (!OTFsw.IsRunning)
                    {
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 8000, "Order To Fire Avoid");
                        //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 8000, "Order To Fire Avoid");
                        OTFsw.Restart();
                    }

                if (OTFsw.ElapsedMilliseconds < 4500)
                    {
                        await MovementHelpers.GetClosestDps.Follow2(OTFsw, 4500);
                        
                    }
                                
                if (OTFsw.ElapsedMilliseconds > 4500 && OTFsw.ElapsedMilliseconds < 8000)
                    {
                    
                        if (!AvoidanceManager.IsRunningOutOfAvoid)
                        {
                            MovementManager.MoveStop();
                        }

                        
                        await MovementHelpers.Spread(3500);
                                            }
                if (OTFsw.ElapsedMilliseconds >= 8000)                
                    OTFsw.Reset();
                    
            }
                        
            if (nofuture.IsCasting() || NFsw.IsRunning)
                
            {
                // Logging.Write(Colors.Aquamarine, $"NFsw Stopwatch:{NFsw.ElapsedMilliseconds}");                           
                
                if (!NFsw.IsRunning)
                    { 
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 19000, "No Future 1 Avoid");
                        //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 19000, "No Future 1 Avoid");
                        NFsw.Start();
                    }
                              
                                
                if (NFsw.ElapsedMilliseconds < 16000)
                {
                     await MovementHelpers.GetClosestAlly.Follow2(NFsw, 16000);                                          
                }
                                
                if (NFsw.ElapsedMilliseconds >= 16000 && NFsw.ElapsedMilliseconds < 19000)
                {
                     await MovementHelpers.Spread(3000);

                        if (!AvoidanceManager.IsRunningOutOfAvoid)
                            {
                                MovementManager.MoveStop();
                            }

                    
                    
                }

                
                if (NFsw.ElapsedMilliseconds >= 84000 && NFsw.ElapsedMilliseconds < 85000)
                {
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 20000, "No Future 2 Avoid");
                        //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 20000, "No Future 2 Avoid");
                }

                if (NFsw.ElapsedMilliseconds >= 84000 && NFsw.ElapsedMilliseconds < 101000)
                {
                                 
                        await MovementHelpers.GetClosestAlly.Follow2(NFsw, 101000);
                }



                if (NFsw.ElapsedMilliseconds >= 101000 && NFsw.ElapsedMilliseconds < 104000)
                {
                                if (!AvoidanceManager.IsRunningOutOfAvoid)
                                {
                                    MovementManager.MoveStop();
                                }

                                await MovementHelpers.Spread(3000);                        
                }
                
            }

             if (peacefire.IsCasting() || (PFsw.IsRunning && PFsw.ElapsedMilliseconds < 28000))
             {
                if (!PFsw.IsRunning || PFsw.ElapsedMilliseconds >= 28000)
                {
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 28000, "Peacefire Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 28000, "Peacefire Avoid");
                    PFsw.Restart();
                }

                if (PFsw.ElapsedMilliseconds < 28000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(PFsw, 28000);
 
                }
                             
             }            

            

            // RA-LA (B3)
            HashSet<uint> lifesbreath = new HashSet<uint>()
            {
                25940, //  Ra-La               Lifesbreath
            };


            HashSet<uint> prance = new HashSet<uint>()
            { 
                25937, //  Ra-La               Prance
            };

            if(lifesbreath.IsCasting() || LBsw.IsRunning)
            {
                if (!LBsw.IsRunning)
                {
                    LBsw.Start();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 18000, "Lifesbreath Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 18000, "Lifesbreath Avoid");
                }

                if (LBsw.ElapsedMilliseconds < 18000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(LBsw, 18000);
                }

                if (LBsw.ElapsedMilliseconds >= 18000)
                {
                    LBsw.Reset();

                }
            }



            if (prance.IsCasting() || PRsw.IsRunning)
            {
                if (!PRsw.IsRunning)
                { 
                PRsw.Restart();
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 18000, "Prance Avoid");
                //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 18000, "Prance Avoid");
                }

                if (PRsw.ElapsedMilliseconds < 14000)
                {
                    if (PRsw.ElapsedMilliseconds > 13000 && ActionManager.IsSprintReady)
                    {
                        ActionManager.Sprint();
                        //await Coroutine.Wait(1000, () => !ActionManager.IsSprintReady);
                    }

                    await MovementHelpers.GetClosestAlly.Follow2(PRsw, 14000);
                }

                if (PRsw.ElapsedMilliseconds >= 14500 && PRsw.ElapsedMilliseconds < 15000)
                {
                    Navigator.Stop();
                }
                                
                if (PRsw.ElapsedMilliseconds >= 15250)
                {
                    
                    Navigator.PlayerMover.MoveTowards(calculateLine(Core.Me.Location, MovementHelpers.GetFurthestAlly.Location, 10f));
                    Task.Delay(100);
                    await Coroutine.Sleep(2500);
                    PRsw.Reset();
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
                    break;
                case SubZoneId.ShellMound:
                    break;

                // Ostrakon Okto
                case SubZoneId.JudgmentDay:
                    break;
                case SubZoneId.DeterrenceGrounds:
                    HandlePeacekeeperMechanics();
                    break;

                // Ostrakon Deka-hepta
                case SubZoneId.ThePlenty:
                    break;
                case SubZoneId.TheWorldTree:
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

                foreach (var (radius, location) in pestilentSandsTraps)
                {
                    AvoidanceManager.AddAvoidLocation(
                        () => (SubZoneId)WorldManager.SubZoneId == SubZoneId.PestilentSands,  // Call WorldManager directly
                        radius: radius,
                        () => location,
                        ignoreIfBlocking: true);
                }
            }
        }
        
        private void HandlePeacekeeperMechanics()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            if (lastSubZoneId != currentSubZoneId)
            { 
                var boss = GameObjectManager.GetObjectByNPCId<BattleCharacter>(Peacekeeper);              
                

                if (boss != null)
                {
                    var bosslocation = boss.Location;
                    Logging.Write(Colors.Aquamarine, $"Adding avoid for {boss.Name} (NpcId:{boss.NpcId}, ObjectId:{boss.ObjectId}).");

                     AvoidanceManager.AddAvoidObject<BattleCharacter>(
                        () => (ERsw.IsRunning && ERsw.ElapsedMilliseconds < 25000),
                        radius: 9f,
                        boss.ObjectId);

                }
                 
                

                foreach (var (radius, location) in PeacekeeperRing)
                {
                         AvoidanceManager.AddAvoidLocation(
                            () => (ERsw.IsRunning),
                            radius: radius,
                            () => location,
                            ignoreIfBlocking: false);
                 }
            }            
        } 
    }
}
