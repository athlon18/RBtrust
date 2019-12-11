using Buddy.Coroutines;
using ff14bot;
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
    public static class TheQitanaRavel
    {
        public static async Task<bool> Run()
        {
            var spellCastIds = new HashSet<uint>()
            {
                15918, 15916, 15917, 17223, 15498, 15499,
                15500, 15725, 15501, 15502, 15503, 15504,
                15509, 15510, 15511, 15512, 15926, 17213,
                15570, 16263, 14730, 16260, 15514, 15516,
                15517, 15518, 15519, 15520, 16923, 15524,
                15523, 15527, 15522, 15526, 15521, 15525
            };

            var spellCast = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                spellCastIds.Contains(obj.CastingSpellId) && obj.Distance() < 50).Count() > 0;

            if (spellCast)
            {
                var closest = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(r =>
                    (r.NpcId == 729 || r.NpcId == 8378 || r.NpcId == 5239 || r.NpcId == 8889 && !r.IsDead)).OrderBy(r => r.Distance()).First();

                if (Core.Me.Distance(closest.Location) >= 0.3)
                {
                    if (Core.Me.IsCasting) ActionManager.StopCasting();
#if RBCN
                    Logging.Write(Colors.Aquamarine, $"跟随 队友 {closest.Name} [距离: {Core.Me.Distance(closest.Location)}]");
#else
                    Logging.Write(Colors.Aquamarine, $"Following {closest.Name} [Distance: {Core.Me.Distance(closest.Location)}]");
#endif
                    while (Core.Me.Distance(closest.Location) >= 0.3)
                    {
                        Navigator.PlayerMover.MoveTowards(closest.Location);
                        await Coroutine.Sleep(100);
                    }

                    Navigator.PlayerMover.MoveStop();
                    await Coroutine.Sleep(100);

                    return true;
                }
            }

            if (Core.Target != null)
            {
                var p = PluginManager.Plugins.Where(r => r.Plugin.Name == "SideStep" || r.Plugin.Name == "回避").First();

                var isBoss = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(r => r.Distance() < 50 &&
                    (r.NpcId == 8231 || r.NpcId == 8232 || r.NpcId == 8233));

                if (isBoss.Any()) { if (p != null) { if (p.Enabled == true) p.Enabled = false; } }
            }

            return false;
        }
    }
}
