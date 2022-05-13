namespace Trust
{
    partial class TrustSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrustSettings));
            this.foodDropBox = new System.Windows.Forms.ComboBox();
            this.foodLabel = new System.Windows.Forms.Label();
            this.condition = new System.Windows.Forms.GroupBox();
            this.drugLabel = new System.Windows.Forms.Label();
            this.drugDropBox = new System.Windows.Forms.ComboBox();
            this.tab = new System.Windows.Forms.TabControl();
            this.task = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.misc = new System.Windows.Forms.TabPage();
            this.start = new System.Windows.Forms.Button();
            this.universal = new System.Windows.Forms.GroupBox();
            this.maps = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.condition.SuspendLayout();
            this.tab.SuspendLayout();
            this.task.SuspendLayout();
            this.misc.SuspendLayout();
            this.SuspendLayout();
            // 
            // foodDropBox
            // 
            this.foodDropBox.FormattingEnabled = true;
            resources.ApplyResources(this.foodDropBox, "foodDropBox");
            this.foodDropBox.Name = "foodDropBox";
            this.foodDropBox.SelectedIndexChanged += new System.EventHandler(this.FoodDropBox_SelectedIndexChanged);
            this.foodDropBox.Click += new System.EventHandler(this.FoodDropBox_Click);
            // 
            // foodLabel
            // 
            resources.ApplyResources(this.foodLabel, "foodLabel");
            this.foodLabel.Name = "foodLabel";
            // 
            // condition
            // 
            this.condition.Controls.Add(this.drugLabel);
            this.condition.Controls.Add(this.drugDropBox);
            this.condition.Controls.Add(this.foodLabel);
            this.condition.Controls.Add(this.foodDropBox);
            resources.ApplyResources(this.condition, "condition");
            this.condition.Name = "condition";
            this.condition.TabStop = false;
            this.condition.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // drugLabel
            // 
            resources.ApplyResources(this.drugLabel, "drugLabel");
            this.drugLabel.Name = "drugLabel";
            // 
            // drugDropBox
            // 
            this.drugDropBox.FormattingEnabled = true;
            resources.ApplyResources(this.drugDropBox, "drugDropBox");
            this.drugDropBox.Name = "drugDropBox";
            this.drugDropBox.SelectedIndexChanged += new System.EventHandler(this.DrugDropBox_SelectedIndexChanged);
            this.drugDropBox.Click += new System.EventHandler(this.DrugDropBox_Click);
            // 
            // tab
            // 
            this.tab.Controls.Add(this.task);
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.misc);
            resources.ApplyResources(this.tab, "tab");
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            // 
            // task
            // 
            this.task.Controls.Add(this.maps);
            resources.ApplyResources(this.task, "task");
            this.task.Name = "task";
            this.task.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // misc
            // 
            this.misc.Controls.Add(this.universal);
            this.misc.Controls.Add(this.condition);
            resources.ApplyResources(this.misc, "misc");
            this.misc.Name = "misc";
            this.misc.UseVisualStyleBackColor = true;
            // 
            // start
            // 
            resources.ApplyResources(this.start, "start");
            this.start.Name = "start";
            this.start.UseVisualStyleBackColor = true;
            // 
            // universal
            // 
            resources.ApplyResources(this.universal, "universal");
            this.universal.Name = "universal";
            this.universal.TabStop = false;
            // 
            // maps
            // 
            this.maps.CheckBoxes = true;
            this.maps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id});
            this.maps.FullRowSelect = true;
            this.maps.HideSelection = false;
            this.maps.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("maps.Items")))});
            resources.ApplyResources(this.maps, "maps");
            this.maps.MultiSelect = false;
            this.maps.Name = "maps";
            this.maps.UseCompatibleStateImageBehavior = false;
            // 
            // id
            // 
            this.id.Tag = "1111";
            // 
            // TrustSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.start);
            this.Controls.Add(this.tab);
            this.Name = "TrustSettings";
            this.condition.ResumeLayout(false);
            this.condition.PerformLayout();
            this.tab.ResumeLayout(false);
            this.task.ResumeLayout(false);
            this.misc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox foodDropBox;
        private System.Windows.Forms.Label foodLabel;
        private System.Windows.Forms.GroupBox condition;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage task;
        private System.Windows.Forms.TabPage misc;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label drugLabel;
        private System.Windows.Forms.ComboBox drugDropBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView maps;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.GroupBox universal;
    }
}
