using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clio.Utilities;
using Clio.XmlEngine;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.NeoProfiles;
using ff14bot.RemoteWindows;
using TreeSharp;
using Action = TreeSharp.Action;

namespace ff14bot.NeoProfiles
{
    [XmlElement("LoadProfile")]

    public class LoadProfileTag : ProfileBehavior
    {
        private bool _done;

        [XmlAttribute("Path")]
        public string ProfileName { get; set; }

        public override bool IsDone { get { return _done; } }

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => TreeRoot.IsRunning,
                    new Action(r =>
                    {
                        NeoProfileManager.Load(NewProfilePath, true);
                        NeoProfileManager.UpdateCurrentProfileBehavior();
                        _done = true;
                    })
                )
            );
        }

		private string NewProfilePath;
		protected override void OnStart()
        {
			
			var CurrentProfile = NeoProfileManager.CurrentProfile.Path;
            // Support for store profiles.
            // Absolute path to a store profile.
            if (IsStoreProfile(ProfileName))
            {
                NewProfilePath = Slashify(ProfileName);
                return;
            }
			
            // Relative path to a store profile
            if (IsStoreProfile(CurrentProfile))
            {
                NewProfilePath = Slashify(CurrentProfile + "/../" + ProfileName);
                return;
            }

            // Convert path name to absolute, and canonicalize it...
            var absolutePath = Path.Combine(Path.GetDirectoryName(CurrentProfile), ProfileName);
            absolutePath = Path.GetFullPath(absolutePath);
            var canonicalPath = new Uri(absolutePath).LocalPath;
            NewProfilePath = Slashify(canonicalPath);
			
			Log("Changing profile to {0}",ProfileName);
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

        }
		
		
		private bool IsStoreProfile(string path)
        {
            return path.StartsWith("store://");
        }

        // Converts all slashes to back-slashes if path is local; otherwise converts all back-slashes to slashes
        private string Slashify(string path)
        {
            return IsStoreProfile(path) ? path.Replace(@"\", "/") : path.Replace("/", @"\");
        }
    }
}