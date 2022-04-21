using ff14bot.Helpers;
using System.Configuration;
using System.IO;

namespace Trust
{
    /// <summary>
    /// Representation for deserialized RBTrust settings.
    /// </summary>
    public class Settings : JsonSettings
    {
        private static Settings instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
            : base(Path.Combine(CharacterSettingsDirectory, "Trust.json"))
        {
        }

        /// <summary>
        /// Gets global reference to <see cref="Settings"/> singleton.
        /// </summary>
        public static Settings Instance => instance ?? (instance = new Settings());

        /// <summary>
        /// Gets or sets ID of food item to eat.
        /// </summary>
        [Setting]
        public uint FoodId { get; set; }
        public uint DrugId { get; set; }
    }
}
