using Buddy.Coroutines;
using Clio.Utilities;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;
using System.Linq;
using System.Threading.Tasks;
using ff14bot.Navigation;
using TreeSharp;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;
using ff14bot.Enums;

using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles.Tags
 {
    [XmlElement("NPCRepair")]
    public class NPCRepairTag : ProfileBehavior {
		
        [XmlAttribute("NpcId")]
        public uint NpcId { get; set; }

        [XmlAttribute("XYZ")]
        public Vector3 XYZ { get; set; }

		[XmlAttribute("ZoneId")]
        public uint ZoneId { get; set; }
		
        [XmlAttribute("DialogOption")]
        public uint DialogOption { get; set; }

        [XmlAttribute("Threshhold")]
        public float Threshhold { get; set; }
        
        private GameObject NPC;

        private bool _done = false;

        private bool RepairAllClicked = false;
        private bool Repaired = false;

		private bool _generatedNodes = false;
		
		public Queue<NavGraph.INode> FinalizedPath;
		 
        public override bool IsDone { get { return _done; } }

        protected Composite Behavior() {
            return new PrioritySelector(
                // can tag execute?
                new Decorator(ret => !CanRepair(),
                    new Action( r=> {
                        _done = true;
                    })
                ),
                new Decorator(ret => CraftingLog.IsOpen,
                    new ActionRunCoroutine(ctx => StopCrafting())
                ),
				new Decorator(r => !_generatedNodes, new ActionRunCoroutine(r => GenerateNodes())),
                // stage 1: Find the NPC
                new Decorator(ret => Poi.Current.Type == PoiType.None,
                    new PrioritySelector(
                        new Decorator(ret => GameObjectManager.GetObjectByNPCId(NpcId) != null,
                            new Action(r => {
                                NPC = GameObjectManager.GetObjectByNPCId(NpcId);
                                Poi.Current = new Poi(NPC, PoiType.Vendor);
                            })
                        ),
                        new Decorator(ret => XYZ != null && Core.Player.Location.Distance(XYZ) > 20f,                
							NavGraph.NavGraphConsumer(r => FinalizedPath)
						),
                        new Decorator(ret => NPC == null,
                            new Action(r => {
                                _done = true;
                            })
                        )
                    )
                ),
                // stage 2: go to NPC and Interact
                new Decorator(ret => Poi.Current.Type == PoiType.Vendor && !Repair.IsOpen,
                    new PrioritySelector(
                        new Decorator(ret => SelectIconString.IsOpen,
                            new Action(r => {
                                SelectIconString.ClickSlot(DialogOption);
                            })
                        ),
                        new Decorator(ret => Core.Player.Location.Distance(Poi.Current.Location) > 3f,
                            CommonBehaviors.MoveAndStop(ret => Poi.Current.Location, 2f, true, null, RunStatus.Failure)
                        ),
                        new Decorator(ret => Poi.Current.Type == PoiType.Vendor,
                            new Action(r => {
                                Poi.Current.Unit.Interact();
                            })
                        )
                )),
                // stage 3: repair
                new Decorator(ret => Repair.IsOpen,
                    new PrioritySelector(
                        new Decorator(ret => Repaired,
                            new Action(r => {
                                Repair.Close();
                                _done = true;
                            })
                        ),
                        new Decorator(ret => Repair.IsOpen && ! RepairAllClicked,
                            new Action(r => {
                                RepairAllClicked = true;
                                Repair.RepairAll();
                            })
                        ),
                        new Decorator(ret => SelectYesno.IsOpen,
                            new Action(r => {
                                SelectYesno.ClickYes();
                                Repaired = true;
                            })
                        )
                ))
            );
        }

        public async Task<bool> StopCrafting() {
            CraftingLog.Close();

            await Coroutine.Sleep(5000);

            return true;
        }

        public bool CanRepair() {
            if (Threshhold == null || Threshhold <= 0 || Threshhold > 100)
                Threshhold = 100f;

            return InventoryManager.EquippedItems.Where(r => r.IsFilled && r.Condition < Threshhold).Count() > 0;
        }

        protected override void OnResetCachedDone() {
            RepairAllClicked = false;
            Repaired = false;
            _done = false;
			_generatedNodes = false;
            FinalizedPath = null;
        }

        protected override void OnDone() {
            TreeHooks.Instance.RemoveHook("TreeStart", _cache);
        }

        private Composite _cache;

        protected override void OnStart() {
            _cache = Behavior();
            TreeHooks.Instance.InsertHook("TreeStart", 0, _cache);
        }
		
		private async Task<bool> GenerateNodes()
        {
            var path = await NavGraph.GetPathAsync((uint)ZoneId, XYZ);
            if (path == null)
            {
                LogError($"Couldn't get a path to {XYZ} on {ZoneId}, Stopping.");
                return true;
            }
            _generatedNodes = true;
            FinalizedPath = path;
            return true;
        }
    }
}