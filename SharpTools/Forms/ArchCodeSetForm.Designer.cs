namespace SharpTools.Forms
{
    partial class ArchCodeSetForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Ckd_Lbx = new System.Windows.Forms.CheckedListBox();
            this.tplComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBuilder = new System.Windows.Forms.Button();
            this.cbToupperFrstword = new System.Windows.Forms.CheckBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.txtProjName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Ckd_Lbx);
            this.groupBox1.Controls.Add(this.tplComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnBuilder);
            this.groupBox1.Controls.Add(this.cbToupperFrstword);
            this.groupBox1.Controls.Add(this.txtClassName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtModule);
            this.groupBox1.Controls.Add(this.txtProjName);
            this.groupBox1.Location = new System.Drawing.Point(78, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(856, 267);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生成配置";
            // 
            // Ckd_Lbx
            // 
            this.Ckd_Lbx.FormattingEnabled = true;
            this.Ckd_Lbx.Location = new System.Drawing.Point(27, 139);
            this.Ckd_Lbx.Name = "Ckd_Lbx";
            this.Ckd_Lbx.Size = new System.Drawing.Size(810, 68);
            this.Ckd_Lbx.TabIndex = 10;
            this.Ckd_Lbx.SelectedIndexChanged += new System.EventHandler(this.Ckd_Lbx_SelectedIndexChanged);
            // 
            // tplComboBox
            // 
            this.tplComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tplComboBox.FormattingEnabled = true;
            this.tplComboBox.Location = new System.Drawing.Point(73, 20);
            this.tplComboBox.Name = "tplComboBox";
            this.tplComboBox.Size = new System.Drawing.Size(298, 20);
            this.tplComboBox.TabIndex = 9;
            this.tplComboBox.SelectedIndexChanged += new System.EventHandler(this.tplComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "模  板：";
            // 
            // btnBuilder
            // 
            this.btnBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuilder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBuilder.Location = new System.Drawing.Point(114, 213);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(574, 30);
            this.btnBuilder.TabIndex = 6;
            this.btnBuilder.Text = "生成代码";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnBuilder_Click);
            // 
            // cbToupperFrstword
            // 
            this.cbToupperFrstword.AutoSize = true;
            this.cbToupperFrstword.Location = new System.Drawing.Point(715, 63);
            this.cbToupperFrstword.Name = "cbToupperFrstword";
            this.cbToupperFrstword.Size = new System.Drawing.Size(84, 16);
            this.cbToupperFrstword.TabIndex = 6;
            this.cbToupperFrstword.Text = "首字母大写";
            this.cbToupperFrstword.UseVisualStyleBackColor = true;
            this.cbToupperFrstword.CheckedChanged += new System.EventHandler(this.cbToupperFrstword_CheckedChanged);
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(538, 62);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(150, 21);
            this.txtClassName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(477, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "类  名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "模块名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "项目名称：";
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(302, 65);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(150, 21);
            this.txtModule.TabIndex = 4;
            // 
            // txtProjName
            // 
            this.txtProjName.Location = new System.Drawing.Point(73, 61);
            this.txtProjName.Name = "txtProjName";
            this.txtProjName.Size = new System.Drawing.Size(150, 21);
            this.txtProjName.TabIndex = 4;
            // 
            // ArchCodeSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 576);
            this.Controls.Add(this.groupBox1);
            this.Name = "ArchCodeSetForm";
            this.Text = "ArchCodeSetForm";
            this.Load += new System.EventHandler(this.ArchCodeSetForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox Ckd_Lbx;
        private System.Windows.Forms.ComboBox tplComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBuilder;
        private System.Windows.Forms.CheckBox cbToupperFrstword;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.TextBox txtProjName;

    }
}