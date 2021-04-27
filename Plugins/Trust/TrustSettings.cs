using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Trust
{
    public partial class TrustSettings : Form
    {
        private readonly Dictionary<uint, string> foodDict;

        public TrustSettings()
        {
            foodDict = new Dictionary<uint, string>();
            InitializeComponent();
            UpdateFood();

            if (InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id)) { foodDropBox.SelectedValue = Settings.Instance.Id; }
        }

        private void FoodDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.Id = (uint)foodDropBox.SelectedValue;
            Settings.Instance.Save();
        }

        private void FoodDropBox_Click(object sender, EventArgs e) { UpdateFood(); }

        private void UpdateFood()
        {
            foodDict.Clear();

            foreach (BagSlot item in InventoryManager.FilledSlots.GetFoodItems())
            {
                foodDict[item.TrueItemId] = "(" + item.Count + ")" + item.Name + (item.IsHighQuality ? " HQ" : "");
            }

            foodDropBox.DataSource = new BindingSource(foodDict, null);
            foodDropBox.DisplayMember = "Value";
            foodDropBox.ValueMember = "Key";
        }
    }
}
