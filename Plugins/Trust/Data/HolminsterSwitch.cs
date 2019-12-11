using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Trust
{
    public class HolminsterSwitch
    {
        public static async Task<bool> Run()
        {
            #region Spell Filters
            /// Spellcast Filter (法术过滤器)                 :: Fetters             :: 脚镣
            HashSet<uint> Fetters = new HashSet<uint>()
            {
                292, 504, 510, 667, 668, 770, 800, 822, 901,
                930, 990, 1010, 1055, 1153, 1258, 1391, 1399,
                1460, 1477, 1497, 1614, 1726, 1757, 1849, 1908
            };
            bool ChainsUp = GameObjectManager.GetObjectsOfType<BattleCharacter>(true, false).Where(obj => Fetters.Any(r => obj.HasAura(r))).Count() > 0;
            /// 15602, 15609                                 :: Heretic's Fork
            /// 15814, 16850                                 :: Thumbscrew
            /// 15815, 16852                                 :: Wooden Horse
            /// 15816, 16851                                 :: Gibbet Cage
            /// 15817, 15820                                 :: Brazen Bull
            /// 15818                                        :: Executioner's Sword
            /// 15819                                        :: Light Shot
            /// 15822, 15886, 17552                          :: Heretic's Fork
            /// 15834, 15835, 15836, 15837, 15838, 15839     :: Fierce Beating
            /// 15840, 15841                                 :: Cat o' Nine Tails
            /// 15843, 16765                                 :: Sickly Inferno
            /// 15845, 17232                                 :: Into the Light
            /// 15846                                        :: Right Knout
            /// 15847                                        :: Left Knout
            /// 15848, 15849                                 :: Aethersup
            /// 16779, 16780, 16781, 16782                   :: Land Rune
            HashSet<uint> Spells = new HashSet<uint>()
            {
                15602, 15609, 15814, 15815, 15816, 15817,
                15818, 15819, 15820, 15822, 15834, 15835,
                15836, 15837, 15838, 15839, 15840, 15841,
                15843, 15845, 15846, 15847, 15848, 15849,
                15886, 16765, 16779, 16780, 16781, 16782,
                16850, 16851, 16852, 17232, 17552
            };
            #endregion

            #region Custom Mechanics
            /// Tesleen, the Forgiven (得到宽恕的泰丝琳)
            /// 15826, 15827                                 :: Exorcise            :: 傩
            HashSet<uint> Exorcise = new HashSet<uint>() { 15826, 15827 };
            if (Exorcise.IsCasting())
            {
                Vector3 _loc = new Vector3("78.77, 0, -82.18");

                if (Core.Me.Distance(_loc) > 1f && Core.Me.IsCasting) { ActionManager.StopCasting(); }

                while (Core.Me.Distance(_loc) > 1f)
                {
                    await CommonTasks.MoveTo(_loc);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();
                await Coroutine.Sleep(6000);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 15000)
                {
                    await Helpers.GetClosestAlly.Follow(7f);
                    await Coroutine.Yield();
                }
                sw.Stop();
            }

            /// Philia (斐利亚)
            /// 15833, 16777, 16790                          :: Pendulum            :: 钟摆
            /// 15842, 16769                                 :: Taphephobia         :: 恐惧症
            HashSet<uint> Pendulum = new HashSet<uint>() { 15833, 15842, 16769, 16777, 16790 };
            if (Pendulum.IsCasting())
            {
                Vector3 _loc = new Vector3("117.1188,23,-474.0881");

                if (Core.Me.Distance(_loc) > 1 && Core.Me.IsCasting) { ActionManager.StopCasting(); }

                while (Core.Me.Distance(_loc) > 1f)
                {
                    await CommonTasks.MoveTo(_loc);
                    await Coroutine.Yield();
                }

                await CommonTasks.StopMoving();
                await Coroutine.Sleep(3000);
            }
            #endregion

            /// Default (缺省)
            if (Spells.IsCasting(!ChainsUp)) { await Helpers.GetClosestAlly.Follow(); }

            /// SideStep (回避)
            Helpers.BossIds.ToggleSideStep();

            return false;
        }
    }
}
