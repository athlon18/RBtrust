using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Navigation;
using ff14bot.Objects;
using RBTrust.Plugins.Trust.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RBTrust.Plugins.Trust.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="BattleCharacter"/>.
    /// </summary>
    internal static class BattleCharacterExtensions
    {
        private static readonly Stopwatch MoveStopTimer = new Stopwatch();
        private static Vector3 moveStopDc;

        /// <summary>
        /// Determines if <see cref="BattleCharacter"/> is in Tank role.
        /// </summary>
        /// <param name="bc"><see cref="BattleCharacter"/> to be classified.</param>
        /// <returns><see langword="true"/> if in Tank role.</returns>
        public static bool IsTank(this BattleCharacter bc)
        {
            return ClassJobRoles.Tanks.Contains(bc.CurrentJob);
        }

        /// <summary>
        /// Determines if <see cref="BattleCharacter"/> is in DPS role.
        /// </summary>
        /// <param name="bc"><see cref="BattleCharacter"/> to be classified.</param>
        /// <returns><see langword="true"/> if in DPS role.</returns>
        public static bool IsDPS(this BattleCharacter bc)
        {
            return ClassJobRoles.DPS.Contains(bc.CurrentJob);
        }

        /// <summary>
        /// Determines if <see cref="BattleCharacter"/> is in Healer role.
        /// </summary>
        /// <param name="bc"><see cref="BattleCharacter"/> to be classified.</param>
        /// <returns><see langword="true"/> if in Healer role.</returns>
        public static bool IsHealer(this BattleCharacter bc)
        {
            return ClassJobRoles.Healers.Contains(bc.CurrentJob);
        }

        /// <summary>
        /// Follows the specified <see cref="BattleCharacter"/>.
        /// </summary>
        /// <param name="bc">Character to follow.</param>
        /// <param name="followDistance">Distance to follow at.</param>
        /// <param name="msWait">Time between movement ticks, in milliseconds.</param>
        /// <param name="useMesh">Whether to use Nav Mesh or move blindly.</param>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task<bool> Follow(this BattleCharacter bc, float followDistance = 0.3f, int msWait = 0, bool useMesh = false)
        {
            if (bc == null)
            {
                return true;
            }

            float curDistance = Core.Me.Distance2D(bc);

            if (curDistance < followDistance)
            {
                return true;
            }

            MoveStopTimer.Reset();

            while (!Core.Me.IsDead && Core.Me.InCombat)
            {
                curDistance = Core.Me.Distance2D(bc);
                if (curDistance < followDistance)
                {
                    break;
                }

                if (Core.Me.IsDead)
                {
                    return false;
                }

                Logging.Write(Colors.Aquamarine, $"Following {bc.Name} [Distance2D: {curDistance}]");

                if (useMesh)
                {
                    await CommonTasks.MoveTo(bc.Location);
                }
                else
                {
                    Navigator.PlayerMover.MoveTowards(bc.Location);
                    await Coroutine.Yield();
                }

                curDistance = Core.Me.Distance2D(bc);

                if (curDistance < followDistance + 0.5f)
                {
                    moveStopDc = bc.Location;

                    if (await Coroutine.Wait(100, () => bc.Distance2D() < followDistance || bc.Distance2D(moveStopDc) > 0))
                    {
                        if (bc.Distance2D(moveStopDc) > 0)
                        {
                            continue;
                        }

                        Navigator.PlayerMover.MoveStop();
                    }
                }

                await Coroutine.Sleep(msWait);
            }

            return true;
        }

        public static async Task<bool> Follow2(this BattleCharacter bc, Stopwatch sw, double timeToFollow = 3000, float followDistance = 0.3f, int msWait = 0, bool useMesh = false)
        {
            if (bc == null)
            {
                return true;
            }

            float curDistance = Core.Me.Location.Distance2D(bc.Location);

            if (!sw.IsRunning)
            {
                sw.Restart();
            }

            if (!Core.Me.IsDead && Core.Me.InCombat && (sw.ElapsedMilliseconds <= timeToFollow))
            {
                Logging.Write(Colors.Aquamarine, $"Following {bc.Name} [Distance: {curDistance}]");

                if (curDistance < followDistance)
                {
                    Navigator.Stop();
                }
                else if (useMesh)
                {
                    await CommonTasks.MoveTo(bc.Location);
                }
                else
                {
                    Navigator.PlayerMover.MoveTowards(bc.Location);
                    await Coroutine.Sleep(msWait);
                }

                curDistance = Core.Me.Distance2D(bc);

                if (curDistance < 1f)
                {
                    moveStopDc = bc.Location;

                    if (await Coroutine.Wait(100, () => bc.Distance2D() < followDistance || bc.Distance2D(moveStopDc) > 0))
                    {
                        if (bc.Distance2D(moveStopDc) > 0)
                        {
                            return false;
                        }

                        Navigator.PlayerMover.MoveStop();
                    }
                }
            }

            return false;
        }
    }
}
