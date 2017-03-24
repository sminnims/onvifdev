namespace ONVIFTester
{
    partial class SystemLog
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
            this.tbLogBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tbLogBox
            // 
            this.tbLogBox.Location = new System.Drawing.Point(12, 12);
            this.tbLogBox.Name = "tbLogBox";
            this.tbLogBox.Size = new System.Drawing.Size(441, 430);
            this.tbLogBox.TabIndex = 0;
            this.tbLogBox.Text = "";
            // 
            // SystemLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 454);
            this.Controls.Add(this.tbLogBox);
            this.Name = "SystemLog";
            this.Text = "SystemLog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbLogBox;
    }
}