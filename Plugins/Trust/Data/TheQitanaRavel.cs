using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.Navigation;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Linq;
using TreeSharp;

using Vector3 = Clio.Utilities.Vector3;

namespace Trust
{
    public static class TheQitanaRavel
    {
        public static async Task<bool> Run()
        {

            var plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();

            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
					r.CastingSpellId == 15918 ||
                    r.CastingSpellId == 15916 ||
                    r.CastingSpellId == 15917 ||
                    r.CastingSpellId == 17223 ||
                    r.CastingSpellId == 15498 ||
                    r.CastingSpellId == 15499 ||
                    r.CastingSpellId == 15500 ||
                    r.CastingSpellId == 15725 ||
                    r.CastingSpellId == 15501 ||
                    r.CastingSpellId == 15502 ||
                    r.CastingSpellId == 15503 ||
                    r.CastingSpellId == 15504 ||
                    r.CastingSpellId == 15509 ||
                    r.CastingSpellId == 15510 ||
                    r.CastingSpellId == 15511 ||
                    r.CastingSpellId == 15512 ||
                    r.CastingSpellId == 15926 ||
                    r.CastingSpellId == 17213 ||
                    r.CastingSpellId == 15570 ||
                    r.CastingSpellId == 16263 ||
                    r.CastingSpellId == 14730 ||
                    r.CastingSpellId == 16260 ||
                    r.CastingSpellId == 15514 ||
                    r.CastingSpellId == 15516 ||
                    r.CastingSpellId == 15517 ||
                    r.CastingSpellId == 15518 ||
                    r.CastingSpellId == 15519 ||
                    r.CastingSpellId == 15520 ||
                    r.CastingSpellId == 16923 ||
                    r.CastingSpellId == 15524 ||
                    r.CastingSpellId == 15523 ||
                    r.CastingSpellId == 15527 ||
                    r.CastingSpellId == 15522 ||
                    r.CastingSpellId == 15526 ||
                    r.CastingSpellId == 15521 ||
                    r.CastingSpellId == 15525
                    )
                );

            if (num != null && num.Count() > 0)
            {
                var spell = num.First();
                Logging.Write(Colors.Aquamarine, $"跟随");

                if (spell.NpcId == 8299)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }
                }

                var Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                        (r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                                                                  //r.NpcId == 1492 ||                       // "于里昂热"
                                                                  //r.NpcId == 4130 ||                       // "阿尔菲诺"
                        r.NpcId == 5239 ||                       // "阿莉塞"
                        r.NpcId == 8889 ||                        // 琳   
                        r.Name == "雅·修特拉" ||
                        r.Name == "阿莉塞" ||
						r.Name == "敏菲利亚" ||
                        r.Name == "琳")
                    && r.IsDead == false
                ).OrderBy(r => r.Distance()).First();

                if (Obj.Location.Distance2D(Core.Me.Location) >= 0.3)
                {
                    //读条中断
                    if (Core.Me.IsCasting) ActionManager.StopCasting();

                    Obj.Target();

                    Logging.Write(Colors.Aquamarine, $"队友{Obj.Name}距离:{Obj.Location.Distance2D(Core.Me.Location)}");

                    while (Obj.Location.Distance2D(Core.Me.Location) >= 0.3)
                    {
                        Navigator.PlayerMover.MoveTowards(Obj.Location);
                        await Coroutine.Sleep(100);
                    }
                    Navigator.PlayerMover.MoveStop();
                    await Coroutine.Sleep(100);

                    return true;
                }
            }

			if (Core.Target != null)
            {

                var sC = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8231
                    );
                var sC1 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8232
                    );
                var sC2 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8233
                    );
                // boss 1    
                if (sC.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }
                }
				// boss 2
                if (sC1.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }
                }
		        // boss 3
                if (sC2.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }
				}
			}	
            return false;

        }
    }

}