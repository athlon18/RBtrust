using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Helpers;
using LlamaLibrary.Extensions;
using LlamaLibrary.Helpers;
using LlamaLibrary.Logging;
using LlamaLibrary.Memory;
using LlamaLibrary.RemoteAgents;
using LlamaLibrary.RemoteWindows;
using LlamaLibrary.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Media;
using Trust;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Runs the in-game Equip Recommended feature. Only considers Armory Chest.
    /// </summary>
    [XmlElement("Extract")]
    public class ExtractTag : AbstractTaskTag
    {
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
           var classId = Trust.Settings.Instance.ClassId;
            Logging.Write(Colors.Aquamarine, "开始[自动精炼] StartAutoExtract ");
            await Inventory.ExtractFromAllGear();
            return false;
        }
    }
}
