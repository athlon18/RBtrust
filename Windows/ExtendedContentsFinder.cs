using Buddy.Coroutines;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Convenience wrapper for Duty Finder and Queue windows.
    /// </summary>
    public class ExtendedContentsFinder
    {
        /// <summary>
        /// Gets a value indicating whether Duty Finder is open.
        /// </summary>
        public static bool IsOpen => RaptureAtkUnitManager.GetWindowByName("ContentsFinder") != null;

        /// <summary>
        /// Opens the Duty Finder window.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task<bool> Open()
        {
            while (!IsOpen)
            {
                ChatManager.SendChat("/dutyfinder");
                await Coroutine.Wait(3000, () => IsOpen);
            }

            return true;
        }

        /// <summary>
        /// Accepts Duty Finder queue if queue has popped.
        /// </summary>
        /// <returns><see langword="true"/> if this behavior expected/handled execution.</returns>
        public static async Task<bool> Join()
        {
            if (!IsOpen)
            {
                return false;
            }

            RaptureAtkUnitManager.GetWindowByName("ContentsFinder").SendAction(1, 1, 3);
            if (await Coroutine.Wait(5000, () => !IsOpen))
            {
                return true;
            }

            return false;
        }
    }
}
