using System;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Meanings of individual bits in InstanceFlags.
    /// </summary>
    [Flags]
    public enum InstanceFlags : byte
    {
        /// <summary>
        /// Bit set when zone is loaded.
        /// </summary>
        LOADED = 0b0000_0100,

        /// <summary>
        /// Bit set when duty loading barrier is down.
        /// </summary>
        BARRIER_DOWN = 0b0000_1000,

        /// <summary>
        /// Bit set after "duty commenced" message has appeared.
        /// </summary>
        DUTY_COMMENCED = 0b0001_0000,
    }
}
