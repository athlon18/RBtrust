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
            this.foodDropBox = new System.Windows.Forms.ComboBox();
            this.foodLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // foodDropBox
            // 
            this.foodDropBox.FormattingEnabled = true;
            this.foodDropBox.Location = new System.Drawing.Point(83, 10);
            this.foodDropBox.Name = "foodDropBox";
            this.foodDropBox.Size = new System.Drawing.Size(182, 21);
            this.foodDropBox.TabIndex = 0;
            this.foodDropBox.SelectedIndexChanged += new System.EventHandler(this.foodDropBox_SelectedIndexChanged);
            this.foodDropBox.Click += new System.EventHandler(this.foodDropBox_Click);
            // 
            // foodLabel
            // 
            this.foodLabel.AutoSize = true;
            this.foodLabel.Location = new System.Drawing.Point(12, 13);
            this.foodLabel.Name = "foodLabel";
            this.foodLabel.Size = new System.Drawing.Size(64, 13);
            this.foodLabel.TabIndex = 1;
            this.foodLabel.Text = "Select Food";
            // 
            // TrustSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 36);
            this.Controls.Add(this.foodLabel);
            this.Controls.Add(this.foodDropBox);
            this.Name = "TrustSettings";
            this.Text = "TrustSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox foodDropBox;
        private System.Windows.Forms.Label foodLabel;
    }
}