using Buddy.Coroutines;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Trust
{
    public static class Extensions
    {
        public static bool IsCasting(this HashSet<uint> spellCastIds, bool additional = false)
        {
            if (!additional)
            {
                if (GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj => spellCastIds.Contains(obj.CastingSpellId) && obj.Distance() < 50).Count() > 0)
                {
                    return true;
                }
            }
            else if (GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj => spellCastIds.Contains(obj.CastingSpellId) && additional && obj.Distance() < 50).Count() > 0)
            {
                return true;
            }

            return false;
        }
        
        public static async Task<bool> Follow(this BattleCharacter bc, float followDistance = 0.3f, int msWait = 100, bool useMesh = false)
        {
            float curDistance = Core.Me.Location.Distance(bc.Location);

            if (bc == null) { return true; }

            if (curDistance < followDistance) { return true; }

            while (!Core.Me.IsDead)
            {
                curDistance = Core.Me.Location.Distance(bc.Location);

                if (curDistance < followDistance) { break; }

                if (Core.Me.IsDead) { return false; }

                if (Core.Me.IsCasting) { ActionManager.StopCasting(); }
#if RB_CN
                Logging.Write(Colors.Aquamarine, $"跟随 队友 {bc.Name} [距离: {Core.Me.Distance(bc.Location)}]");
#else
                Logging.Write(Colors.Aquamarine, $"Following {bc.Name} [Distance: {curDistance}]");
#endif 
                if (useMesh) { await CommonTasks.MoveTo(bc.Location); }
                else { Navigator.PlayerMover.MoveTowards(bc.Location); }

                await Coroutine.Sleep(msWait);
            }

            return await StopMoving();
        }

        public static async Task<bool> StopMoving()
        {
            if (!MovementManager.IsMoving) { return true; }

            int ticks = 0;
            while (MovementManager.IsMoving && ticks < 100)
            {
                Navigator.Stop();
                await Coroutine.Sleep(100);
                ticks++;
            }

            return true;
        }

        public static void ToggleSideStep(this HashSet<uint> bossIds, uint[] ignoreIds = null)
        {
            if (Core.Target != null)
            {
                var npcIds = new HashSet<uint>();
                PluginContainer p = PluginManager.Plugins.Where(r => r.Plugin.Name == "SideStep" || r.Plugin.Name == "回避").FirstOrDefault();

                if (ignoreIds != null && ignoreIds.Count() > 0)
                {
                    foreach (var id in bossIds)
                    {
                        if (ignoreIds.Contains(id)) { continue; }
                        npcIds.Add(id);
                    }
                }

                bool isBoss = ignoreIds != null ? GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj => obj.Distance() < 50 &&
                    npcIds.Contains(obj.NpcId)).Any() : GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj => obj.Distance() < 50 &&
                        bossIds.Contains(obj.NpcId)).Any();

                if (isBoss)
                {
                    if (p != null) { if (p.Enabled) { p.Enabled = false; } }
                }
                else
                {
                    if (p != null) { if (!p.Enabled) { p.Enabled = true; } }
                }
            }
        }
    }
}
