using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using RBTrust.Plugins.Trust.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 71 Holminster Switch dungeon logic.
    /// </summary>
    public class HolminsterSwitch : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.HolminsterSwitch;

        /// <summary>
        /// Set of boss-related monster IDs.
        /// </summary>
        private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            8299,  // Forgiven Dissonance
            8300,  // Tesleen, the Forgiven :: 得到宽恕的泰丝琳
            8301,  // Philia                :: 斐利亚
            8570,  // Iron Chain            :: 锁链
        };

        /// <summary>
        /// Set of spells to dodge by following closest ally.
        /// </summary>
        private static readonly HashSet<uint> SpellsDodgedViaClosest = new HashSet<uint>()
        {
            // 15602, 15609                             :: Heretic's Fork
            // 15814, 16850                             :: Thumbscrew
            // 15815, 16852                             :: Wooden Horse
            // 15816, 16851                             :: Gibbet Cage
            // 15817, 15820                             :: Brazen Bull
            // 15818                                    :: Executioner's Sword
            // 15819                                    :: Light Shot
            // 15822, 15886, 17552                      :: Heretic's Fork
            // 15834, 15835, 15836, 15837, 15838, 15839 :: Fierce Beating
            // 15840, 15841                             :: Cat o' Nine Tails
            // 15843, 16765                             :: Sickly Inferno
            // 15845, 17232                             :: Into the Light
            // 15846                                    :: Right Knout
            // 15847                                    :: Left Knout
            // 15848, 15849                             :: Aethersup
            // 16779, 16780, 16781, 16782               :: Land Rune
            15602, 15609, 15814, 15815, 15816,
            15817, 15818, 15819, 15820, 15822,
            15843, 15845, 15846, 15847, 15848,
            15849, 15886, 16765, 16779, 16780,
            16781, 16782, 16850, 16851, 16852,
            17232, 17552,
        };

        private static readonly HashSet<uint> Exorcise = new HashSet<uint>() { 15826, 15827 };
        private static readonly Vector3 ExorciseStackLoc = new Vector3("79.35034, 0, -81.01664");
        private static readonly int ExorciseDuration = 25_000;

        private static readonly HashSet<uint> Pendulum = new HashSet<uint>() { 15833, 15842, 16769, 16777, 16790 };
        private static readonly Vector3 PendulumDodgeLoc = new Vector3("117.1188,23,-474.0881");

        private static readonly HashSet<uint> FierceBeating = new HashSet<uint>() { 15834, 15835, 15836, 15837, 15838, 15839 };
        private static readonly int FierceBeatingDuration = 32_000;
        private static DateTime fierceBeatingTimestamp = DateTime.MinValue;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.HolminsterSwitch;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            // Tesleen, the Forgiven (得到宽恕的泰丝琳)
            if (Exorcise.IsCasting())
            {
                if (Core.Me.Distance(ExorciseStackLoc) > 1f && Core.Me.IsCasting)
                {
                    ActionManager.StopCasting();
                }

                while (Core.Me.Distance(ExorciseStackLoc) > 1f)
                {
                    await CommonTasks.MoveTo(ExorciseStackLoc);
                    await Coroutine.Yield();
                }

                // Wait in-place for stack marker to go off
                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(5000);

                Stopwatch exorciseTimer = new Stopwatch();
                exorciseTimer.Restart();

                // Create an AOE avoid for the ice puddle where the stack marker went off
                AvoidanceManager.AddAvoidLocation(
                   () => exorciseTimer.IsRunning && exorciseTimer.ElapsedMilliseconds < ExorciseDuration,
                   radius: 6.5f * 1.5f, // Expand to account for stack target maybe standing to the side
                   () => ExorciseStackLoc);

                // Non-targetable but technically .IsVisible copies of Tesleen with the same .NpcId are used to place the outer ice puddles
                // Create AOE avoids on top of them since SideStep doesn't do this automatically
                IEnumerable<GameObject> fakeTesleens = GameObjectManager.GetObjectsByNPCId(8300).Where(obj => !obj.IsTargetable);
                foreach (GameObject fake in fakeTesleens)
                {
                    Vector3 location = fake.Location;

                    ff14bot.Pathing.Avoidance.AvoidInfo a = AvoidanceManager.AddAvoidLocation(
                       () => exorciseTimer.IsRunning && exorciseTimer.ElapsedMilliseconds < ExorciseDuration,
                       radius: 6.5f,
                       () => location);
                }
            }

            // Philia (斐利亚)
            // 15833, 16777, 16790 :: Pendulum    :: 钟摆
            // 15842, 16769        :: Taphephobia :: 恐惧症
            if (Pendulum.IsCasting())
            {
                if (Core.Me.Distance(PendulumDodgeLoc) > 1 && Core.Me.IsCasting)
                {
                    ActionManager.StopCasting();
                }

                while (Core.Me.Distance(PendulumDodgeLoc) > 1f)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 3000, "Pendulum Avoid");
                    await CommonTasks.MoveTo(PendulumDodgeLoc);
                    await Coroutine.Yield();
                }

                await CommonTasks.StopMoving();
                await Coroutine.Sleep(100);
            }

            if (FierceBeating.IsCasting() && fierceBeatingTimestamp.AddMilliseconds(FierceBeatingDuration) < DateTime.Now)
            {
                GameObject philia = GameObjectManager.GetObjectsByNPCId(8301).FirstOrDefault(obj => obj.IsTargetable);

                if (philia != null)
                {
                    Vector3 location = philia.Location;
                    uint objectId = philia.ObjectId;

                    fierceBeatingTimestamp = DateTime.Now;
                    Stopwatch fierceBeatingTimer = new Stopwatch();
                    fierceBeatingTimer.Restart();

                    // Create an AOE avoid for the orange swirly under the boss
                    AvoidanceManager.AddAvoidObject<GameObject>(
                        canRun: () => fierceBeatingTimer.IsRunning && fierceBeatingTimer.ElapsedMilliseconds < FierceBeatingDuration,
                        radius: 11f,
                        unitIds: objectId);

                    // Attach very wide cone avoid pointing out the boss's right, forcing bot to left side
                    // Boss spins clockwise and front cleave comes quickly, so disallow less-safe right side
                    // Position + rotation will auto-update as the boss moves + turns!
                    AvoidanceManager.AddAvoidUnitCone<GameObject>(
                        canRun: () => fierceBeatingTimer.IsRunning && fierceBeatingTimer.ElapsedMilliseconds < FierceBeatingDuration,
                        objectSelector: (obj) => obj.ObjectId == objectId,
                        leashPointProducer: () => location,
                        leashRadius: 40f,
                        rotationDegrees: -90f,
                        radius: 25f,
                        arcDegrees: 345f);
                }
            }

            // Default (缺省)
            if (SpellsDodgedViaClosest.IsCasting())
            {
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 1500, "Spells Avoid");
                await MovementHelpers.GetClosestAlly.Follow();
            }

            // SideStep (回避)
            BossIds.ToggleSideStep();

            return false;
        }
    }
}
