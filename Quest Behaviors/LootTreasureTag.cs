using Buddy.Coroutines;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Loots nearby Treasure Coffers.
    /// </summary>
    [XmlElement("LootTreasure")]
    public class LootTreasureTag : AbstractTaskTag
    {
        private readonly string[] chestNames = { "宝箱", "Treasure Coffer" };
        private const float InteractRange = 2.5f;

        /// <summary>
        /// Gets or sets max search radius for Treasure Coffers.
        /// </summary>
        [XmlAttribute("MaxDistance")]
        public int MaxDistance { get; set; } = 50;

        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            IOrderedEnumerable<Treasure> nearbyChests = GameObjectManager.GetObjectsOfType<Treasure>()
              .Where(c => !c.IsOpen && c.Distance() < MaxDistance && chestNames.Contains(c.Name, StringComparer.OrdinalIgnoreCase))
              .OrderBy(c => c.Distance());

            foreach (Treasure chest in nearbyChests)
            {
                while (Core.Me.Distance(chest.Location) > InteractRange)
                {
                    await CommonTasks.MoveTo(chest.Location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();

                chest.Interact();
                await Coroutine.Sleep(1000);
            }

            return false;
        }
    }
}
