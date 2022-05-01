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
    /// Lv. 85 Vanaspati dungeon logic.
    /// </summary>
    public class AlzadaalsLegacy : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.AlzadaalsLegacy;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.AlzadaalsLegacy;

        //Ambujam

        //Big Wave          	28512
        //Tentacle Dig      	28501, 28505
        //Toxic Fountain	    29466

        //Armored Chariot

        //Articulated Bits  	28441
        //Diffusion Ray	    	28446
        //Rail Cannon	    	28447

        //Kapikulu

        //Billowing Bolts		28528
        //Spin Out	        	28515
        //Crewel Slice	    	28530
        //Wild Weave	    	28521
        //Power Serge	    	28522
        //Rotary Gale	    	28524
        //Magnitude Opus        28526

        private HashSet<uint> spread = new HashSet<uint>()
        {
                28524,
        };

        private HashSet<uint> stack = new HashSet<uint>()
        {
                28526, 28522, 
        };

        private HashSet<uint> articulatedbits = new HashSet<uint>()
        {
                28441,
        };

        private HashSet<uint> spinout = new HashSet<uint>()
        {
                28515,
        };

        private HashSet<uint> tentacledig = new HashSet<uint>()
        {
                28501, 28505,
        };

        private HashSet<uint> toxicfountain = new HashSet<uint>()
        {
                29466,
        };

        private Stopwatch SPsw = new Stopwatch();
        private Stopwatch ABsw = new Stopwatch();
        private Stopwatch RGsw = new Stopwatch();
        private Stopwatch SOsw = new Stopwatch();
        private Stopwatch TDsw = new Stopwatch();
        private Stopwatch TFsw = new Stopwatch();

        private CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();
        private PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            if (spread.IsCasting() || SPsw.IsRunning)
            {
                if (!SPsw.IsRunning)
                {
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 5000, "Spread");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 5000, "Spread");
                    SPsw.Start();
                }
                if (SPsw.ElapsedMilliseconds < 5000)
                {
                    await MovementHelpers.Spread(5000);
                }

                if (SPsw.ElapsedMilliseconds >= 5000)
                    SPsw.Reset();
            }

            if (stack.IsCasting())
            {
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2500, "Stack Mechanic In Progress");
                //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 2500, "Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            if (tentacledig.IsCasting() || (TDsw.IsRunning && TDsw.ElapsedMilliseconds < 18000))
            {
                if (!TDsw.IsRunning || TDsw.ElapsedMilliseconds >= 18000)
                {

                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 18000, "Tentacle Dig Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 18000, "Tentacle Dig Avoid");
                    TDsw.Restart();

                }

                if (TDsw.ElapsedMilliseconds < 18000)
                {
                    if (Core.Me.IsTank())
                    {

                        await MovementHelpers.GetClosestAlly.Follow2(TDsw, 18000, useMesh: true);

                    }
                    else await MovementHelpers.GetClosestTank.Follow2(TDsw, 18000, useMesh: true);

                }

            }

            if (toxicfountain.IsCasting() || (TFsw.IsRunning && TFsw.ElapsedMilliseconds < 12000))
            {
                if (!TFsw.IsRunning || TFsw.ElapsedMilliseconds >= 12000)
                {

                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 12000, "Toxic Fountain Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 12000, "Toxic Fountain Avoid");
                    TFsw.Restart();

                }

                if (TFsw.ElapsedMilliseconds < 12000)
                {
                   
                        await MovementHelpers.GetClosestAlly.Follow2(TFsw, 12000, useMesh: true);

                }

            }

            if (articulatedbits.IsCasting() || (ABsw.IsRunning && ABsw.ElapsedMilliseconds < 24000))
            {
                if (!ABsw.IsRunning || ABsw.ElapsedMilliseconds >= 24000)
                {
                  
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 24000, "Articulated Bits Avoid");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 24000, "Articulated Bits Avoid");
                    ABsw.Restart();

                }

                if (ABsw.ElapsedMilliseconds < 24000)
                {

                    await MovementHelpers.GetClosestAlly.Follow2(ABsw, 24000, useMesh: true);

                }

            }

            if (spinout.IsCasting() || SOsw.IsRunning)
            {
                if (!SOsw.IsRunning)
                {
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 18000, "Spin Out");
                    //CapabilityManager.Update(TrustHandle, CapabilityFlags.Facing, 18000, "Spin Out");
                    SOsw.Start();

                }

                // TODO proper logic

                if (SOsw.ElapsedMilliseconds >= 18000)
                    SOsw.Reset();
            }



            return false;
        }
    }
}
