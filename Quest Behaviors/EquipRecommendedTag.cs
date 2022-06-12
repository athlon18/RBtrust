using Clio.XmlEngine;
using System.Threading.Tasks;
using Trust.Windows;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Runs the in-game Equip Recommended feature. Only considers Armory Chest.
    /// </summary>
    [XmlElement("EquipRecommended")]
    public class EquipRecommendedTag : AbstractTaskTag
    {
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            await RecommendEquip.EquipAsync();

            return false;
        }
    }
}
