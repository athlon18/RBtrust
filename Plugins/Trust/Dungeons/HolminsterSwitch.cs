using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
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
            15602, 15609, 15814, 15815, 15816, 15817,
            15818, 15819, 15820, 15822, 15834, 15835,
            15836, 15837, 15838, 15839, 15840, 15841,
            15843, 15845, 15846, 15847, 15848, 15849,
            15886, 16765, 16779, 16780, 16781, 16782,
            16850, 16851, 16852, 17232, 17552,
        };

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.HolminsterSwitch;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            // Tesleen, the Forgiven (得到宽恕的泰丝琳)
            // 15826, 15827                                 :: Exorcise            :: 傩
            HashSet<uint> exorcise = new HashSet<uint>() { 15826, 15827 };
            if (exorcise.IsCasting())
            {
                Vector3 location = new Vector3("79.24156, 0, -81.55303");

                if (Core.Me.Distance(location) > 1f && Core.Me.IsCasting)
                {
                    ActionManager.StopCasting();
                }

                while (Core.Me.Distance(location) > 1f)
                {
                    await CommonTasks.MoveTo(location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 5000, "Exorcise Avoid");
                while (sw.ElapsedMilliseconds < 5000)
                {
                    await MovementHelpers.GetClosestAlly.Follow(7f);
                    await Coroutine.Yield();
                }

                sw.Stop();
            }

            // Philia (斐利亚)
            // 15833, 16777, 16790                          :: Pendulum            :: 钟摆
            // 15842, 16769                                 :: Taphephobia         :: 恐惧症
            HashSet<uint> pendulum = new HashSet<uint>() { 15833, 15842, 16769, 16777, 16790 };
            if (pendulum.IsCasting())
            {
                Vector3 location = new Vector3("117.1188,23,-474.0881");

                if (Core.Me.Distance(location) > 1 && Core.Me.IsCasting)
                {
                    ActionManager.StopCasting();
                }

                while (Core.Me.Distance(location) > 1f)
                {
                    CapabilityManager.Update(CapabilityHandle, CapabilityFlags.Movement, 3000, "Exorcise Avoid");
                    await CommonTasks.MoveTo(location);
                    await Coroutine.Yield();
                }

                await CommonTasks.StopMoving();
                await Coroutine.Sleep(100);
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
