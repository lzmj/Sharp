namespace SharpTools
{
    partial class CHMForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CHMForm));
            this.txtCHM_Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMakeCHM = new System.Windows.Forms.Button();
            this.CkRetainHtml = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtCHM_Name
            // 
            this.txtCHM_Name.Location = new System.Drawing.Point(95, 48);
            this.txtCHM_Name.Name = "txtCHM_Name";
            this.txtCHM_Name.Size = new System.Drawing.Size(277, 21);
            this.txtCHM_Name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "CHM文件名：";
            // 
            // btnMakeCHM
            // 
            this.btnMakeCHM.Location = new System.Drawing.Point(503, 48);
            this.btnMakeCHM.Name = "btnMakeCHM";
            this.btnMakeCHM.Size = new System.Drawing.Size(75, 23);
            this.btnMakeCHM.TabIndex = 2;
            this.btnMakeCHM.Text = "导出";
            this.btnMakeCHM.UseVisualStyleBackColor = true;
            this.btnMakeCHM.Click += new System.EventHandler(this.btnMakeCHM_Click);
            // 
            // CkRetainHtml
            // 
            this.CkRetainHtml.AutoSize = true;
            this.CkRetainHtml.Location = new System.Drawing.Point(388, 52);
            this.CkRetainHtml.Name = "CkRetainHtml";
            this.CkRetainHtml.Size = new System.Drawing.Size(96, 16);
            this.CkRetainHtml.TabIndex = 3;
            this.CkRetainHtml.Text = "保留html文件";
            this.CkRetainHtml.UseVisualStyleBackColor = true;
            // 
            // CHMForm
            // 
            this.AcceptButton = this.btnMakeCHM;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 157);
            this.Controls.Add(this.CkRetainHtml);
            this.Controls.Add(this.btnMakeCHM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCHM_Name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CHMForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出成CHM";
            this.Load += new System.EventHandler(this.CHMForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCHM_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMakeCHM;
        private System.Windows.Forms.CheckBox CkRetainHtml;
    }
}