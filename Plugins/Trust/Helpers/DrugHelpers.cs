using Buddy.Coroutines;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to food.
    /// </summary>
   internal static class DrugHelpers
    {
        public const int DrugBuff = 49;
        
        public static async Task<bool> EatDrug()
        {
            if (Settings.Instance.DrugId == 0 || !InventoryManager.FilledSlots.ContainsDrugItem(Settings.Instance.DrugId))
            {
                return false;
            }

            if (GatheringManager.WindowOpen)
            {
                return false;
            }

            BagSlot item = InventoryManager.FilledSlots.GetDrugItem(Settings.Instance.DrugId);

            if (item == null)
            {
                return false;
            }

            Logging.Write(Colors.Aquamarine, "[吃精炼药] Eating " + item.Name);
            item.UseItem();
            await Coroutine.Wait(5000, () => Core.Player.HasAura(DrugBuff));

            return true;
        }

         /// <summary>
        /// Filters given <see cref="BagSlot"/>s by those containing any drug.
        /// </summary>
        /// <param name="bags">Slots to search for drug.</param>
        /// <returns><see cref="BagSlot"/>s containing drug.</returns>
        public static IEnumerable<BagSlot> GetDrugItems(this IEnumerable<BagSlot> bags)
        {
            return bags.Where(s => s.IsDrugItem());
        }

        /// <summary>
        /// Determines whether any given <see cref="BagSlot"/> contains the specified drug.
        /// </summary>
        /// <param name="bags">Slots to check for drug.</param>
        /// <param name="id">ID of drug to find.</param>
        /// <returns><see langword="true"/> if drug in at least one <see cref="BagSlot"/>.</returns>
        public static bool ContainsDrugItem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags.Select(s => s.TrueItemId).Contains(id);
        }

        /// <summary>
        /// Gets <see cref="BagSlot"/> containing the specified drug.
        /// </summary>
        /// <param name="bags">Slots to check for drug.</param>
        /// <param name="id">ID of drug to find.</param>
        /// <returns><see cref="BagSlot"/> containing specified drug.</returns>
        public static BagSlot GetDrugItem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags.First(s => s.TrueItemId == id);
        }

        /// <summary>
        /// Determines whether item is drug.
        /// </summary>
        /// <param name="slot">Bag slot to inspect.</param>
        /// <returns><see langword="true"/> if drug.</returns>
        private static bool IsDrugItem(this BagSlot slot)
        {
            // TODO: Check ItemAction to see if item actually usable
            return slot.Item.EquipmentCatagory == ItemUiCategory.Medicine && (slot.EnglishName.Contains("Spiritbond"));
        }
    
    }
}
