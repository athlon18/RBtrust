using ff14bot.Managers;
using System.Linq;
using System.Threading.Tasks;

namespace Trust
{
    public static class TheGrandCosmos
    {
        public static Task<bool> Run()
        {
            PluginContainer plugin = PluginManager.Plugins.Where(p => p.Plugin.Name == "SideStep" || p.Plugin.Name == "回避").First();

            return Task.FromResult(false);
        }
    }

}
