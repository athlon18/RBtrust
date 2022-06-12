using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;
using Trust.Windows;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Loots nearby Treasure Coffers.
    /// </summary>
    [XmlElement("LootTreasure")]
    public class LootTreasureTag : AbstractTaskTag
    {
        private const float InteractRange = 1.5f;
        private const int LootingCooldown = 1500;

        /// <summary>
        /// Gets or sets max search radius for Treasure Coffers.
        /// </summary>
        [XmlAttribute("MaxDistance")]
        public int MaxDistance { get; set; } = 50;

        /// <summary>
        /// Gets or sets a value indicating whether to equip recommended after looting.
        /// </summary>
        [XmlAttribute("EquipRecommended")]
        public bool ShouldEquipRecommended { get; set; } = true;

        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            IOrderedEnumerable<Treasure> nearbyChests = GameObjectManager.GetObjectsOfType<Treasure>()
              .Where(c => !c.IsOpen)
              .Where(c => c.Distance() < MaxDistance)
              .OrderBy(c => c.Distance());

            bool lootedSomething = false;

            foreach (Treasure chest in nearbyChests)
            {
                while (Core.Me.Distance(chest.Location) > InteractRange)
                {
                    await CommonTasks.MoveTo(chest.Location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();

                chest.Interact();
                lootedSomething = true;

                await Coroutine.Sleep(LootingCooldown);
            }

            if (lootedSomething && ShouldEquipRecommended)
            {
                await RecommendEquip.EquipAsync();
            }

            return false;
        }
    }
}
