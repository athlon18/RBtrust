//
// LICENSE:
// This work is licensed under the
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons // 171 Second Street, Suite 300 // San Francisco, California, 94105, USA.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Clio.Utilities;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using TreeSharp;
using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles
{
    [XmlElement("TrustToNPC")]

    public class TrustToNPC : ProfileBehavior
    {
        private bool _done;
		      
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [DefaultValue(3)]
        [XmlAttribute("Distance")]
        public float Distance { get; set; }

        [DefaultValue(true)]
        [XmlAttribute("UseMesh")]
        public bool UseMesh { get; set; }

        [DefaultValue(5000)]
        [XmlAttribute("Wait")]
        public int Wait { get; set; }


        private ushort startmap;
		
		public Vector3 Position;
		
		private bool xyz = false;
		
        protected override void OnStart()
        {
            startmap = WorldManager.ZoneId;
			Position = GameObjectManager.GetObjectByName(Name).Location;
			Log("{0}", Position);
            
        }

        public override bool HighPriority
        {
            get { return true; }
        }

        public override bool IsDone { get { return _done; } }

        private async Task<bool> Destination()
        {
            Navigator.Clear();

            if (!Core.Player.InCombat)
            {
                _done = true;
                return true;
            }

            Log("Reached destination, waiting for {0}ms or until combat drops.", Wait);

            if (await Coroutine.Wait(Wait, () => !Core.Player.InCombat))
            {
                Log("Combat exited, completing early.");
            }

            _done = true;
            return true;
        }
		
        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(

                CommonBehaviors.HandleLoading,
                new Decorator(r => WorldManager.ZoneId != startmap, new Action(r => _done = true)),
                new Decorator(ret => Navigator.InPosition(Core.Player.Location, Position, Distance), new ActionRunCoroutine(r=> Destination())),
                new Decorator(r => UseMesh, CommonBehaviors.MoveAndStop(ret => Position, Distance, stopInRange: true, destinationName: Name)),
                new Decorator(r => !UseMesh, new Action(r =>
              {
                  Core.Player.Face(Position);
                  MovementManager.MoveForwardStart();
              })),
              new ActionAlwaysSucceed()
                );
        }

        /// <summary>
        /// This gets called when a while loop starts over so reset anything that is used inside the IsDone check
        /// </summary>
        protected override void OnResetCachedDone()
        {
            _done = false;
        }

        protected override void OnDone()
        {
            // Force a stop!
            Navigator.PlayerMover.MoveStop();
        }
    }
}
