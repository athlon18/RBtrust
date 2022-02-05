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

        /// <summary>
        /// Initializes a new instance of the <see cref="TrustSettings"/> class.
        /// </summary>
        public TrustSettings()
        {
            foodDict = new Dictionary<uint, string>();
            InitializeComponent();
            UpdateFood();

            if (InventoryManager.FilledSlots.ContainsFoodItem(Settings.Instance.FoodId))
            {
                foodDropBox.SelectedValue = Settings.Instance.FoodId;
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
    }
}
