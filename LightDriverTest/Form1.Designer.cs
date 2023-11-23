namespace LightDriverTest
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtWrite = new System.Windows.Forms.TextBox();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.btnLightTerminate = new System.Windows.Forms.Button();
            this.btnLightOn = new System.Windows.Forms.Button();
            this.nudLightValuePercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightValuePercent)).BeginInit();
            this.SuspendLayout();
            // 
            // txtWrite
            // 
            this.txtWrite.Location = new System.Drawing.Point(29, 22);
            this.txtWrite.Name = "txtWrite";
            this.txtWrite.Size = new System.Drawing.Size(262, 19);
            this.txtWrite.TabIndex = 0;
            // 
            // txtReceive
            // 
            this.txtReceive.Location = new System.Drawing.Point(29, 47);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.Size = new System.Drawing.Size(262, 44);
            this.txtReceive.TabIndex = 1;
            // 
            // btnRequest
            // 
            this.btnRequest.Location = new System.Drawing.Point(314, 22);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(75, 23);
            this.btnRequest.TabIndex = 2;
            this.btnRequest.Text = "リクエスト";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click_1);
            // 
            // btnLightTerminate
            // 
            this.btnLightTerminate.Location = new System.Drawing.Point(29, 115);
            this.btnLightTerminate.Name = "btnLightTerminate";
            this.btnLightTerminate.Size = new System.Drawing.Size(75, 23);
            this.btnLightTerminate.TabIndex = 3;
            this.btnLightTerminate.Text = "全照明終了";
            this.btnLightTerminate.UseVisualStyleBackColor = true;
            this.btnLightTerminate.Click += new System.EventHandler(this.btnLightTerminate_Click);
            // 
            // btnLightOn
            // 
            this.btnLightOn.Location = new System.Drawing.Point(29, 154);
            this.btnLightOn.Name = "btnLightOn";
            this.btnLightOn.Size = new System.Drawing.Size(75, 23);
            this.btnLightOn.TabIndex = 4;
            this.btnLightOn.Text = "照明オン";
            this.btnLightOn.UseVisualStyleBackColor = true;
            this.btnLightOn.Click += new System.EventHandler(this.btnLightOn_Click);
            // 
            // nudLightValuePercent
            // 
            this.nudLightValuePercent.Location = new System.Drawing.Point(132, 158);
            this.nudLightValuePercent.Name = "nudLightValuePercent";
            this.nudLightValuePercent.Size = new System.Drawing.Size(74, 19);
            this.nudLightValuePercent.TabIndex = 5;
            this.nudLightValuePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "%";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(257, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 109);
            this.label2.TabIndex = 7;
            this.label2.Text = "照明ドライバは以下外部リクエストに耐えられれば良い。\r\n\r\n・照明値の変更\r\n・自動起動とポーリング\r\n・自動復旧\r\n・終了（照明を消して、スレッドを落とす）";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 344);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudLightValuePercent);
            this.Controls.Add(this.btnLightOn);
            this.Controls.Add(this.btnLightTerminate);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.txtReceive);
            this.Controls.Add(this.txtWrite);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudLightValuePercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWrite;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Button btnLightTerminate;
        private System.Windows.Forms.Button btnLightOn;
        private System.Windows.Forms.NumericUpDown nudLightValuePercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

