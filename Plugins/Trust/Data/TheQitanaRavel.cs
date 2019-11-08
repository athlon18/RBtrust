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

            var check = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 && (r.CastingSpellId == 15514));

            //蓄力冲撞   连线远离     BOSS 3             
            if (check.Any() == true)
            {
                Logging.Write(Colors.Aquamarine, $"蓄力冲撞");
                //中点
                var Location = new Vector3("31.49962,,-77,-536.7681");
                while (Location.Distance2D(Core.Me.Location) > 1)
                {
                    Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                    Navigator.PlayerMover.MoveTowards(Location);
                    await Coroutine.Sleep(100);
                }

                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(5000);                
            }
			
            // 检测附近 对象是否有特定读条技能
            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    //r.CastingSpellId == 15918 ||   //隆卡之怒 墙壁
                    //r.CastingSpellId == 15916 ||    //隆卡之怒 墙壁
                    //r.CastingSpellId == 15917 ||    //隆卡之怒 墙壁
                    //r.CastingSpellId == 17223 ||   //隆卡之怒 墙壁
                    //r.CastingSpellId == 17387 ||    //隆卡深渊
                    //r.CastingSpellId == 17919 ||    //高热激光   小怪直线
					//r.CastingSpellId == 15923 ||    //高热激光   小怪直线
                    r.CastingSpellId == 15498 ||   //投射石块	    圆形AOE                  1王
                    r.CastingSpellId == 15499 ||   //洛查特尔的骂声  AOE  点亮石柱
                    r.CastingSpellId == 15500 ||    //隆卡之光	 可能是半场石柱
                    r.CastingSpellId == 15725 ||    //水隆卡之光	 可能是半场石柱
                    r.CastingSpellId == 15501 ||    //赤热化	    半身方亮  一左一右
                    r.CastingSpellId == 15502 ||    //赤热化	    半身方亮  一左一右
                    r.CastingSpellId == 15503 ||    //洛查特尔的愤怒	   半场拳一左一右
                    r.CastingSpellId == 15504 ||    //洛查特尔的愤怒	   半场拳一左一右    1王
					//r.CastingSpellId == 15924 ||   //甩头攻击	  蛇  扇形
                    //r.CastingSpellId == 15507 ||   //亚音速   持续AOE                        2王
                    //r.CastingSpellId == 15508 ||    //亚音速   持续AOE
                    r.CastingSpellId == 15509 ||    //落石    不确定
                    r.CastingSpellId == 15510 ||    //落石    不确定
                    r.CastingSpellId == 15511 ||    //大落石   不确定
                    r.CastingSpellId == 15512 ||    //崩塌   可能是石柱倒下                  2王
                    r.CastingSpellId == 15926 ||    //吐出罪孽   随机圆形
					//r.CastingSpellId == 17213 ||   //自爆   不确定
                    //r.CastingSpellId == 15570 ||   //自爆   不确定
                    //r.CastingSpellId == 16263 ||    //自爆   不确定
                    //r.CastingSpellId == 14730 ||    //自爆   不确定
                    //r.CastingSpellId == 16260 ||    //自爆   不确定
                    //r.CastingSpellId == 15514 ||    //蓄力冲撞   连线远离                   3王
                    //r.CastingSpellId == 15515 ||    //灵语   AOE
                    r.CastingSpellId == 15516 ||    //尾蛇毒  毒圈
					r.CastingSpellId == 15517 ||   //尾蛇毒  毒圈
                    r.CastingSpellId == 15518 ||   //尾蛇毒  毒圈
                    r.CastingSpellId == 15519 ||    //跳跃   跳边上 大概  
                    r.CastingSpellId == 15520 ||    //吐息   击退
                    r.CastingSpellId == 16923 ||    //吐息   击退
                    r.CastingSpellId == 15524 ||    //信仰宣言   分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15523 ||    //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15527 ||    //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15522 ||    //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15526 ||    //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15521 ||    //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全  
                    r.CastingSpellId == 15525     //信仰宣言  分摊点名+中间安全  OR 分散点名+两边安全   					
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