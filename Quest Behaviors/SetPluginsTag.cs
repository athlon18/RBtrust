using Clio.XmlEngine;
using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    [XmlElement("SetPlugins")]
    public class SetPluginsTag : AbstractTaskTag
    {
        [XmlAttribute("PluginNames")]
        public string PluginNames { get; set; }

        [XmlAttribute("IsEnabled")]
        public bool IsEnabled { get; set; } = true;

        protected override Task<bool> RunAsync()
        {
            IEnumerable<PluginContainer> plugins = PluginManager.Plugins.Where(p => PluginNames.Split(',').Contains(p.Plugin.Name, StringComparer.OrdinalIgnoreCase));

            foreach(PluginContainer p in plugins)
            {
                p.Enabled = IsEnabled;
            }

            return Task.FromResult(false);
        }
    }
}