using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Trust.Data;
using Trust.Extensions;
using Trust.Helpers;

namespace Trust.Dungeons
{
    /// <summary>
    /// Lv. 81 The Tower of Zot dungeon logic.
    /// </summary>
    public class TheTowerOfZot : AbstractDungeon
    {
        /// <summary>
        /// Gets zone ID for this dungeon.
        /// </summary>
        public new const ZoneId ZoneId = Data.ZoneId.TheTowerOfZot;

        private static readonly HashSet<uint> BossIds = new HashSet<uint>
        {
            10256,10257
        };

        /// <inheritdoc/>
        public override DungeonId DungeonId => DungeonId.TheTowerOfZot;

        private readonly static Stopwatch Timer = new Stopwatch();

        private readonly static Stopwatch TargetTimer = new Stopwatch();

        private static DateTime resetTime = DateTime.Now;
        /// <inheritdoc/>
        public override async Task<bool> RunAsync()
        {
            CapabilityManagerHandle TrustHandle = CapabilityManager.CreateNewHandle();

            PluginContainer sidestepPlugin = PluginHelpers.GetSideStepPlugin();

            HashSet<uint> goldChaser = new HashSet<uint>() { 25242, 25371, 25372, 25249, 25253, 25259 };

            HashSet<uint> nfwChaser = new HashSet<uint>() { 25251 , 25248 , 25244 };

            HashSet<uint> stopsidestep = new HashSet<uint>() { 25259 };

            ReceiveMessageHelpers.SkillsdetstrGet(nfwChaser);

            if (WorldManager.SubZoneId != 3733 && WorldManager.SubZoneId != 3734 && WorldManager.SubZoneId != 3735)
            {
                var target = GameObjectManager.Attackers?.OrderByDescending(e => e.CurrentHealthPercent).FirstOrDefault();

                if (target != null && Core.Player.CurrentTarget != target)
                {
                    if (DateTime.Now > resetTime)
                    {
                        resetTime = DateTime.Now.AddSeconds(3);

                        target.Target();
                    }                    
                }
            }


            if (stopsidestep.IsCasting())
            {
                if (sidestepPlugin != null)
                {
                    sidestepPlugin.Enabled = false;
                }
            }

            if (goldChaser.IsCasting())
            {
                Timer.Restart();
            }



            if (nfwChaser.IsCasting())
            {

                CapabilityManager.Clear();
            

                if (sidestepPlugin != null)
                {
                    sidestepPlugin.Enabled = true;
                }


                Timer.Reset();
            }

            if (BossIds.IsFw())
            {
                Timer.Reset();
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 0, "运动会结束，启用近战战斗插件自动走位");
            }

            ReceiveMessageHelpers.SkillsdeterminationOverStr = "三角攻击";

            ReceiveMessageHelpers.VcNPCAcmtsstr = "燃烧吧！";

            if (ReceiveMessageHelpers.VcNPCAcmtsstrStatus)
            {
                if (Timer.ElapsedMilliseconds >= 18000)
                {
                    Timer.Reset();
                    ReceiveMessageHelpers.VcNPCAcmtsstrStatus = false;
                    CapabilityManager.Clear();
                }
            }


            if (Timer.IsRunning && Timer.ElapsedMilliseconds < 20000)
            {
                await MovementHelpers.GetClosestAlly.Follow();

                var mvax = (int)(20000 - Timer.ElapsedMilliseconds > 0 ? 20000 - Timer.ElapsedMilliseconds : 0);


                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, mvax, "运动会近战禁用战斗插件自动走位");


                goto nextdo;
            }
            else
            {
                Timer.Reset();
            }



            HashSet<uint> spells = new HashSet<uint>()
            {
               25233, 25234,  25235 ,25236,  25242, 25699, 25235, 25242, 25371 ,25372
            };

            if (spells.IsCasting())
            {
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 2000, "敌人施法进行中");
                await MovementHelpers.GetClosestAlly.Follow();
            }



            if (WorldManager.SubZoneId == 3733)
            {
                if (sidestepPlugin != null)
                {
                    sidestepPlugin.Enabled = false;
                }
            }
            else
            {
                if (sidestepPlugin != null)
                {
                    sidestepPlugin.Enabled = true;
                }
            }
        nextdo:
            return false;
        }
    }
}
