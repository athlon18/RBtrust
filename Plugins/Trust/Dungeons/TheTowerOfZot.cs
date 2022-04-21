using Buddy.Coroutines;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
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

            HashSet<uint> nfwChaser = new HashSet<uint>() { 25251 , 25248 , 25244 , 25278 };

            HashSet<uint> stopsidestep = new HashSet<uint>() { 25259 };

            HashSet<uint> startsidestep = new HashSet<uint>() { 25257 , 25273 };

            HashSet<uint> spells = new HashSet<uint>()
            {
               25233, 25234,  25235 ,25236,  25242, 25699
            };

            HashSet<string> overStr = new HashSet<string>()
            {
               "物创灭"
            };

            ReceiveMessageHelpers.SkillsdeterminationOverStr = overStr ;

            ReceiveMessageHelpers.VcNPCAcmtsstr = "燃烧吧！";

            ReceiveMessageHelpers.SkillsdetstrGet(goldChaser);

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
            if (WorldManager.SubZoneId != 3733)
            {
                if (startsidestep.IsCasting() || nfwChaser.IsCasting() || ReceiveMessageHelpers.SkillsdeterminationOverStatus)
                {
                    if (sidestepPlugin != null && !sidestepPlugin.Enabled)
                    {
                        Logging.Write(Colors.Yellow, $@" 自动躲闪开启 {Timer.ElapsedMilliseconds} >>>>>> >**>");
                        sidestepPlugin.Enabled = true;
                        ReceiveMessageHelpers.SkillsdeterminationOverStatus = false;
                    }
                }
            }



            if (stopsidestep.IsCasting() || goldChaser.IsCasting() || spells.IsCasting())
            {
                if (sidestepPlugin != null && sidestepPlugin.Enabled)
                {
                    Logging.Write(Colors.Yellow, $@" 自动躲闪关闭 {Timer.ElapsedMilliseconds} >>>>>> >**>");
                    sidestepPlugin.Enabled = false;
                }
            }

            if (ReceiveMessageHelpers.SkillsdetStatus)
            {
                Timer.Restart();
                ReceiveMessageHelpers.SkillsdetStatus = false;
            }



            if (nfwChaser.IsCasting())
            {

                CapabilityManager.Clear();

                Timer.Reset();
            }

            if (BossIds.IsFw())
            {
                Timer.Reset();
                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 0, "运动会结束，启用近战战斗插件自动走位");
            }

            

            

            if (ReceiveMessageHelpers.VcNPCAcmtsstrStatus)
            {
                if (Timer.ElapsedMilliseconds >= 21000)
                {
                    Timer.Reset();
                    ReceiveMessageHelpers.VcNPCAcmtsstrStatus = false;
                    CapabilityManager.Clear();
                    Logging.Write(Colors.Yellow, $@" 分散 {Timer.ElapsedMilliseconds} >>>>>> >**>");
                    await MovementHelpers.Spread(3500);
                }
            }


            if (Timer.IsRunning && Timer.ElapsedMilliseconds < 25000)
            {
                var mvax = (int)(25000 - Timer.ElapsedMilliseconds > 0 ? 25000 - Timer.ElapsedMilliseconds : 0);

                CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, mvax, "运动会近战禁用战斗插件自动走位");

                await MovementHelpers.GetClosestAlly.Follow();
                
            }
            else
            {
                Timer.Reset();
            }



    

            if (spells.IsCasting())
            {
                if (!RoutineManager.IsAnyDisallowed(CapabilityFlags.Movement))
                {
                    CapabilityManager.Update(TrustHandle, CapabilityFlags.Movement, 4000, "敌人施法进行中");
                }
                
                await MovementHelpers.GetClosestAlly.Follow();
            }



            if (WorldManager.SubZoneId == 3733)
            {
                if (sidestepPlugin != null && sidestepPlugin.Enabled)
                {
                    sidestepPlugin.Enabled = false;
                }
            }
            else
            {
                if (WorldManager.SubZoneId != 3733 && WorldManager.SubZoneId != 3734 && WorldManager.SubZoneId != 3735)
                {
                    if (sidestepPlugin != null && !sidestepPlugin.Enabled)
                    {
                        sidestepPlugin.Enabled = true;
                    }
                }
  
            }
        
            return false;
        }
    }
}
