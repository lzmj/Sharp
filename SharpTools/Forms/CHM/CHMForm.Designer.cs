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
            this.btnBuilder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCHM_Name
            // 
            this.txtCHM_Name.Location = new System.Drawing.Point(127, 60);
            this.txtCHM_Name.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCHM_Name.Name = "txtCHM_Name";
            this.txtCHM_Name.Size = new System.Drawing.Size(368, 25);
            this.txtCHM_Name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "CHM文件名：";
            // 
            // btnMakeCHM
            // 
            this.btnMakeCHM.Location = new System.Drawing.Point(788, 59);
            this.btnMakeCHM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMakeCHM.Name = "btnMakeCHM";
            this.btnMakeCHM.Size = new System.Drawing.Size(100, 29);
            this.btnMakeCHM.TabIndex = 2;
            this.btnMakeCHM.Text = "导出CHM";
            this.btnMakeCHM.UseVisualStyleBackColor = true;
            this.btnMakeCHM.Click += new System.EventHandler(this.btnMakeCHM_Click);
            // 
            // CkRetainHtml
            // 
            this.CkRetainHtml.AutoSize = true;
            this.CkRetainHtml.Location = new System.Drawing.Point(523, 63);
            this.CkRetainHtml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CkRetainHtml.Name = "CkRetainHtml";
            this.CkRetainHtml.Size = new System.Drawing.Size(121, 19);
            this.CkRetainHtml.TabIndex = 3;
            this.CkRetainHtml.Text = "保留html文件";
            this.CkRetainHtml.UseVisualStyleBackColor = true;
            // 
            // btnBuilder
            // 
            this.btnBuilder.Location = new System.Drawing.Point(651, 60);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(114, 28);
            this.btnBuilder.TabIndex = 4;
            this.btnBuilder.Text = "生成html文件";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnBuilder_Click);
            // 
            // CHMForm
            // 
            this.AcceptButton = this.btnMakeCHM;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 196);
            this.Controls.Add(this.btnBuilder);
            this.Controls.Add(this.CkRetainHtml);
            this.Controls.Add(this.btnMakeCHM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCHM_Name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button btnBuilder;
    }
}