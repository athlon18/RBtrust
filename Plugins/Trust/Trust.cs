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
    public class Trust : BotPlugin
    {
        private Composite _coroutine;
        private TrustSettings _settingsForm;
        private static int _buff = 48;   

        public override string Author
        {
            get { return "athlon"; }
        }


#if RB_CN
        public override string Name => "亲信战友";
#else
        public override string Name => "Trust";
#endif

        public override Version Version
        {
            get { return new Version(1, 1, 0); }
        }

        private static async Task<bool> RunTrust()
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
                
				if(Trust.IsHealer() == false){
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
					r.Name == "琳" ||
					r.Name == "桑科瑞德"
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

        public override void OnInitialize()
        {
            var plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();
            if (plugin != null)
            {
                if (plugin.Enabled == false) plugin.Enabled = true;
            }
            _coroutine = new Decorator(c => CanTarget(), new ActionRunCoroutine(r => RunTrust()));
        }

        public bool CanTarget() {
            //疾跑
            if (ActionManager.IsSprintReady && MovementManager.IsMoving && Core.Me.InCombat == false) ActionManager.Sprint();

            return Core.Target != null;
        }

        public static bool IsHealer() {
            switch (Core.Me.CurrentJob)
            {
                case ClassJobType.Arcanist:
                case ClassJobType.Astrologian:
                case ClassJobType.Conjurer:
                case ClassJobType.Scholar:
                case ClassJobType.WhiteMage:
                    return true;
                default:
                    return false;
            }
			return false;
        }

        public override void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            if (TreeRoot.IsRunning)
            {
                AddHooks();
            }
        }

        public override void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            RemoveHooks();
            
        }

        public override void OnShutdown()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            RemoveHooks();
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override void OnButtonPress()
        {
            //if (_settingsForm == null || _settingsForm.IsDisposed || _settingsForm.Disposing)
            //{
            //    _settingsForm = new TrustSettings();
            //}

            //_settingsForm.ShowDialog();
        }

        private void AddHooks()
        {
            Logging.Write(Colors.Aquamarine, "Adding Trust Hook");
            TreeHooks.Instance.AddHook("TreeStart", _coroutine);
        }

        private void RemoveHooks()
        {
            Logging.Write(Colors.Aquamarine, "Removing Trust Hook");
            TreeHooks.Instance.RemoveHook("TreeStart", _coroutine);
        }

        private void OnBotStop(BotBase bot)
        {
            RemoveHooks();
        }

        private void OnBotStart(BotBase bot)
        {
            AddHooks();
        }

        private void OnHooksCleared(object sender, EventArgs e)
        {
            RemoveHooks();
        }
    }

    public class Settings : JsonSettings
    {
        private static Settings _instance;

        public static Settings Instance
        {
            get { return _instance ?? (_instance = new Settings()); ; }
        }

        public Settings()
            : base(Path.Combine(CharacterSettingsDirectory, "Trust.json"))
        {
        }

        [Setting]
        public uint Id { get; set; }
    }
}
