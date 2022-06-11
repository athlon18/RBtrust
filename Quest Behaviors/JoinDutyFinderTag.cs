using Buddy.Coroutines;
using Clio.XmlEngine;
using System.Threading.Tasks;
using Trust.Windows;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Accepts pending duty finder commence windows.
    /// </summary>
    [XmlElement("JoinDutyFinder")]
    public class JoinDutyFinderTag : AbstractTaskTag
    {
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            await ExtendedContentsFinder.Open();

            await Coroutine.Yield();

            if (await ExtendedContentsFinder.Join())
            {
                return true;
            }

            Log("Could not join duty finder. Stopping bot.");

            return false;
        }
    }
}
