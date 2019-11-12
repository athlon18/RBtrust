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
    public static class HolminsterSwitch
    {
        public static async Task<bool> Run()
        {

            var plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();

            var check = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 && (r.CastingSpellId == 15826 || r.CastingSpellId == 15827));

            var check1 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 && (r.CastingSpellId == 15842 || r.CastingSpellId == 15833 || r.CastingSpellId == 16777 || r.CastingSpellId == 16790));

            //驱魔(分摊)      BOSS 2              
            if (check.Any() == true)
            {
                Logging.Write(Colors.Aquamarine, $"驱魔");

                //中点
                var Location = new Vector3("78.77, 0, -82.18");

                //读条中断
                if (Location.Distance2D(Core.Me.Location) > 1f && Core.Me.IsCasting) ActionManager.StopCasting();

                while (Location.Distance2D(Core.Me.Location) > 1f)
                {
                    Logging.Write(Colors.Aquamarine, $"中点距离:{Location.Distance2D(Core.Me.Location)}");
                    Navigator.PlayerMover.MoveTowards(Location);
                    await Coroutine.Sleep(100);
                }

                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(6000);

                if (Helpers.IsHealer() == false)
                {
                    //远点
                    Location = new Vector3("84.92282, 0, -97.60876"); // 远点坐标

                    //读条中断
                    if (Location.Distance2D(Core.Me.Location) > 1f && Core.Me.IsCasting) ActionManager.StopCasting();

                    while (Location.Distance2D(Core.Me.Location) > 1f)
                    {
                        Logging.Write(Colors.Aquamarine, $"远点距离:{Location.Distance2D(Core.Me.Location)}");
                        Navigator.PlayerMover.MoveTowards(Location);
                        await Coroutine.Sleep(100);
                    }

                    Navigator.PlayerMover.MoveStop();
                    await Coroutine.Sleep(20000);

                    return true;
                }
            }


            if (check1.Any() == true)
            {
                Logging.Write(Colors.Aquamarine, $"TMFS");

                var Location = new Vector3("117.1188,23,-474.0881");

                //读条中断
                if (Location.Distance2D(Core.Me.Location) > 1f && Core.Me.IsCasting) ActionManager.StopCasting();

                while (Location.Distance2D(Core.Me.Location) > 1f)
                {
                    Logging.Write(Colors.Aquamarine, $"分散:{Location.Distance2D(Core.Me.Location)}");
                    Navigator.PlayerMover.MoveTowards(Location);
                    await Coroutine.Sleep(100);
                }

                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(3000);
            }


            // 检测附近 对象是否有特定读条技能
            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    r.CastingSpellId == 15815 ||    //木马  扇形AOE
                    r.CastingSpellId == 15816 ||    //圆形AOE 不让sidestep处理(绞刑笼)
                    r.CastingSpellId == 15814 ||    //螺旋突刺
                    r.CastingSpellId == 15818 ||    //处刑之剑
                    r.CastingSpellId == 15819 ||    //光线射击
                    r.CastingSpellId == 15846 ||    //右鞭打测试
                    r.CastingSpellId == 15847 ||    //左鞭打测试
                    r.CastingSpellId == 15848 ||    //吸取以太测试
                    r.CastingSpellId == 15849 ||    //吸取以太测试
                    r.CastingSpellId == 17232 ||    //埋没之光测试
                    r.CastingSpellId == 15845 ||    //埋没之光测试
                    r.CastingSpellId == 15834 ||    //捶打测试
                    r.CastingSpellId == 15835 ||    //捶打测试
                    r.CastingSpellId == 15836 ||    //捶打测试
                    r.CastingSpellId == 15837 ||    //捶打测试
                    r.CastingSpellId == 15838 ||    //捶打测试
                    r.CastingSpellId == 15839 ||    //捶打测试
                    r.CastingSpellId == 15843 ||    //污浊豪炎测试
                    r.CastingSpellId == 16765 ||    //污浊豪炎测试
                    r.CastingSpellId == 15840 ||    //九尾猫测试
                    r.CastingSpellId == 15841 ||    //九尾猫测试
                    r.CastingSpellId == 16779 ||    //土符文测试
                    r.CastingSpellId == 16780 ||    //土符文测试
                    r.CastingSpellId == 16781 ||    //土符文测试
                    r.CastingSpellId == 16782 ||    //土符文测试
                    r.CastingSpellId == 15822     //异端十字叉
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
                    (r.NpcId == 729 || r.NpcId == 8378 ||       // "雅·修特拉"
                    r.NpcId == 1492 ||                          // "于里昂热"
                    r.NpcId == 4130 ||                          // "阿尔菲诺"
                    r.NpcId == 5239 ||                          // "阿莉塞"
                    r.NpcId == 8889 ||                          // 琳  
                    r.NpcId == 8650 ||                          // 水晶公
                    r.NpcId == 8919 ||                          // 莱楠
                    r.NpcId == 8917 ||                          // 敏菲利亚 
                    r.Name == "桑克瑞德" ||
                    r.Name == "阿莉塞" ||
                    r.Name == "水晶公" ||
                    r.Name == "莱楠" ||
                    r.Name == "敏菲利亚" ||
                    r.Name == "雅·修特拉" ||
                    r.Name == "琳")
                    && r.IsDead == false
                ).OrderBy(r => r.Distance()).First();

                //当距离大于跟随距离 再处理跟随
                if (Obj.Location.Distance2D(Core.Me.Location) >= 0.3f)
                {
                    //读条中断
                    if (Core.Me.IsCasting) ActionManager.StopCasting();

                    // 选中跟随最近的队友
                    Obj.Target();

                    Logging.Write(Colors.Aquamarine, $"队友{Obj.Name}距离:{Obj.Location.Distance2D(Core.Me.Location)}");

                    while (Obj.Location.Distance2D(Core.Me.Location) >= 0.3f)
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
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8301
                    );
                var sC1 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8299
                    );
                var sC2 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8300
                    );
                var sC3 = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(
                    r => !r.IsMe && r.Distance() < 50 && r.NpcId == 8570
                    );	//锁链

                // boss 1    
                if (sC1.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }
                }
                /*// boss 3 移动    
                if (sC.Any() == true)
                {
                    if (plugin != null)
                    {
                        if (plugin.Enabled == true) plugin.Enabled = false;
                    }

                    Logging.Write(Colors.Aquamarine, $"boss3");
                    var spellCaster = sC.First();


                    if (spellCaster != null && spellCaster.Name == Core.Target.Name)
                    {
                        //读条中断
                        if (Core.Me.IsCasting) ActionManager.StopCasting();

                        var Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                                    (r.NpcId == 729 || r.NpcId == 8378 ||        // "雅·修特拉"
                                    r.NpcId == 8889 ||                          // 琳
                                    r.NpcId == 5239 ||                          // "阿莉塞"
                                    r.Name == "阿莉塞" ||
                                    r.Name == "雅·修特拉" ||
                                    r.Name == "琳")
                                    && r.IsDead == false
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
                }*/

            }

            return false;

        }
    }

}
