using Buddy.Coroutines;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Trust
{
    public static class MalikahWell
    {
        public static async Task<bool> Run()
        {

            PluginContainer plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();

            // 检测附近 对象是否有特定读条技能
            System.Collections.Generic.IEnumerable<BattleCharacter> num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    r.CastingSpellId == 15594 ||
                    r.CastingSpellId == 15590 ||
                    r.CastingSpellId == 15591 ||
                    r.CastingSpellId == 15592 ||
                    r.CastingSpellId == 15593 ||
                    r.CastingSpellId == 15602 ||
                    r.CastingSpellId == 15605 ||
                    r.CastingSpellId == 15606 ||
                    r.CastingSpellId == 15607 ||
                    r.CastingSpellId == 15610 ||
                    r.CastingSpellId == 15609
                    )
                );

            if (num != null && num.Count() > 0)
            {
                BattleCharacter spell = num.First();
                Logging.Write(Colors.Aquamarine, $"跟随");

                if (spell.NpcId == 8299)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true)
                        {
                            plugin.Enabled = false;
                        }
                    }
                }

                BattleCharacter Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                    r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                    r.NpcId == 1492 ||                       // "于里昂热"
                    r.NpcId == 4130 ||                       // "阿尔菲诺"
                    r.NpcId == 5239 ||                       // "阿莉塞"
                    r.NpcId == 8889 ||                        // 琳
                    r.Name == "阿莉塞" ||
                    r.Name == "琳" ||
                    r.Name == "水晶公" ||
                    r.Name == "敏菲利亚" ||
                    r.Name == "桑克瑞德"
                ).OrderBy(r => r.Distance()).First();

                //当距离大于跟随距离 再处理跟随
                if (Obj.Location.Distance2D(Core.Me.Location) >= 0.2)
                {
                    if (Core.Me.IsCasting)
                    {
                        ActionManager.StopCasting();  //断读条
                    }
                    // 选中跟随最近的队友
                    Obj.Target();

                    Logging.Write(Colors.Aquamarine, $"队友{Obj.Name}距离:{Obj.Location.Distance2D(Core.Me.Location)}");

                    while (Obj.Location.Distance2D(Core.Me.Location) >= 0.2)
                    {
                        Navigator.PlayerMover.MoveTowards(Obj.Location);
                        await Coroutine.Sleep(50);
                    }
                    Navigator.PlayerMover.MoveStop();
                    await Coroutine.Sleep(50);

                    return true;
                }
            }

            if (Core.Target != null)
            {

                System.Collections.Generic.IEnumerable<BattleCharacter> sC = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8250
                    );  //77BOSS2

                // /77BOSS2 移动 
                if (sC.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true)
                        {
                            plugin.Enabled = false;
                        }
                    }

                    Logging.Write(Colors.Aquamarine, $"boss2");
                    BattleCharacter spellCaster = sC.First();


                    if (spellCaster != null && spellCaster.Name == Core.Target.Name)
                    {
                        BattleCharacter Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                                    r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                                    r.NpcId == 1492 ||                       // "于里昂热"
                                    r.NpcId == 4130 ||                       // "阿尔菲诺"
                                    r.NpcId == 5239 ||                       // "阿莉塞"
                                    r.NpcId == 8889 ||                        // 琳   
                                    r.Name == "阿莉塞" ||
                                    r.Name == "琳" ||
                                    r.Name == "水晶公" ||
                                    r.Name == "敏菲利亚" ||
                                    r.Name == "桑克瑞德"
                                     ).OrderBy(r => r.Distance()).First();

                        //当距离大于跟随距离 再处理跟随
                        if (Obj.Location.Distance2D(Core.Me.Location) >= 0.2)
                        {
                            if (Core.Me.IsCasting)
                            {
                                ActionManager.StopCasting();  //断读条
                            }
                            // 选中跟随最近的队友
                            Obj.Target();

                            Logging.Write(Colors.Aquamarine, $"队友{Obj.Name}距离:{Obj.Location.Distance2D(Core.Me.Location)}");

                            while (Obj.Location.Distance2D(Core.Me.Location) >= 0.2)
                            {
                                Navigator.PlayerMover.MoveTowards(Obj.Location);
                                await Coroutine.Sleep(50);
                            }
                            Navigator.PlayerMover.MoveStop();
                            await Coroutine.Sleep(50);
                            return true;
                        }

                    }
                }

            }
            return false;

        }
    }

}