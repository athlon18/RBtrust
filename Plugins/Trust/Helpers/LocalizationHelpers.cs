using ff14bot.Enums;
using GreyMagic;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to localization and language.
    /// </summary>
    internal static class LocalizationHelpers
    {
        /// <summary>
        /// Gets the current client language.
        /// </summary>
        public static Language ClientLanguage
        {
            get
            {
                using (PatternFinder patternFinder = new PatternFinder(ff14bot.Core.Memory))
                {
                    const string clientLanguagePattern = "Search 48 8D 0D ? ? ? ? E8 ? ? ? ? 48 8D 0D ? ? ? ? E8 ? ? ? ? EB AB Add 3 TraceRelative";
                    return (Language)ff14bot.Core.Memory.Read<byte>(patternFinder.Find(clientLanguagePattern));
                }
            }
        }
    }
}
