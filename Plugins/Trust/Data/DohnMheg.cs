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
 
            var check = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 && (r.CastingSpellId == 15723 || r.CastingSpellId == 13520 || r.CastingSpellId == 13844));

			var sC3 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8146);  //73BOSS3
					
            //过独木桥     BOSS 3             
                if (check.Any() == true && sC3.First().Location.Distance2D(Core.Me.Location) >= 10)
                {   await Coroutine.Sleep(3000);
                    Logging.Write(Colors.Aquamarine, $"过独木桥");
                    //过桥
                    var Location = new Vector3("-142.8355,-144.5264,-232.6624");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-140.8284,-144.5366,-246.1443");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-130.1889,-144.5366,-242.384");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-114.455,-144.5366,-244.2632");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-125.6857,-144.5238,-249.264");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-122.5055,-144.5192,-258.3726");
                    while (Location.Distance2D(Core.Me.Location) > 0.2)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(30);
                    }
                    Location = new Vector3("-128.1084,-144.5226,-258.0896");
                    while (Location.Distance2D(Core.Me.Location) > 1)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(100);
                    }

                }
			
            // 检测附近 对象是否有特定读条技能
            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    r.CastingSpellId == 15788 ||   //小怪禁园篮筐 圆形
                    //r.CastingSpellId == 15793 ||    //小怪禁园花楸树  圆形
                    //r.CastingSpellId == 15792 ||    //小怪禁园花楸树  扇形
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
				
/*             var sC2 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                r => r.NpcId == 8145 && r.IsVisible == true
                );	//藤蔓
				
            if (Core.Target != sC2.First())
			{
				if (sC2.Any() == true)
				{
					sC2.First().Target();
					return true;
				}
			}
			
            else  */
			
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
					//r.NpcId == 8145||						 // 藤蔓
                    r.NpcId == 729 || r.NpcId == 8378 ||     // "雅·修特拉"
                    r.NpcId == 1492 ||                       // "于里昂热"
                    r.NpcId == 4130 ||                       // "阿尔菲诺"
                    r.NpcId == 5239 ||                       // "阿莉塞"
                    r.NpcId == 8889 ||                        // 琳   
                    r.Name == "雅·修特拉" ||
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
