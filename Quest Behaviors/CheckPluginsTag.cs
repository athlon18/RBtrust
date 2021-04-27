using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Managers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("CheckPlugins")]
    public class CheckPluginsTag : AbstractTaskTag
    {
        protected override async Task<bool> RunAsync()
        {
            if (PluginManager.Plugins.Any(p => (p.Plugin.Name == "Trust" || p.Plugin.Name == "亲信战友") && p.Enabled))
            {
                string usabilityWarning = "Melee classes may have difficulty with some bosses.";
#if RB_CN
                usabilityWarning = "近战如果打不到BOSS,另一个DPS带琳,添加吃食物,在trust设置里设置食物";
#endif

                Core.OverlayManager.AddToast(() => usabilityWarning,
                    TimeSpan.FromMilliseconds(5000),
                    System.Windows.Media.Color.FromRgb(29, 213, 226),
                    System.Windows.Media.Color.FromRgb(14, 106, 113),
                    new System.Windows.Media.FontFamily("Gautami")
                );

                await Coroutine.Sleep(6000);
            }
            else
            {
                string pluginMissingError = "This profile requires the \"Trust\" plugin to be installed and enabled.  Check your Plugins tab.";
#if RB_CN
                pluginMissingError = "近战如果打不到BOSS,另一个DPS带琳,添加吃食物,在trust设置里设置食物";
#endif

                Core.OverlayManager.AddToast(() => pluginMissingError,
                    TimeSpan.FromMilliseconds(5000),
                    System.Windows.Media.Color.FromRgb(210, 55, 65),
                    System.Windows.Media.Color.FromRgb(105, 27, 32),
                    new System.Windows.Media.FontFamily("Gautami")
                );

                await Coroutine.Sleep(6000);
            }

            return false;
        }
    }
}