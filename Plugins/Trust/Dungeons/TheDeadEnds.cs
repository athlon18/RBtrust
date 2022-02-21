using Clio.Utilities;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Trust.Data;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 90.1 The Dead Ends dungeon logic.
    /// </summary>
    public class TheDeadEnds : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheDeadEnds;

        private const int CausticGrebuloff = 10313;
        private const int Peacekeeper = 10315;
        private const int Rala = 10316;

        /// <summary>
        /// Collection of Pestilent Sands environmental traps.
        /// </summary>
        private readonly List<(float Radius, Vector3 Location)> pestilentSandsTraps = new List<(float Radius, Vector3 Location)>()
        {
            (11f, new Vector3(384.6537f, 499.6f, 134.6268f)),
            (7f, new Vector3(393.4057f, 500.5982f, 108.7927f)),
            (7f, new Vector3(383.599f, 500.4645f, 84.56992f)),
            (7f, new Vector3(349.5155f, 499.6613f, 55.23549f)),
            (6f, new Vector3(428.6128f, 499.2374f, 102.9493f)),
            (6f, new Vector3(340.2103f, 499.5605f, 65.34068f)),
            (5f, new Vector3(385.912f, 499.8732f, 60.55911f)),
            (5f, new Vector3(394.0793f, 499.9395f, 69.48797f)),
            (5f, new Vector3(400.2813f, 499.5386f, 78.03534f)),
            (5f, new Vector3(403.9248f, 500.1888f, 91.52982f)),
            (5f, new Vector3(411.6979f, 500.2933f, 122.0323f)),
            (4f, new Vector3(414.9597f, 499.6505f, 89.00001f)),
            (4f, new Vector3(409.712f, 500.2364f, 129.654f)),
            (4f, new Vector3(419.4437f, 499.6318f, 131.0557f)),
            (4f, new Vector3(391.057f, 499.2928f, 163.2473f)),
            (4f, new Vector3(383.4154f, 499.6458f, 160.7675f)),
            (4f, new Vector3(373.9166f, 500.1277f, 152.5391f)),
            (4f, new Vector3(377.9803f, 500.3969f, 74.72343f)),
            (3f, new Vector3(424.1697f, 499.1639f, 90.62242f)),
            (3f, new Vector3(419.0435f, 500.0604f, 97.24624f)),
            (3f, new Vector3(407.203f, 499.472f, 136.9477f)),
            (3f, new Vector3(345.3004f, 499.6697f, 175.295f)),
            (3f, new Vector3(372.0362f, 499.2906f, 169.7142f)),
            (3f, new Vector3(364.4807f, 499.2977f, 186.0001f)),
            (2f, new Vector3(349.9047f, 499.3683f, 188.2401f)),
        };

        /// <summary>
        /// Tracks sub-zone since last tick for environmental decision making.
        /// </summary>
        private SubZoneId lastSubZoneId = SubZoneId.NONE;

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheDeadEnds;

        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            switch (currentSubZoneId)
            {
                // Ostrakon Hexi
                case SubZoneId.PestilentSands:
                    AvoidPestilentSandsTraps();
                    break;
                case SubZoneId.GrebuloffPillars:
                    break;
                case SubZoneId.ShellMound:
                    break;

                // Ostrakon Okto
                case SubZoneId.JudgmentDay:
                    break;
                case SubZoneId.DeterrenceGrounds:
                    HandlePeacekeeperMechanics();
                    break;

                // Ostrakon Deka-hepta
                case SubZoneId.ThePlenty:
                    break;
                case SubZoneId.TheWorldTree:
                    break;
            }

            lastSubZoneId = currentSubZoneId;

            return false;
        }

        private void AvoidPestilentSandsTraps()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            if (lastSubZoneId != currentSubZoneId)
            {
                Logging.Write(Colors.Aquamarine, $"Adding avoids for sub-zone: {SubZoneId.PestilentSands} Pestilent Sands.");

                foreach (var (radius, location) in pestilentSandsTraps)
                {
                    AvoidanceManager.AddAvoidLocation(
                        () => (SubZoneId)WorldManager.SubZoneId == SubZoneId.PestilentSands,  // Call WorldManager directly
                        radius: radius,
                        () => location,
                        ignoreIfBlocking: true);
                }
            }
        }

        private void HandlePeacekeeperMechanics()
        {
            SubZoneId currentSubZoneId = (SubZoneId)WorldManager.SubZoneId;

            if (lastSubZoneId != currentSubZoneId)
            {
                var boss = GameObjectManager.GetObjectByNPCId<BattleCharacter>(Peacekeeper);

                if (boss != null)
                {
                    Logging.Write(Colors.Aquamarine, $"Adding avoid for {boss.Name} (NpcId:{boss.NpcId}, ObjectId:{boss.ObjectId}).");

                    AvoidanceManager.AddAvoidObject<BattleCharacter>(
                        () => boss.IsValid && !boss.IsDead,
                        radius: 9.5f,
                        boss.ObjectId);
                }
            }
        }
    }
}
