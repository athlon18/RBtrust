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
    /// Lv. 83 The Tower of Babil dungeon logic.
    /// </summary>
    public class TheTowerOfBabil : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        ///

        // SPELLIDS
        
        // BARNABAS (B1)	Ground and Pound	25159
        //			        Ground and Pound	25322   
        //	        		Dynamic Pound		25326
        //		        	Dynamic Pound		25157
        //	        		Shocking Force (St)	25324
        //	        		Dynamic Scrapline	25158
        //		        	Dynamic Scrapline	25328
        //	        		Thundercall		    25325
        //		        	Rolling Scrapline	25323
        //
        // LUGAE (B2)		Thermal Suppression	25338
        //      			Magitek Missile		25334
        //		        	Magitek Ray		    25340
        //		        	Magitek Chakram		25331
        //		        	Magitek Explosive	25336
        //		        	Downpour	    	25333
        //
        // ANIMA (B3)   	Lunar Nail		    25342
        //		        	Phantom Pain		21182
        //		        	Mega Graviton		25344
        //		        	Pater Patriae		25350
        //		        	Boundless Pain		25347
        //		        	Imperatum		    25353
        //		        	Obliviating Claw	25355
        //		    	    Obliviating Claw 2	25354
        //                  Erupting Pain       25351

        private HashSet<uint> follow = new HashSet<uint>()
        {
            21182, 25324,
        };

        private HashSet<uint> magnet = new HashSet<uint>()
        {
            25326, 25157, 25158, 25328,
        };

        private HashSet<uint> toad = new HashSet<uint>()
        {
            25333,
        };

        private HashSet<uint> mini = new HashSet<uint>()
        {
            25331,
        };

        private HashSet<uint> boundlesspain = new HashSet<uint>()
        {
            25347,
        };

        private HashSet<uint> spread = new HashSet<uint>()
        {
            25351,
        };

        private HashSet<uint> claw2 = new HashSet<uint>()
        {
            25354,
        };
        
        private CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();
        private PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();
        private Stopwatch followSW = new Stopwatch();
        private Stopwatch magnetSW = new Stopwatch();
        private Stopwatch miniSW = new Stopwatch();
        private Stopwatch toadSW = new Stopwatch();
        private Stopwatch boundlesspainSW = new Stopwatch();
        private Stopwatch claw2SW = new Stopwatch();
        private Stopwatch spreadSW = new Stopwatch();


        public new const ZoneId ZoneId = Data.ZoneId.TheTowerOfBabil;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheTowerOfBabil;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {

            if (follow.IsCasting() || followSW.IsRunning)
            {

                if (!followSW.IsRunning)
                {

                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    followSW.Restart();

                }

                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2500, "Follow/Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();

                if (!follow.IsCasting())
                {

                    sidestepPlugin.Enabled = true;
                    followSW.Reset();

                }


            }

            if (magnet.IsCasting() || magnetSW.IsRunning)
            {

                if (!magnetSW.IsRunning)
                {
    
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 12000, "Magnet Spell In Progress");
                    magnetSW.Restart();

                }

                if (magnetSW.ElapsedMilliseconds < 12000)
                {

                    Vector3 location = new Vector3("-314.4527, -176, 70.98297");

                    if (Core.Me.Distance(location) > 1f)
                    {
                        Navigator.PlayerMover.MoveTowards(location);
                    }

                    else MovementManager.MoveStop(); 

                }

                if (magnetSW.ElapsedMilliseconds >= 12000)
                {

                    sidestepPlugin.Enabled = true;
                    magnetSW.Reset();

                }
            }

            if (toad.IsCasting() || toadSW.IsRunning)
            {

                if (!toadSW.IsRunning)
                {

                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 30000, "Shapeshift Mechanic In Progress");
                    toadSW.Restart();

                }

                if (toadSW.ElapsedMilliseconds < 12000)
                {
                     Vector3 location = new Vector3("214.2467, 0.9999993, 306.0189");

                     if (Core.Me.Distance(location) < 1f)
                    {  
                        MovementManager.MoveStop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);
                }

                if (toadSW.ElapsedMilliseconds >= 12000 && toadSW.ElapsedMilliseconds < 30000 )
                {
                    await MovementHelpers.GetClosestAlly.Follow();
                }

                if (toadSW.ElapsedMilliseconds >= 30000)
                {

                    sidestepPlugin.Enabled = true;
                    toadSW.Reset();

                }

            }

            if (mini.IsCasting() || miniSW.IsRunning)
            {

                if (!miniSW.IsRunning)
                {

                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 24000, "Shapeshift Mechanic In Progress");
                    miniSW.Restart();

                }

                if (miniSW.ElapsedMilliseconds < 12000)
                {
                     Vector3 location = new Vector3("227.0484, 1.00001, 305.9774");

                    if (Core.Me.Distance(location) < 1f)
                    {  
                        MovementManager.MoveStop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);
                }

                if (miniSW.ElapsedMilliseconds >= 12000 && miniSW.ElapsedMilliseconds < 24000 )
                {
                     Vector3 location = new Vector3("220.9772, 1, 305.9483");

                    if (Core.Me.Distance(location) < 1f)
                    {  
                        MovementManager.MoveStop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);
                }

                if (miniSW.ElapsedMilliseconds >= 24000)
                {

                    sidestepPlugin.Enabled = true;
                    miniSW.Reset();

                }

            }
                        

            if (claw2.IsCasting() || claw2SW.IsRunning)
            {

                if (!claw2SW.IsRunning)
                {

                    
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 12000, "Obliviating Claw 2 In Progress");
                    claw2SW.Restart();

                }

                if (claw2SW.ElapsedMilliseconds < 6000)
                {

                    Vector3 location = new Vector3("16.74083, 120, -406.9069");

                    if (Core.Me.Distance(location) < 1f)
                    {  
                        MovementManager.MoveStop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);

                }

                if (claw2SW.ElapsedMilliseconds >= 6000 && claw2SW.ElapsedMilliseconds < 12000)
                {

                    Vector3 location = new Vector3("-15.15774, 120, -408.2812");

                    if (Core.Me.Distance(location) < 1f)
                    {  
                        MovementManager.MoveStop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);

                }

                if (claw2SW.ElapsedMilliseconds >= 12000 )
                {

                    sidestepPlugin.Enabled = true;
                    claw2SW.Reset();

                }


            }

            if (boundlesspain.IsCasting() || boundlesspainSW.IsRunning)
            {
                if (!boundlesspainSW.IsRunning)
                {
                    
                    sidestepPlugin.Enabled = false;
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);
                    CapabilityManager.Clear();
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 18000, "Boundless Pain Avoid");
                    boundlesspainSW.Restart();
                }

                if (boundlesspainSW.ElapsedMilliseconds >= 8000 && boundlesspainSW.ElapsedMilliseconds < 18000)
                {

                    Vector3 location = new Vector3("11.11008, 479.9997, -199.1336");
                    
                    if (Core.Me.Distance(location) < 1f)
                    {  
                        Navigator.Stop();    
                    }

                    else Navigator.PlayerMover.MoveTowards(location);

                }

                if (boundlesspainSW.ElapsedMilliseconds >= 18000)
                {
                    sidestepPlugin.Enabled = true;
                    boundlesspainSW.Reset();
                }


            }

            if (spread.IsCasting() || spreadSW.IsRunning)
            {
                if (!spreadSW.IsRunning)
                { 
                        CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 5000, "Spread");
                        //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 5000, "Spread");
                        spreadSW.Start();
                }

                if (spreadSW.ElapsedMilliseconds < 5000)
                {

                    await MovementHelpers.Spread(5000);

                }

                if (spreadSW.ElapsedMilliseconds >= 5000)
                {

                    spreadSW.Reset();
                    AvoidanceManager.RemoveAllAvoids(i=> i.CanRun);

                }
            }

            // Avoid Claw Game in final zone only if in combat, otherwise frequent stucks
            if (WorldManager.SubZoneId == 4133)
            {
                if (!Core.Me.InCombat)
                {
                    sidestepPlugin.Enabled = false;
                }
                else
                {
                    sidestepPlugin.Enabled = true;
                }

            }

            return false;
        }

            
        
    }
}
