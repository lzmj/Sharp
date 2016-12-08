namespace SharpTools
{
    partial class ContentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentForm));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSetColDisplay = new System.Windows.Forms.Button();
            this.gridColumns = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSqlContent = new ICSharpCode.TextEditor.TextEditorControl();
            this.ckl_DBTypes = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(642, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSetColDisplay
            // 
            this.btnSetColDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetColDisplay.Location = new System.Drawing.Point(561, 2);
            this.btnSetColDisplay.Name = "btnSetColDisplay";
            this.btnSetColDisplay.Size = new System.Drawing.Size(75, 23);
            this.btnSetColDisplay.TabIndex = 11;
            this.btnSetColDisplay.Text = "设置列别名";
            this.btnSetColDisplay.UseVisualStyleBackColor = true;
            this.btnSetColDisplay.Click += new System.EventHandler(this.btnSetColDisplay_Click);
            // 
            // gridColumns
            // 
            this.gridColumns.AllowUserToAddRows = false;
            this.gridColumns.AllowUserToDeleteRows = false;
            this.gridColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColumns.Location = new System.Drawing.Point(12, 31);
            this.gridColumns.Name = "gridColumns";
            this.gridColumns.RowTemplate.Height = 23;
            this.gridColumns.Size = new System.Drawing.Size(726, 260);
            this.gridColumns.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtSqlContent);
            this.groupBox1.Controls.Add(this.ckl_DBTypes);
            this.groupBox1.Location = new System.Drawing.Point(12, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(726, 232);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sql脚本";
            // 
            // txtSqlContent
            // 
            this.txtSqlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlContent.Encoding = ((System.Text.Encoding)(resources.GetObject("txtSqlContent.Encoding")));
            this.txtSqlContent.Location = new System.Drawing.Point(6, 46);
            this.txtSqlContent.Name = "txtSqlContent";
            this.txtSqlContent.ShowEOLMarkers = true;
            this.txtSqlContent.ShowSpaces = true;
            this.txtSqlContent.ShowTabs = true;
            this.txtSqlContent.ShowVRuler = true;
            this.txtSqlContent.Size = new System.Drawing.Size(714, 180);
            this.txtSqlContent.TabIndex = 2;
            // 
            // ckl_DBTypes
            // 
            this.ckl_DBTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckl_DBTypes.FormattingEnabled = true;
            this.ckl_DBTypes.Location = new System.Drawing.Point(6, 20);
            this.ckl_DBTypes.MultiColumn = true;
            this.ckl_DBTypes.Name = "ckl_DBTypes";
            this.ckl_DBTypes.Size = new System.Drawing.Size(714, 20);
            this.ckl_DBTypes.TabIndex = 1;
            this.ckl_DBTypes.UseCompatibleTextRendering = true;
            this.ckl_DBTypes.SelectedIndexChanged += new System.EventHandler(this.ckl_DBTypes_SelectedIndexChanged);
            // 
            // ContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 541);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridColumns);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSetColDisplay);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ContentForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ContentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSetColDisplay;
        private System.Windows.Forms.DataGridView gridColumns;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox ckl_DBTypes;
        private ICSharpCode.TextEditor.TextEditorControl txtSqlContent;
    }
}