using ff14bot.Managers;
using System.Linq;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to RebornBuddy plugins.
    /// </summary>
    internal static class PluginHelpers
    {
        /// <summary>
        /// Gets the SideStep plugin reference.
        /// </summary>
        /// <returns>SideStep <see cref="PluginContainer"/>.</returns>
        public static PluginContainer GetSideStepPlugin()
        {
            return PluginManager.Plugins.FirstOrDefault(r => r.Plugin.Name == "SideStep" || r.Plugin.Name == "回避");
        }
    }
}
