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
            get { return new Version(1, 2, 0); }
        }

        private static async Task<bool> RunTrust()
        {
            var mapId = WorldManager.ZoneId;
            switch (mapId)
            {
                case 837: //71本 水滩村
                    return await HolminsterSwitch.Run();
                    break;
                case 821: //73本 多恩美格禁园
                    return await DohnMheg.Run();
                    break;
                case 823: //75本 奇坦那神影洞
                    return await TheQitanaRavel.Run();
                    break;
                case 836: //77本 马利卡大井
                    return await MalikahWell.Run();
                    break;
                case 822: //79本 格鲁格火山
                    return await MtGulg.Run();
                    break;
                case 823: //80本 亚马乌罗提
                    return await Amaurot.Run();
                    break;
                case 884: //80本 国际服 5.1
                    return await TheGrandCosmos.Run();
                    break;
                default:
                    return false;
            }


        }

        public override void OnInitialize()
        {
            var plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避");

            if (plugin.Any() == true)
            {
                var Plugin = plugin.First();
                if (Plugin.Enabled == false) Plugin.Enabled = true;
            }
            _coroutine = new Decorator(c => CanTarget(), new ActionRunCoroutine(r => RunTrust()));
        }

        public bool CanTarget()
        {


            //疾跑
            if (ActionManager.IsSprintReady && MovementManager.IsMoving && Core.Me.InCombat == false) ActionManager.Sprint();

            return Core.Target != null;
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
