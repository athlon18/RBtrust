using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using RBTrust.Plugins.Trust.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to movement.
    /// </summary>
    internal static class MovementHelpers
    {
        /// <summary>
        /// Gets the nearest party member.
        /// </summary>
        public static BattleCharacter GetClosestAlly => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMembers.PartyMemberIds.Contains(obj.NpcId))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the furthest Ally from the Player.
        /// </summary>
        public static BattleCharacter GetFurthestAlly => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMembers.PartyMemberIds.Contains(obj.NpcId))
            .OrderByDescending(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the nearest DPS party member.
        /// </summary>
        public static BattleCharacter GetClosestDps => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMembers.AllPartyMemberIds.Contains(obj.NpcId) && ClassJobRoles.DPS.Contains(obj.CurrentJob))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the nearest Tank party member.
        /// </summary>
        public static BattleCharacter GetClosestTank => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMembers.AllPartyMemberIds.Contains(obj.NpcId) && ClassJobRoles.Tanks.Contains(obj.CurrentJob))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the nearest melee party member.
        /// </summary>
        public static BattleCharacter GetClosestMelee => GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
            .Where(obj => !obj.IsDead && PartyMembers.AllPartyMemberIds.Contains(obj.NpcId) && ClassJobRoles.Melee.Contains(obj.CurrentJob))
            .OrderBy(r => r.Distance())
            .FirstOrDefault();

        /// <summary>
        /// Gets the nearest party member to a point.
        /// </summary>
        /// <param name="location">Point to measure from.</param>
        /// <returns>Nearest party member.</returns>
        public static BattleCharacter GetClosestPartyMember(Vector3 location)
        {
            if (location == null)
            {
                return null;
            }

            return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                .Where(obj => !obj.IsDead && PartyMembers.PartyMemberIds.Contains(obj.NpcId))
                .OrderBy(r => r.Distance(location))
                .FirstOrDefault();
        }

        public static async Task<bool> Spread(double timeToSpread, float spreadDistance = 6.5f, bool isSpreading = false, uint spbc = 0)
        {
            if (isSpreading)
            {
                return true;
            }

            double currentMS = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double endMS = currentMS + timeToSpread;

            if (spbc != 0)
            {
                PartyMembers.AllPartyMemberIds.Add(spbc);
            }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
            {
                foreach (BattleCharacter npc in PartyManager.AllMembers.Select(p => p.BattleCharacter).OrderByDescending(obj => Core.Player.Distance(obj)))
                {
                    AvoidanceManager.AddAvoidObject<BattleCharacter>(
                        () => DateTime.Now.TimeOfDay.TotalMilliseconds <= endMS,
                        radius: spreadDistance,
                        npc.ObjectId);
                }

                await Coroutine.Wait(300, () => AvoidanceManager.IsRunningOutOfAvoid);
            }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
            {
                MovementManager.MoveStop();
            }

            return true;
        }

        public static async Task<bool> HalfSpread(double timeToSpread, float spreadDistance = 6.5f, bool isSpreading = false, uint spbc = 0)
        {
            if (isSpreading)
            {
                return true;
            }

            double currentMS = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double endMS = currentMS + timeToSpread;

            if (spbc != 0)
            {
                BattleCharacter nobj = PartyManager.AllMembers.Select(pm => pm.BattleCharacter).OrderBy(obj => obj.Distance(Core.Player)).FirstOrDefault(obj => !obj.IsMe);

                GameObject st = Core.Player.CurrentTarget;

                LocalPlayer fs = Core.Player;

                Vector3 tl = default;
                if (st != null && fs != null && st.Distance2D(fs) > 0)
                {
                    float k = (st.Z - fs.Z) / (st.X - fs.X);
                    float b = st.Z - (k * st.X);

                    float plg = 100f / fs.DistanceSqr(st.Location);

                    tl.X = fs.X - (plg * (st.X - fs.X));
                    tl.Z = (k * tl.X) + b;
                    tl.Y = st.Y;

                    if (nobj.Distance(tl) - 2f < Core.Player.Distance(tl))
                    {
                        Navigator.PlayerMover.MoveTowards(tl);
                        await Coroutine.Yield();
                        return false;
                    }
                }
            }

            foreach (BattleCharacter npc in GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                .Where(obj => PartyMembers.AllPartyMemberIds.Contains(obj.NpcId)).OrderByDescending(obj => Core.Player.Distance(obj)))
            {
                AvoidanceManager.AddAvoidObject<BattleCharacter>(
                    () => DateTime.Now.TimeOfDay.TotalMilliseconds <= endMS,
                    radius: spreadDistance,
                    npc.ObjectId);

                await Coroutine.Yield();
            }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
            {
                MovementManager.MoveStop();
            }

            return true;
        }

        public static async Task<bool> SpreadSp(double timeToSpread, Vector3 vector, float spreadDistance = 6.5f, bool isSpreading = false, uint spbc = 0)
        {
            if (isSpreading)
            {
                return true;
            }

            double currentMS = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double endMS = currentMS + timeToSpread;

            if (spbc != 0)
            {
                PartyMembers.AllPartyMemberIds.Add(spbc);
            }

            BattleCharacter nobj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                .Where(obj => PartyMembers.AllPartyMemberIds.Contains(obj.NpcId)).OrderBy(obj => obj.Distance(Core.Player)).FirstOrDefault();

            Vector3 playerLoc = Core.Player.Location;

            float ls = 0;
            if (vector != null)
            {
                if (playerLoc.X > vector.X)
                {
                    ls = -20;
                }
                else
                {
                    ls = 20;
                }
            }

            foreach (BattleCharacter npc in GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                .Where(obj => PartyMembers.AllPartyMemberIds.Contains(obj.NpcId))
                .OrderByDescending(r => Core.Player.Distance()))
            {
                AvoidanceManager.AddAvoidObject<BattleCharacter>(
                    () => DateTime.Now.TimeOfDay.TotalMilliseconds <= endMS,
                    () => new Vector3(playerLoc.X - ls, playerLoc.Y, playerLoc.Z),
                    leashRadius: 40,
                    radius: spreadDistance,
                    npc.ObjectId);

                await Coroutine.Yield();
            }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
            {
                MovementManager.MoveStop();
            }

            return true;
        }

        public static async Task<bool> SpreadSpLoc(double timeToSpread, Vector3 vector, float spreadDistance = 6.5f, bool isSpreading = false, uint spbc = 0)
        {
            if (isSpreading)
            {
                return true;
            }

            double currentMS = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double endMS = currentMS + timeToSpread;

            if (spbc != 0)
            {
                PartyMembers.AllPartyMemberIds.Add(spbc);
            }

            if (vector == null)
            {
                vector = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                    .Where(obj => PartyMembers.AllPartyMemberIds.Contains(obj.NpcId))
                    .OrderBy(obj => obj.Distance(Core.Player))
                    .FirstOrDefault()
                    .Location;
            }

            foreach (BattleCharacter npc in GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                .Where(obj => PartyMembers.AllPartyMemberIds.Contains(obj.NpcId))
                .OrderByDescending(r => Core.Player.Distance()))
            {
                AvoidanceManager.AddAvoidObject<BattleCharacter>(
                    () => DateTime.Now.TimeOfDay.TotalMilliseconds <= endMS,
                    () => vector,
                    leashRadius: 40,
                    radius: spreadDistance,
                    npc.ObjectId);

                await Coroutine.Yield();
            }

            if (!AvoidanceManager.IsRunningOutOfAvoid)
            {
                MovementManager.MoveStop();
            }

            return true;
        }
    }
}
