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
    public static class DohnMheg
    {
        public static async Task<bool> Run()
        {

            var plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();

            // 检测附近 对象是否有特定读条技能
            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    r.CastingSpellId == 15788 ||   //小怪禁园篮筐 圆形
                    r.CastingSpellId == 15793 ||    //小怪禁园花楸树  圆形
                    r.CastingSpellId == 15792 ||    //小怪禁园花楸树  扇形
                    r.CastingSpellId == 15794 ||   //小怪蜜蜂 针
                    r.CastingSpellId == 15796 ||    //小怪熊 扇形
                    r.CastingSpellId == 8906 ||    //重击 扇形  73 2王
                                                   //r.CastingSpellId == 15798 ||   //小怪凯尔派 圆圈
                    r.CastingSpellId == 15799 ||   //小怪那伊阿得斯 扇形 
                    r.CastingSpellId == 15800 ||   //小怪那伊阿得斯 圆形
                                                   //r.CastingSpellId == 13552 ||    //河童歌唱队	Imp Choir     背对   73 3王
                    r.CastingSpellId == 13551 ||    //青蛙歌唱队	Toad Choir   扇形变形
                                                    //r.CastingSpellId == 13498 ||    //独木桥幻想曲	 读条结束击退出现独木桥
                                                    //r.CastingSpellId == 15723 ||    // 终章	Finale  独木桥狂暴读条
                                                    //r.CastingSpellId == 13520 ||    // 终章	Finale  独木桥狂暴读条
                                                    //r.CastingSpellId == 13844 ||    // 终章	Finale  独木桥狂暴读条
                    r.CastingSpellId == 13514 ||    //水毒浴	 ID有可能错误
                    r.CastingSpellId == 13547 ||    //腐蚀咬     正面范围持续
                    r.CastingSpellId == 13548 ||    //腐蚀咬     正面范围持续
                    r.CastingSpellId == 13952 ||    //触手轰击    十字触手
                    r.CastingSpellId == 13953     //触手轰击   十字触手
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
                    r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                    r.NpcId == 1492 ||                       // "于里昂热"
                    r.NpcId == 4130 ||                       // "阿尔菲诺"
                    r.NpcId == 5239 ||                       // "阿莉塞"
                    r.NpcId == 8889 ||                        // 琳   
                    r.Name == "雅·修特拉" ||
                    r.Name == "于里昂热" ||
                    r.Name == "阿尔菲诺" ||
                    r.Name == "阿莉塞" ||
                    r.Name == "琳"
                ).OrderBy(r => r.Distance()).First();

                //当距离大于跟随距离 再处理跟随
                if (Obj.Location.Distance2D(Core.Me.Location) >= 0.3)
                {
                    // 选中跟随最近的队友
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
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8141
                    );  //73BOSS1

                // 73boss 1 移动 
                if (sC.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }

                    Logging.Write(Colors.Aquamarine, $"boss1");
                    var spellCaster = sC.First();


                    if (spellCaster != null && spellCaster.Name == Core.Target.Name)
                    {
                        var Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                                    r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                                    r.NpcId == 1492 ||                       // "于里昂热"
                                    r.NpcId == 4130 ||                       // "阿尔菲诺"
                                    r.NpcId == 5239 ||                       // "阿莉塞"
                                    r.NpcId == 8889 ||                        // 琳   
                                    r.Name == "雅·修特拉" ||
                                    r.Name == "于里昂热" ||
                                    r.Name == "阿尔菲诺" ||
                                    r.Name == "阿莉塞" ||
                                    r.Name == "琳"
                                     ).OrderBy(r => r.Distance()).First();

                        //当距离大于跟随距离 再处理跟随
                        if (Obj.Location.Distance2D(Core.Me.Location) >= 0.2)
                        {
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
