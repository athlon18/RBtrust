using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Trust
{
    public static class Helpers
    {
        private static bool IsFoodItem(this BagSlot slot)
        {
            return (slot.Item.EquipmentCatagory == ItemUiCategory.Meal || slot.Item.EquipmentCatagory == ItemUiCategory.Ingredient);
        }

        public static IEnumerable<BagSlot> GetFoodItems(this IEnumerable<BagSlot> bags)
        {
            return bags
                .Where(s => s.IsFoodItem());
        }

        public static bool ContainsFooditem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags
                .Select(s => s.TrueItemId)
                .Contains(id);
        }

        public static BagSlot GetFoodItem(this IEnumerable<BagSlot> bags, uint id)
        {
            return bags.First(s => s.TrueItemId == id);
        }

        public static bool IsHealer() {
            switch (Core.Me.CurrentJob)
            {
                case ClassJobType.Arcanist:
                case ClassJobType.Astrologian:
                case ClassJobType.Conjurer:
                case ClassJobType.Scholar:
                case ClassJobType.WhiteMage:
                    return true;
                default:
                    return false;
            }
			return false;
        }
    }

}
