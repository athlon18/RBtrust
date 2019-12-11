using Buddy.Coroutines;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Trust
{
    public static class MtGulg
    {
        public static async Task<bool> Run()
        {
            #region Spell Filters
            /// 15614, 15615, 15616, 15617, 15618, 17153     :: Typhoon Wing
            /// 15622, 15623, 16987, 16988, 16989            :: Exegesis
            /// 15638, 15640, 15641, 15649, 18025            :: Divine Diminuendo
            /// 15642, 15643, 15648                          :: Conviction Marcato
            /// 15644                                        :: Penance Pianissimo
            /// 15645                                        :: Feather Marionette
            /// 16247, 16248                                 :: Right Palm
            /// 16249, 16250                                 :: Left Palm
            /// 16521                                        :: Glittering Emerald
            /// 16818                                        :: Lumen Infinitum
            HashSet<uint> Spells = new HashSet<uint>()
            {
                15614, 15615, 15616, 15617, 15618, 15622, 15623, 15638,
                15640, 15641, 15642, 15643, 15644, 15645, 15648, 15649,
                16247, 16248, 16249, 16250, 16521, 16818, 16987, 16988,
                16989, 17153, 18025
            };
            #endregion

            #region Custom Mechanics
            /// Forgiven Obscenity (得到宽恕的猥亵)
            /// 15652                                        :: Ringsmith
            /// 15653                                        :: Gold Chaser
            /// 17066                                        :: Solitaire Ring
            HashSet<uint> GoldChaser = new HashSet<uint>() { 15652, 15653, 17066 };
            if (GoldChaser.IsCasting())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 20000)
                {
                    await Helpers.GetClosestAlly.Follow();
                    await Coroutine.Yield();
                }
                sw.Stop();
            }
            #endregion

            /// Default (缺省)
            if (Spells.IsCasting()) { await Helpers.GetClosestAlly.Follow(); }

            /// SideStep (回避)
            if (WorldManager.SubZoneId != 3000) { Helpers.BossIds.ToggleSideStep(new uint[] { 8262 }); } else { Helpers.BossIds.ToggleSideStep(); }

            return false;
        }
    }
}
