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

            //驱魔(分摊)      BOSS 2              
            if (check.Any() == true)
            {
                Logging.Write(Colors.Aquamarine, $"驱魔");
                //中点
                var Location = new Vector3("78.77, 0, -82.18");
                while (Location.Distance2D(Core.Me.Location) > 1)
                {
                    Logging.Write(Colors.Aquamarine, $"中点距离:{Location.Distance2D(Core.Me.Location)}");
                    Navigator.PlayerMover.MoveTowards(Location);
                    await Coroutine.Sleep(100);
                }

                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(7000);

                if (Helpers.IsHealer() == false)
                {
                    //远点
                    Location = new Vector3("84.92282, 0, -97.60876"); // 远点坐标
                    while (Location.Distance2D(Core.Me.Location) > 1)
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


            // 检测附近 对象是否有特定读条技能
            var num = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .Where(r => r.CastingSpellId != 0 && !r.IsMe && r.Distance() < 50 &&
                    (
                    r.CastingSpellId == 15815 ||    //木马  扇形AOE
                    r.CastingSpellId == 15816 ||    //圆形AOE 不让sidestep处理(绞刑笼)
                    r.CastingSpellId == 15814 ||    //螺旋突刺
                    r.CastingSpellId == 15818 ||    //处刑之剑
                    r.CastingSpellId == 15819 ||    //光线射击
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
                    r.NpcId == 713 ||                       	// "桑克瑞德"
                    r.NpcId == 8889 ||                          // 琳   
                    r.Name == "琳" ||
                    r.Name == "桑克瑞德"

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
                // boss 3 移动    
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
                        var Obj = GameObjectManager.GetObjectsOfType<BattleCharacter>(true).Where(r =>
                                    r.NpcId == 8889 ||                          // 琳
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
