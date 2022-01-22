using ff14bot.Directors;
using ff14bot.Managers;

namespace RBTrust.Plugins.Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to dungeons, duties, or other instances.
    /// </summary>
    internal static class InstanceHelpers
    {
        /// <summary>
        /// Gets a value indicating whether in instance, regardless of stage of loading (black screen, barrier, duty commenced, etc).
        /// </summary>
        public static bool IsInInstance => InstanceDirector != null;

        /// <summary>
        /// Gets instanced content-specific Director.  <see langword="null"/> if not inside a relevant instance.
        /// </summary>
        private static InstanceContentDirector InstanceDirector => DirectorManager.ActiveDirector as InstanceContentDirector;
    }
}
