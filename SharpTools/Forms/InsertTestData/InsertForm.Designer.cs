namespace SharpTools
{
    partial class InsertForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertForm));
            this.gpBoxColNames = new System.Windows.Forms.GroupBox();
            this.CKlst = new System.Windows.Forms.CheckedListBox();
            this.gpBoxSet = new System.Windows.Forms.GroupBox();
            this.gpBoxDS = new System.Windows.Forms.GroupBox();
            this.gpBoxColNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpBoxColNames
            // 
            this.gpBoxColNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gpBoxColNames.Controls.Add(this.CKlst);
            this.gpBoxColNames.Location = new System.Drawing.Point(30, 28);
            this.gpBoxColNames.Name = "gpBoxColNames";
            this.gpBoxColNames.Size = new System.Drawing.Size(306, 629);
            this.gpBoxColNames.TabIndex = 0;
            this.gpBoxColNames.TabStop = false;
            this.gpBoxColNames.Text = "所有列";
            // 
            // CKlst
            // 
            this.CKlst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CKlst.FormattingEnabled = true;
            this.CKlst.Location = new System.Drawing.Point(19, 25);
            this.CKlst.Name = "CKlst";
            this.CKlst.Size = new System.Drawing.Size(267, 584);
            this.CKlst.TabIndex = 0;
            this.CKlst.SelectedIndexChanged += new System.EventHandler(this.CKlst_SelectedIndexChanged);
            // 
            // gpBoxSet
            // 
            this.gpBoxSet.Location = new System.Drawing.Point(365, 28);
            this.gpBoxSet.Name = "gpBoxSet";
            this.gpBoxSet.Size = new System.Drawing.Size(243, 629);
            this.gpBoxSet.TabIndex = 2;
            this.gpBoxSet.TabStop = false;
            this.gpBoxSet.Text = "设置";
            // 
            // gpBoxDS
            // 
            this.gpBoxDS.Location = new System.Drawing.Point(649, 28);
            this.gpBoxDS.Name = "gpBoxDS";
            this.gpBoxDS.Size = new System.Drawing.Size(337, 629);
            this.gpBoxDS.TabIndex = 3;
            this.gpBoxDS.TabStop = false;
            this.gpBoxDS.Text = "数据来源";
            // 
            // InsertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 707);
            this.Controls.Add(this.gpBoxDS);
            this.Controls.Add(this.gpBoxSet);
            this.Controls.Add(this.gpBoxColNames);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InsertForm";
            this.Text = "InsertForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.InsertForm_Load);
            this.gpBoxColNames.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpBoxColNames;
        private System.Windows.Forms.CheckedListBox CKlst;
        private System.Windows.Forms.GroupBox gpBoxSet;
        private System.Windows.Forms.GroupBox gpBoxDS;
    }
}