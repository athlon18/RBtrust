using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Trust
{
    public static class Helpers
    {
        /// 713  :: Thancred                 :: 桑克瑞德
        /// 729  :: Y'shtola                 :: 雅·修特拉
        /// 1492 :: Urianger                 :: 于里昂热
        /// 4130 :: Alphinaud                :: 阿尔菲诺
        /// 5239 :: Alisaie                  :: 阿莉塞
        /// 8378 :: Y'shtola                 :: 雅·修特拉
        /// 8650 :: Crystal Exarch           :: 水晶公
        /// 8889 :: Ryne                     :: 琳
        /// 8917 :: Minfilia                 :: 敏菲利亚
        /// 8919 :: Lyna                     :: 莱楠
        /// <summary>
        /// Gets the nearest Ally to the Player.
        /// </summary>

		public static BattleCharacter GetClosestAlly
        {
            get
            {
#if RB_CN
                HashSet<string> partyMemberNames = new HashSet<string>() { /*"桑克瑞德",*/ "雅·修特拉", "于里昂热", "阿尔菲诺", "阿莉塞", "雅·修特拉", /*"水晶公",*/ "琳", "敏菲利亚", "莱楠" };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberNames.Contains(obj.Name) && !obj.IsDead).OrderBy(r => r.Distance()).FirstOrDefault();
#else
				HashSet<uint> partyMemberIds = new HashSet<uint>() { /*713,*/ 729, 1492, 4130, 5239, 8378, /*8650,*/ 8889, 8917, 8919 };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberIds.Contains(obj.NpcId) && !obj.IsDead).OrderBy(r => r.Distance()).FirstOrDefault();			
#endif	
                
            }
        }
		
        /// <summary>
        /// Gets the furthest Ally from the Player.
        /// </summary>

		public static BattleCharacter GetFurthestAlly
        {
            get
            {
#if RB_CN				
                HashSet<string> partyMemberNames = new HashSet<string>() { /*"桑克瑞德",*/ "雅·修特拉", "于里昂热", "阿尔菲诺", "阿莉塞", "雅·修特拉", /*"水晶公",*/ "琳", "敏菲利亚", "莱楠" };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberNames.Contains(obj.Name) && !obj.IsDead).OrderBy(r => r.Distance()).Reverse().FirstOrDefault();
#else
				HashSet<uint> partyMemberIds = new HashSet<uint>() { /*713,*/ 729, 1492, 4130, 5239, 8378, /*8650,*/ 8889, 8917, 8919 };
				
				return GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj =>
                partyMemberIds.Contains(obj.NpcId) && !obj.IsDead).OrderBy(r => r.Distance()).Reverse().FirstOrDefault();
#endif
            }
        }

        /// 4385, 7864, 8925    :: Brightsphere             :: 光明晶球
        /// 8201                :: The First Beast          :: 第一之兽
        /// 8210                :: Therion                  :: 至大灾兽.
        /// 8260                :: Forgiven Cruelty         :: 得到宽恕的残忍
        /// 8261                :: Forgiven Whimsy          :: 得到宽恕的无常
        /// 8262                :: Forgiven Obscenity       :: 得到宽恕的猥亵
        /// 8270                :: Forgiven Revelry         :: 得到宽恕的放纵
        /// 8299                :: Forgiven Dissonance      :: 得到宽恕的失调
        /// 8300                :: Tesleen, the Forgiven    :: 得到宽恕的泰丝琳
        /// 8301                :: Philia                   :: 斐利亚
        /// 8570                :: Iron Chain               :: 锁链
        public static HashSet<uint> BossIds = new HashSet<uint>
        {
            4385, 7864, 8201, 8210, 8260, 8261, 8262, 8270, 8299, 8300, 8301, 8570, 8925
        };

        private static bool IsFoodItem(this BagSlot slot) =>  (slot.Item.EquipmentCatagory == ItemUiCategory.Meal || slot.Item.EquipmentCatagory == ItemUiCategory.Ingredient);
        public static IEnumerable<BagSlot> GetFoodItems(this IEnumerable<BagSlot> bags) => bags.Where(s => s.IsFoodItem());
        public static bool ContainsFooditem(this IEnumerable<BagSlot> bags, uint id) => bags.Select(s => s.TrueItemId).Contains(id);
        public static BagSlot GetFoodItem(this IEnumerable<BagSlot> bags, uint id) => bags.First(s => s.TrueItemId == id);

        public static bool IsHealer()
        {
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
        }
    }
}
