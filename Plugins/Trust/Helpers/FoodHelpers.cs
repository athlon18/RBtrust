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
    internal static class FoodHelpers
    {
        /// <summary>
        /// "Well Fed" status effect from food.
        /// </summary>
        public const int FoodBuff = 48;

        /// <summary>
        /// Eats the food item configured in plugin settings.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task<bool> EatFood()
        {
            if (Settings.Instance.FoodId == 0 || !InventoryManager.FilledSlots.ContainsFoodItem(Settings.Instance.FoodId))
            {
                return false;
            }

            if (GatheringManager.WindowOpen)
            {
                return false;
            }

            BagSlot item = InventoryManager.FilledSlots.GetFoodItem(Settings.Instance.FoodId);

            if (item == null)
            {
                return false;
            }

            Logging.Write(Colors.Aquamarine, "[吃食物] Eating " + item.Name);
            item.UseItem();
            await Coroutine.Wait(5000, () => Core.Player.HasAura(FoodBuff));

            return true;
        }

        /// <summary>
        /// Filters given <see cref="BagSlot"/>s by those containing any food.
        /// </summary>
        /// <param name="bags">Slots to search for food.</param>
        /// <returns><see cref="BagSlot"/>s containing food.</returns>
        public static IEnumerable<BagSlot> GetFoodItems(this IEnumerable<BagSlot> bags)
        {
            return bags.Where(s => s.IsFoodItem());
        }

        /// <summary>
        /// Determines whether any given <see cref="BagSlot"/> contains the specified food.
        /// </summary>
        /// <param name="bags">Slots to check for food.</param>
        /// <param name="id">ID of food to find.</param>
        /// <returns><see langword="true"/> if food in at least one <see cref="BagSlot"/>.</returns>
        public static bool ContainsFoodItem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags.Select(s => s.TrueItemId).Contains(id);
        }

        /// <summary>
        /// Gets <see cref="BagSlot"/> containing the specified food.
        /// </summary>
        /// <param name="bags">Slots to check for food.</param>
        /// <param name="id">ID of food to find.</param>
        /// <returns><see cref="BagSlot"/> containing specified food.</returns>
        public static BagSlot GetFoodItem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags.First(s => s.TrueItemId == id);
        }

        /// <summary>
        /// Determines whether item is food.
        /// </summary>
        /// <param name="slot">Bag slot to inspect.</param>
        /// <returns><see langword="true"/> if food.</returns>
        private static bool IsFoodItem(this BagSlot slot)
        {
            // TODO: Check ItemAction to see if item actually usable
            return slot.Item.EquipmentCatagory == ItemUiCategory.Meal || slot.Item.EquipmentCatagory == ItemUiCategory.Ingredient;
        }
    }
}
