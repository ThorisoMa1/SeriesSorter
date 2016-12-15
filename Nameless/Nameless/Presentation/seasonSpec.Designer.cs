namespace Nameless
{
    partial class seasonSpec
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
            this.txtBoxSeason = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBoxSeason
            // 
            this.txtBoxSeason.FormattingEnabled = true;
            this.txtBoxSeason.Location = new System.Drawing.Point(45, 57);
            this.txtBoxSeason.Name = "txtBoxSeason";
            this.txtBoxSeason.Size = new System.Drawing.Size(121, 21);
            this.txtBoxSeason.TabIndex = 0;
            this.txtBoxSeason.SelectedIndexChanged += new System.EventHandler(this.txtBoxSeason_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Season";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(67, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // seasonSpec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 128);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxSeason);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "seasonSpec";
            this.Text = "seasonSpec";
            this.Load += new System.EventHandler(this.seasonSpec_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox txtBoxSeason;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}