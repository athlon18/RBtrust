using ff14bot.Managers;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Trust
{
    public partial class TrustSettings : Form
    {
        private Dictionary<uint, string> foodDict;

        public TrustSettings()
        {
            foodDict = new Dictionary<uint, string>();
            InitializeComponent();

            UpdateFood();

            if (InventoryManager.FilledSlots.ContainsFooditem(Settings.Instance.Id))
            {
                foodDropBox.SelectedValue = Settings.Instance.Id;
            }
        }

        private void foodDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.Id = (uint)foodDropBox.SelectedValue;
            Settings.Instance.Save();
        }

        private void foodDropBox_Click(object sender, EventArgs e)
        {
            UpdateFood();
        }

        private void UpdateFood()
        {
            foodDict.Clear();

            foreach (var item in InventoryManager.FilledSlots.GetFoodItems())
            {
                foodDict[item.TrueItemId] = "(" + item.Count + ")" + item.Name + (item.IsHighQuality ? " HQ" : "");
            }

            foodDropBox.DataSource = new BindingSource(foodDict, null);
            foodDropBox.DisplayMember = "Value";
            foodDropBox.ValueMember = "Key";
        }
    }
}
