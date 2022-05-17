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
            this.selectedDungeon = new System.Windows.Forms.Label();
            this.selectedClass = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dungeon = new System.Windows.Forms.Label();
            this.dungeonDropBox = new System.Windows.Forms.ComboBox();
            this.jobLabel = new System.Windows.Forms.Label();
            this.classDropBox = new System.Windows.Forms.ComboBox();
            this.output = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.misc = new System.Windows.Forms.TabPage();
            this.universal = new System.Windows.Forms.GroupBox();
            this.start = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.condition.SuspendLayout();
            this.tab.SuspendLayout();
            this.task.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.misc.SuspendLayout();
            this.universal.SuspendLayout();
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
            this.task.Controls.Add(this.selectedDungeon);
            this.task.Controls.Add(this.selectedClass);
            this.task.Controls.Add(this.label2);
            this.task.Controls.Add(this.label1);
            this.task.Controls.Add(this.dungeon);
            this.task.Controls.Add(this.dungeonDropBox);
            this.task.Controls.Add(this.jobLabel);
            this.task.Controls.Add(this.classDropBox);
            this.task.Controls.Add(this.output);
            resources.ApplyResources(this.task, "task");
            this.task.Name = "task";
            this.task.UseVisualStyleBackColor = true;
            // 
            // selectedDungeon
            // 
            resources.ApplyResources(this.selectedDungeon, "selectedDungeon");
            this.selectedDungeon.Name = "selectedDungeon";
            // 
            // selectedClass
            // 
            resources.ApplyResources(this.selectedClass, "selectedClass");
            this.selectedClass.Name = "selectedClass";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dungeon
            // 
            resources.ApplyResources(this.dungeon, "dungeon");
            this.dungeon.Name = "dungeon";
            this.dungeon.Click += new System.EventHandler(this.label1_Click);
            // 
            // dungeonDropBox
            // 
            this.dungeonDropBox.FormattingEnabled = true;
            resources.ApplyResources(this.dungeonDropBox, "dungeonDropBox");
            this.dungeonDropBox.Name = "dungeonDropBox";
            this.dungeonDropBox.SelectedIndexChanged += new System.EventHandler(this.DungeonDropBox_SelectedIndexChanged);
            this.dungeonDropBox.Click += new System.EventHandler(this.DungeonDropBox_Click);
            // 
            // jobLabel
            // 
            resources.ApplyResources(this.jobLabel, "jobLabel");
            this.jobLabel.Name = "jobLabel";
            // 
            // classDropBox
            // 
            this.classDropBox.FormattingEnabled = true;
            resources.ApplyResources(this.classDropBox, "classDropBox");
            this.classDropBox.Name = "classDropBox";
            this.classDropBox.SelectedIndexChanged += new System.EventHandler(this.ClassDropBox_SelectedIndexChanged);
            this.classDropBox.Click += new System.EventHandler(this.ClassDropBox_Click);
            // 
            // output
            // 
            resources.ApplyResources(this.output, "output");
            this.output.Name = "output";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox2);
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
            // universal
            // 
            this.universal.Controls.Add(this.checkBox3);
            this.universal.Controls.Add(this.checkBox1);
            resources.ApplyResources(this.universal, "universal");
            this.universal.Name = "universal";
            this.universal.TabStop = false;
            // 
            // start
            // 
            resources.ApplyResources(this.start, "start");
            this.start.Name = "start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.BtnStartTrust_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // TrustSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.start);
            this.Controls.Add(this.tab);
            this.Name = "TrustSettings";
            this.Load += new System.EventHandler(this.TrustSettings_Load);
            this.condition.ResumeLayout(false);
            this.condition.PerformLayout();
            this.tab.ResumeLayout(false);
            this.task.ResumeLayout(false);
            this.task.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.misc.ResumeLayout(false);
            this.universal.ResumeLayout(false);
            this.universal.PerformLayout();
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
        private System.Windows.Forms.GroupBox universal;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.Label jobLabel;
        private System.Windows.Forms.ComboBox classDropBox;
        private System.Windows.Forms.Label dungeon;
        private System.Windows.Forms.ComboBox dungeonDropBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label selectedClass;
        private System.Windows.Forms.Label selectedDungeon;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
