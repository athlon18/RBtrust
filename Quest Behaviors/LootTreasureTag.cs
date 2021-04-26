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
    [XmlElement("LootTreasure")]
    public class LootTreasureTag : AbstractTaskTag
    {
        [XmlAttribute("MaxDistance")]
        public int MaxDistance { get; set; } = 50;

        private readonly string[] _chestNames = { "宝箱", "Treasure Coffer" };

        protected override async Task<bool> RunAsync()
        {
            IOrderedEnumerable<Treasure> nearbyChests = GameObjectManager.GetObjectsOfType<Treasure>()
                                          .Where(c => !c.IsOpen && c.Distance() < MaxDistance && _chestNames.Contains(c.Name, StringComparer.OrdinalIgnoreCase))
                                          .OrderBy(c => c.Distance());

            foreach(Treasure chest in nearbyChests)
            {
                while (Core.Me.Distance(chest.Location) > 1f)
                {
                    await CommonTasks.MoveTo(chest.Location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();

                while(!chest.IsOpen)
                {
                    chest.Interact();
                    await Coroutine.Sleep(1000);
                }
            }

            return false;
        }
    }
}