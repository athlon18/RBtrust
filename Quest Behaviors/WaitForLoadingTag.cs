using Clio.XmlEngine;
using System.Threading.Tasks;
using Trust.Helpers;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Waits for all loading, cutscenes, and duty to commence.
    /// </summary>
    [XmlElement("WaitForLoading")]
    public class WaitForLoadingTag : AbstractTaskTag
    {
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            await LoadingHelpers.WaitForLoadingAsync();
            await LoadingHelpers.SkipCutsceneAsync();
            await LoadingHelpers.WaitForDutyCommencedAsync();

            return false;
        }
    }
}
