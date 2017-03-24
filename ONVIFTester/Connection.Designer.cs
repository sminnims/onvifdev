namespace ONVIFTester
{
    partial class Connection
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGetLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 89);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(423, 275);
            this.tbLog.TabIndex = 0;
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(328, 17);
            this.tbID.MaxLength = 12;
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(107, 21);
            this.tbID.TabIndex = 1;
            this.tbID.Text = "root0";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(328, 44);
            this.tbPass.MaxLength = 12;
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(107, 21);
            this.tbPass.TabIndex = 2;
            this.tbPass.Text = "password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "http://";
            // 
            // tbAddr
            // 
            this.tbAddr.Location = new System.Drawing.Point(51, 17);
            this.tbAddr.Name = "tbAddr";
            this.tbAddr.Size = new System.Drawing.Size(182, 21);
            this.tbAddr.TabIndex = 5;
            this.tbAddr.Text = "10.1.31.211";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "ID :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password :";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(493, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnGetLog
            // 
            this.btnGetLog.Location = new System.Drawing.Point(493, 47);
            this.btnGetLog.Name = "btnGetLog";
            this.btnGetLog.Size = new System.Drawing.Size(75, 23);
            this.btnGetLog.TabIndex = 9;
            this.btnGetLog.Text = "Get Log";
            this.btnGetLog.UseVisualStyleBackColor = true;
            this.btnGetLog.Visible = false;
            this.btnGetLog.Click += new System.EventHandler(this.btnGetLog_Click);
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 376);
            this.Controls.Add(this.btnGetLog);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbAddr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.tbLog);
            this.Name = "Connection";
            this.Text = "Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnGetLog;
    }
}

