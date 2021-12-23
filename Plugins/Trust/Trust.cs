using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using TreeSharp;

namespace Trust
{
    public class Trust : BotPlugin
    {
        private Composite _coroutine;
        private TrustSettings _settingsForm;
        private static readonly uint _buff = 48;

        public override string Author => "athlon";
#if RB_CN
        public override string Name => "亲信战友";
#else
        public override string Name => "Trust";
#endif
        public override Version Version => new Version(1, 2, 0);

        private bool CanTrust()
        {
            return Array.IndexOf(new int[] { 837, 821, 823, 836, 822, 838, 884 }, WorldManager.ZoneId) >= 0;
        }

        public override void OnInitialize()
        {
            if (PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").Any())
            {
                PluginContainer _plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").FirstOrDefault();
                if (_plugin.Enabled == false) { _plugin.Enabled = true; }
            }

            _coroutine = new Decorator(c => CanTrust(), new ActionRunCoroutine(r => RunTrust()));
        }

        public override void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            if (TreeRoot.IsRunning) { AddHooks(); }
        }

        public override void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            RemoveHooks();
        }

        public override void OnShutdown() { OnDisabled(); }

        public override bool WantButton => true;

        public override void OnButtonPress()
        {
            if (_settingsForm == null || _settingsForm.IsDisposed || _settingsForm.Disposing) { _settingsForm = new TrustSettings(); }
            _settingsForm.ShowDialog();
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

        private void OnBotStop(BotBase bot) { RemoveHooks(); }

        private void OnBotStart(BotBase bot) { AddHooks(); }

        private void OnHooksCleared(object sender, EventArgs e) { RemoveHooks(); }

        private static async Task<bool> EatFood()
        {
            if (Settings.Instance.Id == 0 || !InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id)) { return false; }

            if (GatheringManager.WindowOpen) { return false; }

            BagSlot item = InventoryManager.FilledSlots.GetFoodItem(Settings.Instance.Id);

            if (item == null)
            {
                return false;
            }

            Logging.Write(Colors.Aquamarine, "[吃食物] Eating " + item.Name);
            item.UseItem();
            await Coroutine.Wait(5000, () => Core.Player.HasAura(_buff));

            return true;
        }
        private static async Task<bool> PlayerCheck()
        {
            if (Core.Me.CurrentHealthPercent <= 0)
            {
#if RB_CN
                Logging.Write(Colors.Aquamarine, $"检测到死亡");
#else
                Logging.Write(Colors.Aquamarine, $"Player has died.");
#endif
                await Coroutine.Sleep(10000);
                NeoProfileManager.Load(NeoProfileManager.CurrentProfile.Path, true);
                NeoProfileManager.UpdateCurrentProfileBehavior();
                await Coroutine.Sleep(5000);
                return true;
            }

            return false;
        }
        private async Task<bool> RunTrust()
        {
            if (!Core.Me.InCombat && ActionManager.IsSprintReady && MovementManager.IsMoving)
            {
                ActionManager.Sprint();
                await Coroutine.Wait(1000, () => !ActionManager.IsSprintReady);
            }

            if (!Core.Player.HasAura(_buff)) { await EatFood(); }

            switch (WorldManager.ZoneId)
            {
                case 837: //71本 水滩村
                    return await HolminsterSwitch.Run();
                case 821: //73本 多恩美格禁园
                    if (await PlayerCheck())  {  return true; }
                    return await DohnMheg.Run();
                case 823: //75本 奇坦那神影洞
                    if (await PlayerCheck())  {  return true; }
                    return await TheQitanaRavel.Run();
                case 836: //77本 马利卡大井
                    if (await PlayerCheck())  {  return true; }
                    return await MalikahWell.Run();
                case 822: //79本 格鲁格火山
                    if (await PlayerCheck())  {  return true; }
                    return await MtGulg.Run();
                case 838: //80本 亚马乌罗提
                    if (await PlayerCheck())  {  return true; }
                    return await Amaurot.Run();
                case 884: //80本 国际服 5.1
                    if (await PlayerCheck())  {  return true; }
                    return await TheGrandCosmos.Run();
                case 969: //83本 The Tower of Babil
                    if (await PlayerCheck()) { return true; }
                    return await TheTowerofBabil.Run();
                default:
                    return false;
            }
        }
    }

    public class Settings : JsonSettings
    {
        private static Settings _instance;
        public static Settings Instance { get { return _instance ?? (_instance = new Settings()); ; } }

        public Settings() : base(Path.Combine(CharacterSettingsDirectory, "Trust.json")) { }

        [Setting]
        public uint Id { get; set; }
    }
}
