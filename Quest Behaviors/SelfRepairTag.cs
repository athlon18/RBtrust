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
using TreeSharp;

using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles.Tags
 {
    [XmlElement("SelfRepair")]
    public class SelfRepairTag : ProfileBehavior {
   
        [XmlAttribute("Threshhold")]
        public float Threshhold { get; set; }
       
        private bool _done = false;

        private bool RepairAllClicked = false;
        private bool Repaired = false;

        public override bool IsDone { get { return _done; } }

        protected Composite Behavior() {
            return new PrioritySelector(
                // can tag execute?
                new Decorator(ret => !CanRepair(),
                    new Action( r=> {
                        _done = true;
                    })
                ),
				new Decorator(ret => CanRepair(),
					new PrioritySelector(
                        new Decorator(ret => CanRepair() && !Repair.IsOpen,
							new Action(r => {
                                ActionManager.ToggleRepairWindow();
							})
						)	
					)
				),
                // repair
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

        public bool CanRepair() {
            if (Threshhold == null || Threshhold <= 0 || Threshhold > 100)
                Threshhold = 100f;
            return InventoryManager.EquippedItems.Where(r => r.IsFilled && r.Condition < Threshhold).Count() > 0;
        }

        protected override void OnResetCachedDone() {
            RepairAllClicked = false;
            Repaired = false;
			_done = false;
		}

        protected override void OnDone() {
            TreeHooks.Instance.RemoveHook("TreeStart", _cache);
        }

        private Composite _cache;

        protected override void OnStart() {
            _cache = Behavior();
            TreeHooks.Instance.InsertHook("TreeStart", 0, _cache);
        }

    }
}