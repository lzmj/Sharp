namespace SharpTools.Forms
{
    partial class ImportPDMForm
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
            this.txtMulItem = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnBrow = new System.Windows.Forms.Button();
            this.BtnUpdateDisplayName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMulItem
            // 
            this.txtMulItem.Location = new System.Drawing.Point(100, 25);
            this.txtMulItem.Multiline = true;
            this.txtMulItem.Name = "txtMulItem";
            this.txtMulItem.Size = new System.Drawing.Size(486, 57);
            this.txtMulItem.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Pdm 文件路径：";
            // 
            // BtnBrow
            // 
            this.BtnBrow.Location = new System.Drawing.Point(605, 37);
            this.BtnBrow.Name = "BtnBrow";
            this.BtnBrow.Size = new System.Drawing.Size(82, 36);
            this.BtnBrow.TabIndex = 5;
            this.BtnBrow.Text = "浏览(可多选)";
            this.BtnBrow.UseVisualStyleBackColor = true;
            this.BtnBrow.Click += new System.EventHandler(this.BtnBrow_Click);
            // 
            // BtnUpdateDisplayName
            // 
            this.BtnUpdateDisplayName.Location = new System.Drawing.Point(712, 37);
            this.BtnUpdateDisplayName.Name = "BtnUpdateDisplayName";
            this.BtnUpdateDisplayName.Size = new System.Drawing.Size(89, 36);
            this.BtnUpdateDisplayName.TabIndex = 8;
            this.BtnUpdateDisplayName.Text = "更新表列别名";
            this.BtnUpdateDisplayName.UseVisualStyleBackColor = true;
            this.BtnUpdateDisplayName.Click += new System.EventHandler(this.BtnUpdateDisplayName_Click);
            // 
            // ImportPDMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 142);
            this.Controls.Add(this.BtnUpdateDisplayName);
            this.Controls.Add(this.txtMulItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnBrow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportPDMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImportPDMForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMulItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnBrow;
        private System.Windows.Forms.Button BtnUpdateDisplayName;
    }
}