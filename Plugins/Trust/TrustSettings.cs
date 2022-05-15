using ff14bot;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Trust.Helpers;
using System.Text;
using ff14bot.Enums;
using ff14bot.RemoteWindows;
using Buddy.Coroutines;
using System.IO;

namespace Trust
{
    /// <summary>
    /// RB Trust settings window.
    /// </summary>
    public partial class TrustSettings : Form
    {
        private readonly Dictionary<uint, string> foodDict;

        private readonly Dictionary<uint, string> drugDict;

        private readonly Dictionary<ClassJobType, string> classJobDict;

        private readonly Dictionary<string, string> dungeonDict;

        private readonly ListView listView;

        private string newProfilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrustSettings"/> class.
        /// </summary>
        public TrustSettings()
        {
            foodDict = new Dictionary<uint, string>();
            drugDict = new Dictionary<uint, string>();
            classJobDict = new Dictionary<ClassJobType, string>();
            dungeonDict = new Dictionary<string, string>();
            InitializeComponent();
            UpdateFood();
            UpdateDrug();
            GetClassJob();
            GetDungeons();
            load();

            if (InventoryManager.FilledSlots.ContainsFoodItem(Settings.Instance.FoodId))
            {
                foodDropBox.SelectedValue = Settings.Instance.FoodId;
            }



            if (InventoryManager.FilledSlots.ContainsDrugItem(Settings.Instance.DrugId))
            {
                drugDropBox.SelectedValue = Settings.Instance.DrugId;
            }
        }

        private void FoodDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.FoodId = (uint)foodDropBox.SelectedValue;
            Settings.Instance.Save();
        }

        private void FoodDropBox_Click(object sender, EventArgs e)
        {
            UpdateFood();
        }

        private void UpdateFood()
        {
            foodDict.Clear();

            foreach (BagSlot item in InventoryManager.FilledSlots.GetFoodItems())
            {
                foodDict[item.TrueItemId] = "(" + item.Count + ")" + item.Name + (item.IsHighQuality ? " HQ" : string.Empty);
            }

            foodDropBox.DataSource = new BindingSource(foodDict, null);
            foodDropBox.DisplayMember = "Value";
            foodDropBox.ValueMember = "Key";
        }

        private void DrugDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.DrugId = (uint)drugDropBox.SelectedValue;
            Settings.Instance.Save();
        }

        private void DrugDropBox_Click(object sender, EventArgs e)
        {
            UpdateDrug();
        }

        private void UpdateDrug()
        {
            drugDict.Clear();

            foreach (var item in InventoryManager.FilledSlots.GetDrugItems())
            {
                drugDict[item.TrueItemId] = "(" + item.Count + ")" + item.Name + (item.IsHighQuality ? " HQ" : string.Empty);
            }

            drugDropBox.DataSource = new BindingSource(drugDict, null);
            drugDropBox.DisplayMember = "Value";
            drugDropBox.ValueMember = "Key";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TrustSettings_Load(object sender, EventArgs e)
        {
            load();

        }

        private void load()
        {
            var name = Settings.Instance.DungeonName;
            var classType = Settings.Instance.ClassId;
            if (name != null && name != string.Empty)
            {
                selectedDungeon.Text = name;
            }

            if (classType.ToString() != null && classType.ToString() != string.Empty)
            {
                var gearSets = GearsetManager.GearSets.Where(i => i.InUse);
                if (gearSets != null)
                {
                    if (gearSets.Any(gs => gs.Class == classType))
                    {
                        byte[] buffer = Encoding.GetEncoding("GBK").GetBytes(gearSets.First(gs => gs.Class == classType).Name);
                        var keyword = Encoding.UTF8.GetString(buffer);
                        selectedClass.Text = keyword;
                    }
                }
            }
        }


        private void btnStartTrust_Click(object sender, EventArgs e)
        {
            var name = Settings.Instance.DungeonName;
            if (name == null) return;
            var ProfileName = "/Plugins/RBtrust/Profiles/" + name;
            newProfilePath = Slashify(ProfileName);
            output.Text = newProfilePath;
            NeoProfileManager.Load(Environment.CurrentDirectory + newProfilePath, true);
            StartBotBase();
        }

        private void ClassDropBox_Click(object sender, EventArgs e)
        {
            GetClassJob();
        }

        private void ClassDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.ClassId = (ClassJobType)classDropBox.SelectedValue;
            Settings.Instance.Save();
            load();
        }

        private void GetClassJob()
        {
            classJobDict.Clear();

            var gearSets = GearsetManager.GearSets.Where(i => i.InUse);
            foreach (var item in gearSets)
            {
                byte[] buffer = Encoding.GetEncoding("GBK").GetBytes(item.Name);
                var keyword = Encoding.UTF8.GetString(buffer);

                classJobDict[item.Class] = "(" + item.Class.ToString() + ")" + keyword.Replace("", string.Empty);
            }

            classDropBox.DataSource = new BindingSource(classJobDict, null);
            classDropBox.DisplayMember = "Value";
            classDropBox.ValueMember = "Key";
        }

        private void GetDungeons()
        {
            dungeonDict.Clear();

            string rootPath = Directory.GetCurrentDirectory() + "/Plugins/RBtrust/Profiles/";
            DirectoryInfo root = new DirectoryInfo(rootPath);
            FileInfo[] files = root.GetFiles();
         
            foreach (var item in files)
            {
                dungeonDict[item.ToString()] = item.ToString();           
            }

            dungeonDropBox.DataSource = new BindingSource(dungeonDict, null);
            dungeonDropBox.DisplayMember = "Value";
            dungeonDropBox.ValueMember = "Key";
        }

        private void DungeonDropBox_Click(object sender, EventArgs e)
        {
            GetDungeons();
        }

        private void DungeonDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.DungeonName = (string)dungeonDropBox.SelectedValue;
            Settings.Instance.Save();
            load();
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

        private static void StartBotBase()
        {
            if (Core.Me.CurrentJob != Settings.Instance.ClassId)
            {
                GearsetManager.GearSets.First(gs => gs.Class == Settings.Instance.ClassId).Activate();

                if (SelectYesno.IsOpen)
                {
                    SelectYesno.Yes();
                }
            }
            BotManager.Current.Start();
            TreeRoot.Start();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
