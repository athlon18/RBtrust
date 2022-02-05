using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using RBTrust.Plugins.Trust.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Media;
using TreeSharp;
using Trust.Dungeons;
using Trust.Helpers;

namespace Trust
{
    /// <summary>
    /// Main RebornBuddy plugin class for RB Trust.
    /// </summary>
    public class Trust : BotPlugin
    {
        private Composite root;
        private TrustSettings settingsForm;
        private DungeonManager dungeonManager;

        /// <inheritdoc/>
        public override string Author => "athlon";
#if RB_CN
        /// <inheritdoc/>
        public override string Name => "亲信战友";
#else
        /// <inheritdoc/>
        public override string Name => "Trust";
#endif

        /// <inheritdoc/>
        public override Version Version => new Version(1, 2, 0);

        /// <inheritdoc/>
        public override bool WantButton => true;

        /// <inheritdoc/>
        public override void OnInitialize()
        {
            PluginContainer plugin = PluginHelpers.GetSideStepPlugin();
            if (plugin != null)
            {
                plugin.Enabled = true;
            }

            root = new Decorator(c => CanTrust(), new ActionRunCoroutine(r => RunTrust()));
        }

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            TreeRoot.OnStart += OnBotStart;
            TreeRoot.OnStop += OnBotStop;
            TreeHooks.Instance.OnHooksCleared += OnHooksCleared;

            if (TreeRoot.IsRunning)
            {
                AddHooks();
            }

            dungeonManager = new DungeonManager();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            TreeRoot.OnStart -= OnBotStart;
            TreeRoot.OnStop -= OnBotStop;
            RemoveHooks();
        }

        /// <inheritdoc/>
        public override void OnShutdown()
        {
            OnDisabled();
        }

        /// <inheritdoc/>
        public override void OnButtonPress()
        {
            if (settingsForm == null || settingsForm.IsDisposed || settingsForm.Disposing)
            {
                settingsForm = new TrustSettings();
            }

            settingsForm.ShowDialog();
        }

        private void AddHooks()
        {
            Logging.Write(Colors.Aquamarine, "Adding Trust Hook");
            TreeHooks.Instance.AddHook("TreeStart", root);
        }

        private void RemoveHooks()
        {
            Logging.Write(Colors.Aquamarine, "Removing Trust Hook");
            TreeHooks.Instance.RemoveHook("TreeStart", root);
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

        private bool CanTrust()
        {
            return InstanceHelpers.IsInInstance;
        }

        private async Task<bool> RunTrust()
        {
            if (!Core.Me.InCombat && ActionManager.IsSprintReady && MovementManager.IsMoving)
            {
                ActionManager.Sprint();
                await Coroutine.Wait(1000, () => !ActionManager.IsSprintReady);
            }

            if (!Core.Player.HasAura(FoodHelpers.FoodBuff))
            {
                await FoodHelpers.EatFood();
            }

            if (await PlayerCheck())
            {
                return true;
            }

            return await dungeonManager.RunAsync();
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
    }
}
