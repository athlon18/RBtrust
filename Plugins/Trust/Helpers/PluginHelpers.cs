using ff14bot.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trust.Helpers
{
    /// <summary>
    /// Miscellaneous functions related to RebornBuddy plugins.
    /// </summary>
    internal static class PluginHelpers
    {
        /// <summary>
        /// Gets the SideStep plugin reference.
        /// </summary>
        /// <returns>SideStep <see cref="PluginContainer"/>.</returns>
        public static PluginContainer GetSideStepPlugin()
        {
            return PluginManager.Plugins.FirstOrDefault(r => r.Plugin.Name == "SideStep" || r.Plugin.Name == "回避");
        }
    }

    internal static class ReceiveMessageHelpers
    {
        public static void ReceiveMessage(object sender, ChatEventArgs args)
        {
            Regex retr = new Regex("的(.*?)状态效果消失了", RegexOptions.None);

            var npcac = args.ChatLogEntry.MessageType != ff14bot.Enums.MessageType.NPCAnnouncements;

            if (npcac && ((int)args.ChatLogEntry.MessageType <= 8774 ||

                    retr.IsMatch(args.ChatLogEntry.FullLine) ||

                    args.ChatLogEntry.FullLine.Contains("⇒"))) return;

            SkillsdeterminationStart(args.ChatLogEntry.FullLine);

            SkillsdeterminationOver(args.ChatLogEntry.FullLine);

            NPCAcmts(args);

        }
        public static string VcNPCAcmtsstr;

        public static bool VcNPCAcmtsstrStatus;
        public static void NPCAcmts(ChatEventArgs npcatstr)
        {
            if (npcatstr.ChatLogEntry.MessageType == ff14bot.Enums.MessageType.NPCAnnouncements)
            {
                if (VcNPCAcmtsstr != null)
                {
                    VcNPCAcmtsstrStatus = npcatstr.ChatLogEntry.FullLine.Contains(VcNPCAcmtsstr);                  
                }
            }            
        }

        public static HashSet<string>  Skillsdetstr;

        public static void SkillsdetstrGet(HashSet<uint> nfwChaser)
        {
            var skstr = nfwChaser?.Select(r => DataManager.GetSpellData(r).LocalizedName);

            Skillsdetstr = new HashSet<string>(skstr);
        }

        public static bool SkillsdetStatus;
        public static void SkillsdeterminationStart(string sderstr)
        {            
            if (sderstr.Contains("正在发动") ||
                    sderstr.Contains("正在咏唱"))
            {
                try
                {
                    if (Skillsdetstr != null)
                    {
                        SkillsdetStatus = (bool)Skillsdetstr?.Any(r => sderstr.Contains(r));
                    }                    
                }
                catch
                {

                }
                
            }

            if (sderstr.Contains("发动了") ||
                    sderstr.Contains("咏唱了"))
            {
               if (Skillsdetstr != null)
                {
                    if ((bool)Skillsdetstr?.Any(r => sderstr.Contains(r)))
                        SkillsdetStatus = false;
                }               
            }
        }

        public static string SkillsdeterminationOverStr;

        public static bool SkillsdeterminationOverStatus;

        public static void SkillsdeterminationOver(string sderstr)
        {
            if (sderstr.Contains("发动了") ||
                    sderstr.Contains("咏唱了"))
            {
                if (SkillsdeterminationOverStr != null)
                {
                    if (sderstr.Contains(SkillsdeterminationOverStr))
                        SkillsdeterminationOverStatus = true;
                }                
            }
        }
    }
}
