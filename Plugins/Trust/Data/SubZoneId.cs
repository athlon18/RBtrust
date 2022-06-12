namespace Trust.Data
{
    /// <summary>
    /// Static map of place names to Sub-Zone IDs.
    /// </summary>
    public enum SubZoneId : uint
    {
#pragma warning disable SA1629 // Documentation text should end with a period
        /// <summary>
        /// Sub-Zone ID unavailable.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > Pestilent Sands
        /// </summary>
        PestilentSands = 4107,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > Grebuloff Pillars
        /// </summary>
        GrebuloffPillars = 4108,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > Shell Mound
        /// </summary>
        ShellMound = 4104,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > Judgment Day
        /// </summary>
        JudgmentDay = 4109,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > Deterrence Grounds
        /// </summary>
        DeterrenceGrounds = 4105,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > The Plenty
        /// </summary>
        ThePlenty = 4110,

        /// <summary>
        /// Lv. 90.1: The Dead Ends > The World Tree
        /// </summary>
        TheWorldTree = 4106,
#pragma warning restore SA1629 // Documentation text should end with a period
    }
}
