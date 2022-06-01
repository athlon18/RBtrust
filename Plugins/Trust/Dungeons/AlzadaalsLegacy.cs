using ff14bot;
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
    /// Lv. 85 Vanaspati dungeon logic.
    /// </summary>
    public class AlzadaalsLegacy : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.AlzadaalsLegacy;

        // Ambujam
        // Big Wave         28512
        // Tentacle Dig     28501, 28505
        // Toxic Fountain   29466

        // Armored Chariot
        // Articulated Bits 28441
        // Diffusion Ray    28446
        // Rail Cannon      28447

        // Kapikul
        // Billowing Bolts  28528
        // Spin Out         28515
        // Crewel Slice     28530
        // Wild Weave       28521
        // Power Serge      28522
        // Rotary Gale      28524
        // Magnitude Opus   28526
        private readonly HashSet<uint> spread = new HashSet<uint>()
        {
            28524,
        };

        private readonly HashSet<uint> stack = new HashSet<uint>()
        {
            28526, 28522,
        };

        private readonly HashSet<uint> articulatedBits = new HashSet<uint>()
        {
            28441,
        };

        private readonly HashSet<uint> spinOut = new HashSet<uint>()
        {
            28515,
        };

        private readonly HashSet<uint> tentacleDig = new HashSet<uint>()
        {
            28501, 28505,
        };

        private readonly HashSet<uint> toxicFountain = new HashSet<uint>()
        {
            29466,
        };

        private readonly Stopwatch spreadTimer = new Stopwatch();
        private readonly Stopwatch articulatedBitsTimer = new Stopwatch();
        private readonly Stopwatch spinOutTimer = new Stopwatch();
        private readonly Stopwatch tentacleDigTimer = new Stopwatch();
        private readonly Stopwatch toxicFountainTimer = new Stopwatch();

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.AlzadaalsLegacy;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
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
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 2500, "Stack Mechanic In Progress");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            if (tentacleDig.IsCasting() || (tentacleDigTimer.IsRunning && tentacleDigTimer.ElapsedMilliseconds < 18000))
            {
                if (!tentacleDigTimer.IsRunning || tentacleDigTimer.ElapsedMilliseconds >= 18000)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 18000, "Tentacle Dig Avoid");
                    tentacleDigTimer.Restart();
                }

                if (tentacleDigTimer.ElapsedMilliseconds < 18000)
                {
                    if (Core.Me.IsTank())
                    {
                        await MovementHelpers.GetClosestAlly.Follow2(tentacleDigTimer, 18000, useMesh: true);
                    }
                    else
                    {
                        await MovementHelpers.GetClosestTank.Follow2(tentacleDigTimer, 18000, useMesh: true);
                    }
                }
            }

            if (toxicFountain.IsCasting() || (toxicFountainTimer.IsRunning && toxicFountainTimer.ElapsedMilliseconds < 12000))
            {
                if (!toxicFountainTimer.IsRunning || toxicFountainTimer.ElapsedMilliseconds >= 12000)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 12000, "Toxic Fountain Avoid");
                    toxicFountainTimer.Restart();
                }

                if (toxicFountainTimer.ElapsedMilliseconds < 12000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(toxicFountainTimer, 12000, useMesh: true);
                }
            }

            if (articulatedBits.IsCasting() || (articulatedBitsTimer.IsRunning && articulatedBitsTimer.ElapsedMilliseconds < 24000))
            {
                if (!articulatedBitsTimer.IsRunning || articulatedBitsTimer.ElapsedMilliseconds >= 24000)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 24000, "Articulated Bits Avoid");
                    articulatedBitsTimer.Restart();
                }

                if (articulatedBitsTimer.ElapsedMilliseconds < 24000)
                {
                    await MovementHelpers.GetClosestAlly.Follow2(articulatedBitsTimer, 24000, useMesh: true);
                }
            }

            if (spinOut.IsCasting() || spinOutTimer.IsRunning)
            {
                if (!spinOutTimer.IsRunning)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 18000, "Spin Out");
                    spinOutTimer.Start();
                }

                // TODO proper logic
                if (spinOutTimer.ElapsedMilliseconds >= 18000)
                {
                    spinOutTimer.Reset();
                }
            }

            return false;
        }
    }
}
