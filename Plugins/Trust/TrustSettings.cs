using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Trust.Helpers;

namespace Trust
{
    /// <summary>
    /// RB Trust settings window.
    /// </summary>
    public partial class TrustSettings : Form
    {
        private readonly Dictionary<uint, string> foodDict;

        private readonly Dictionary<uint, string> drugDict;

        private readonly ListView listView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrustSettings"/> class.
        /// </summary>
        public TrustSettings()
        {
            foodDict = new Dictionary<uint, string>();
            drugDict = new Dictionary<uint, string>();
            InitializeComponent();
            UpdateFood();
            UpdateDrug();

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

    }
}
