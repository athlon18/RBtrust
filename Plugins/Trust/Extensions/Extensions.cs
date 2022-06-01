using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Trust.Helpers;

namespace Trust.Extensions
{
    /// <summary>
    /// Various extension methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Checks if any nearby <see cref="BattleCharacter"/> is casting any spell ID in this collection.
        /// </summary>
        /// <param name="spellCastIds">Spell IDs to check against.</param>
        /// <returns><see langword="true"/> if any given spell is being casted.</returns>
        public static bool IsCasting(this HashSet<uint> spellCastIds)
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                    .Any(obj => spellCastIds.Contains(obj.CastingSpellId) && obj.Distance() < 50);
        }

        public static bool IsCastingtwo(this HashSet<uint> spellCastIds)
        {
            IEnumerable<BattleCharacter> actids = GameObjectManager.GetObjectsOfType<BattleCharacter>()
               ?.Where(obj => obj.IsCasting && !(bool)PartyManager.AllMembers?.Any(p => p.ObjectId == obj.ObjectId));

            foreach (BattleCharacter actid in actids)
            {
                Logging.Write(Colors.Yellow, $@" IsCastingtwo 判断显示在使用的 actid ： {actid.CastingSpellId} {actid.SpellCastInfo.IsCasting} {actid.SpellCastInfo.SpellData.LocalizedName} {spellCastIds.Contains(actid.CastingSpellId)}");

                return spellCastIds.Contains(actid.CastingSpellId) && actid.SpellCastInfo.IsCasting;
            }

            return false;
        }

        /// <summary>
        /// Disables SideStep around certain boss-related monsters.
        /// </summary>
        /// <param name="bossIds">Boss monster IDs.</param>
        /// <param name="ignoreIds">IDs to filter out of the base list.</param>
        public static void ToggleSideStep(this HashSet<uint> bossIds, uint[] ignoreIds = null)
        {
            if (Core.Target == null)
            {
                return;
            }

            PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();

            if (sidestepPlugin != null)
            {
                HashSet<uint> filteredIds = new HashSet<uint>(bossIds.Where(id => ignoreIds == null || !ignoreIds.Contains(id)));

                bool isBoss = ignoreIds != null
                ? GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                   .Any(obj => obj.Distance() < 50 && filteredIds.Contains(obj.NpcId))
                : GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false)
                                   .Any(obj => obj.Distance() < 50 && bossIds.Contains(obj.NpcId));

                sidestepPlugin.Enabled = !isBoss;
            }
        }
    }
}
