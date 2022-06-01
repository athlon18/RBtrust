using ff14bot.Helpers;
using ff14bot.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Trust.Helpers
{
    internal static class ReceiveMessageHelpers
    {
        private static readonly Regex LosesEffectRegex = new Regex("loses the effect of (.*?)", RegexOptions.None);

        public static void ReceiveMessage(object sender, ChatEventArgs args)
        {
            NPCAcmts(args);
            WFNPCAcmts(args);

            if ((int)args.ChatLogEntry.MessageType <= 8774
                || LosesEffectRegex.IsMatch(args.ChatLogEntry.FullLine)
                || args.ChatLogEntry.FullLine.Contains("⇒"))
            {
                return;
            }

            SkillsdeterminationStart(args.ChatLogEntry.FullLine);
            SkillsdeterminationOver(args.ChatLogEntry.FullLine);
            MagnetOver(args.ChatLogEntry.FullLine);
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

        public static string WFNPCAcmtsstr;

        public static bool WFNPCAcmtsstrStatus;

        public static void WFNPCAcmts(ChatEventArgs npcatstr)
        {
            if (npcatstr.ChatLogEntry.MessageType == ff14bot.Enums.MessageType.NPCAnnouncements)
            {
                if (!string.IsNullOrEmpty(WFNPCAcmtsstr))
                {
                    WFNPCAcmtsstrStatus = npcatstr.ChatLogEntry.FullLine.Contains(WFNPCAcmtsstr);
                }
            }
        }

        public static HashSet<string> Skillsdetstr { get; set; } = new HashSet<string>();

        public static void SkillsdetstrGet(HashSet<uint> goldChaser)
        {
            IEnumerable<string> skstr = goldChaser?.Select(r => DataManager.GetSpellData(r).LocalizedName);

            Skillsdetstr = new HashSet<string>(skstr);
        }

        public static bool SkillsdetStatus;

        public static void SkillsdeterminationStart(string sderstr)
        {
            if (sderstr.Contains("readies") ||
                    sderstr.Contains("begins casting"))
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
                    Logging.Write(Colors.OrangeRed, $@"亲信没有添加 判断技能");
                }
            }

            if (sderstr.Contains("casts") ||
                    sderstr.Contains("uses"))
            {
                if (Skillsdetstr != null)
                {
                    if ((bool)Skillsdetstr?.Any(r => sderstr.Contains(r)))
                    {
                        SkillsdetStatus = false;
                    }
                }
            }
        }

        public static HashSet<string> SkillsdeterminationOverStr = new HashSet<string>();

        public static bool SkillsdeterminationOverStatus;

        public static void SkillsdeterminationOver(string sderstr)
        {
            if (sderstr.Contains("uses") ||
                    sderstr.Contains("casts"))
            {
                try
                {
                    if ((bool)SkillsdeterminationOverStr?.Any())
                    {
                        if (SkillsdeterminationOverStr.Any(str => sderstr.Contains(str)))
                        {
                            SkillsdeterminationOverStatus = true;
                        }
                    }
                }
                catch
                {
                    Logging.Write(Colors.OrangeRed, $@"亲信没有添加 判断技能");
                }
            }
        }

        public static HashSet<string> MagnetOverStr = new HashSet<string>();

        public static void MagnetOverStrGet(HashSet<uint> magnet)
        {
            IEnumerable<string> str = magnet?.Select(r => DataManager.GetSpellData(r).LocalizedName);

            MagnetOverStr = new HashSet<string>(str);
        }

        public static bool MagnetOverStatus;

        public static void MagnetOver(string sderstr)
        {
            if (sderstr.Contains("uses") ||
                    sderstr.Contains("casts"))
            {
                try
                {
                    if ((bool)MagnetOverStr?.Any())
                    {
                        MagnetOverStatus = (bool)MagnetOverStr?.Any(r => sderstr.Contains(r));
                    }
                }
                catch
                {
                    Logging.Write(Colors.OrangeRed, $@"亲信没有添加 判断技能");
                }
            }
        }
    }
}
