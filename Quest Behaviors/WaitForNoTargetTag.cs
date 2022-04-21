using Buddy.Coroutines;
using Clio.Utilities;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.Pathing;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using TreeSharp;
using Trust.Helpers;

namespace ff14bot.NeoProfiles.Tags
{
    /// <summary>
    /// Waits for all loading, cutscenes, and duty to commence.
    /// </summary>
    [XmlElement("WaitForNoTargetTag")]
    public class WaitForNoTargetTag : AbstractTaskTag
    {
        private static DateTime resetTime = DateTime.Now;

        private Vector3 PositionMove;

        private Vector3 PositionDistance;

        private string _npcSay;

        [Clio.XmlEngine.XmlAttribute("MoveXYZ")]
        private Vector3 MoveXYZ
        {
            get { return PositionMove; }
            set { PositionMove = value; }
        }

        [Clio.XmlEngine.XmlAttribute("DistanceXYZ")]
        private Vector3 DistanceXYZ
        {
            get { return PositionDistance; }
            set { PositionDistance = value; }
        }

        [Clio.XmlEngine.XmlAttribute("NpcSay")]
        private string NpcSay
        {
            get { return _npcSay; }
            set { _npcSay = value; }
        }


        public WaitForNoTargetTag()
        {
            _preCombatLogic = new HookExecutor("PreCombatLogic", null, new ActionAlwaysFail());
            _preCombatBuff = new HookExecutor("PreCombatBuff", null, RoutineManager.Current.PreCombatBuffBehavior ?? new ActionAlwaysFail());
            _heal = new HookExecutor("Heal", null, RoutineManager.Current.HealBehavior ?? new ActionAlwaysFail());
            _pull = new HookExecutor("Pull", null, RoutineManager.Current.PullBehavior ?? new ActionAlwaysFail());
            _combatBuff = new HookExecutor("CombatBuff", null, RoutineManager.Current.CombatBuffBehavior ?? new ActionAlwaysFail());
            _combatBehavior = new HookExecutor("Combat", null, RoutineManager.Current.CombatBehavior ?? new ActionAlwaysFail());
            _rest = new HookExecutor("Rest", null, RoutineManager.Current.RestBehavior ?? new ActionAlwaysFail());
        }

        internal Composite _preCombatLogic { get; }
        internal Composite _preCombatBuff { get; }
        internal Composite _heal { get; }
        internal Composite _pull { get; }
        internal Composite _combatBuff { get; }
        internal Composite _combatBehavior { get; }
        internal Composite _rest { get; }

        private readonly object context = new object();
        /// <inheritdoc/>
        protected override async Task<bool> RunAsync()
        {
            //Logging.Write(Colors.Yellow, $@" 自动躲闪开启 {Core.Player.Distance(DistanceXYZ)} >>>>>> >**>");            

            var mt = new MoveToParameters(MoveXYZ);

            mt.DistanceTolerance = 2f;

            ReceiveMessageHelpers.WFNPCAcmtsstr = NpcSay;

            while (!ReceiveMessageHelpers.WFNPCAcmtsstrStatus && WorldManager.SubZoneId == 4129)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    var target = GameObjectManager.Attackers?.OrderByDescending(e => e.CurrentHealthPercent).FirstOrDefault();

                    if (target != null && Core.Player.CurrentTarget != target)
                    {
                        if (DateTime.Now > resetTime)
                        {
                            resetTime = DateTime.Now.AddSeconds(3);

                            Poi.Current = new Poi(target, PoiType.Kill);                            

                            Poi.Current.BattleCharacter.Target();       
                        }
                    }
                    await _preCombatLogic.ExecuteCoroutine(context);

                    await _preCombatBuff.ExecuteCoroutine(context);

                    await _heal.ExecuteCoroutine(context);

                    await _pull.ExecuteCoroutine(context);

                    await _combatBuff.ExecuteCoroutine(context);

                    await _combatBehavior.ExecuteCoroutine(context);

                    await _rest.ExecuteCoroutine(context);
                }
                else
                {
                    if (await CommonTasks.MoveAndStop(mt, 3, true))
                    {
                        //Logging.Write(Colors.Yellow, $@" 自动躲闪开启 {Core.Player.Distance(DistanceXYZ)} >>>>>> >**>");
                        await Coroutine.Wait(1000, () => MovementManager.IsMoving);
                    }
                }

  

                await Coroutine.Yield();
            }

            return await Task.FromResult(false);
        }

        private static bool IsValid(BattleCharacter c)
        {

            //if (!c.InCombat)
            //    return false;

            if (c.IsMe)
                return false;

            if (PartyManager.IsInParty)
            {
                if ((bool)PartyManager.AllMembers?.Any(p => p.ObjectId == c.ObjectId))
                    return false;
            }
            return false;
        }
    }
}
