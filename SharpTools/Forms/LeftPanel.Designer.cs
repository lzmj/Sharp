﻿namespace SharpTools
{
    partial class LeftPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftPanel));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("服务器");
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tview = new System.Windows.Forms.TreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripTop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDatabase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOneDataBase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.表别名设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成CHM帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批量生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插入测试数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出CHM文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入PDM文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStripTop.SuspendLayout();
            this.contextMenuStripDatabase.SuspendLayout();
            this.contextMenuStripOneDataBase.SuspendLayout();
            this.contextMenuStripTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(173, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.ToolTipText = "新建数据库连接";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tview
            // 
            this.tview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tview.ImageIndex = 0;
            this.tview.ImageList = this.imgList;
            this.tview.Location = new System.Drawing.Point(0, 25);
            this.tview.Name = "tview";
            treeNode1.Name = "节点0";
            treeNode1.Text = "服务器";
            this.tview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tview.SelectedImageIndex = 0;
            this.tview.Size = new System.Drawing.Size(173, 339);
            this.tview.TabIndex = 1;
            this.tview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tview_NodeMouseClick);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "server.ICO");
            this.imgList.Images.SetKeyName(1, "database.ICO");
            this.imgList.Images.SetKeyName(2, "file.ICO");
            this.imgList.Images.SetKeyName(3, "fileopen.ICO");
            this.imgList.Images.SetKeyName(4, "table.ICO");
            // 
            // contextMenuStripTop
            // 
            this.contextMenuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.contextMenuStripTop.Name = "contextMenuStripTop";
            this.contextMenuStripTop.Size = new System.Drawing.Size(95, 48);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.新建ToolStripMenuItem.Text = "添加";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // contextMenuStripDatabase
            // 
            this.contextMenuStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.刷新ToolStripMenuItem1});
            this.contextMenuStripDatabase.Name = "contextMenuStripDatabase";
            this.contextMenuStripDatabase.Size = new System.Drawing.Size(95, 70);
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.连接ToolStripMenuItem.Text = "连接";
            this.连接ToolStripMenuItem.Click += new System.EventHandler(this.连接ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.刷新ToolStripMenuItem1_Click);
            // 
            // contextMenuStripOneDataBase
            // 
            this.contextMenuStripOneDataBase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.表别名设置ToolStripMenuItem,
            this.生成CHM帮助ToolStripMenuItem,
            this.批量生成ToolStripMenuItem,
            this.刷新ToolStripMenuItem2});
            this.contextMenuStripOneDataBase.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripOneDataBase.Size = new System.Drawing.Size(153, 114);
            // 
            // 表别名设置ToolStripMenuItem
            // 
            this.表别名设置ToolStripMenuItem.Name = "表别名设置ToolStripMenuItem";
            this.表别名设置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.表别名设置ToolStripMenuItem.Text = "查看表别名";
            this.表别名设置ToolStripMenuItem.Click += new System.EventHandler(this.表别名设置ToolStripMenuItem_Click);
            // 
            // 生成CHM帮助ToolStripMenuItem
            // 
            this.生成CHM帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出CHM文件ToolStripMenuItem,
            this.导入PDM文件ToolStripMenuItem});
            this.生成CHM帮助ToolStripMenuItem.Name = "生成CHM帮助ToolStripMenuItem";
            this.生成CHM帮助ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.生成CHM帮助ToolStripMenuItem.Text = "CHM文件…";
            // 
            // 批量生成ToolStripMenuItem
            // 
            this.批量生成ToolStripMenuItem.Name = "批量生成ToolStripMenuItem";
            this.批量生成ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.批量生成ToolStripMenuItem.Text = "批量生成";
            this.批量生成ToolStripMenuItem.Click += new System.EventHandler(this.批量生成ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem2
            // 
            this.刷新ToolStripMenuItem2.Name = "刷新ToolStripMenuItem2";
            this.刷新ToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.刷新ToolStripMenuItem2.Text = "刷新";
            this.刷新ToolStripMenuItem2.Click += new System.EventHandler(this.刷新ToolStripMenuItem2_Click);
            // 
            // contextMenuStripTable
            // 
            this.contextMenuStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.生成代码ToolStripMenuItem,
            this.插入测试数据ToolStripMenuItem});
            this.contextMenuStripTable.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripTable.Size = new System.Drawing.Size(143, 70);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 生成代码ToolStripMenuItem
            // 
            this.生成代码ToolStripMenuItem.Name = "生成代码ToolStripMenuItem";
            this.生成代码ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.生成代码ToolStripMenuItem.Text = "生成代码";
            this.生成代码ToolStripMenuItem.Click += new System.EventHandler(this.生成代码ToolStripMenuItem_Click);
            // 
            // 插入测试数据ToolStripMenuItem
            // 
            this.插入测试数据ToolStripMenuItem.Name = "插入测试数据ToolStripMenuItem";
            this.插入测试数据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.插入测试数据ToolStripMenuItem.Text = "插入测试数据";
            this.插入测试数据ToolStripMenuItem.Click += new System.EventHandler(this.插入测试数据ToolStripMenuItem_Click);
            // 
            // 导出CHM文件ToolStripMenuItem
            // 
            this.导出CHM文件ToolStripMenuItem.Name = "导出CHM文件ToolStripMenuItem";
            this.导出CHM文件ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导出CHM文件ToolStripMenuItem.Text = "导出CHM文件";
            this.导出CHM文件ToolStripMenuItem.Click += new System.EventHandler(this.导出CHM文件ToolStripMenuItem_Click);
            // 
            // 导入PDM文件ToolStripMenuItem
            // 
            this.导入PDM文件ToolStripMenuItem.Name = "导入PDM文件ToolStripMenuItem";
            this.导入PDM文件ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导入PDM文件ToolStripMenuItem.Text = "导入PDM文件";
            this.导入PDM文件ToolStripMenuItem.ToolTipText = "更新数据库的表或列描述";
            this.导入PDM文件ToolStripMenuItem.Click += new System.EventHandler(this.导入PDM文件ToolStripMenuItem_Click);
            // 
            // LeftPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(173, 364);
            this.Controls.Add(this.tview);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LeftPanel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "数据库视图";
            this.Load += new System.EventHandler(this.LeftPanel_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStripTop.ResumeLayout(false);
            this.contextMenuStripDatabase.ResumeLayout(false);
            this.contextMenuStripOneDataBase.ResumeLayout(false);
            this.contextMenuStripTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.TreeView tview;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTop;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDatabase;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOneDataBase;
        private System.Windows.Forms.ToolStripMenuItem 批量生成ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTable;
        private System.Windows.Forms.ToolStripMenuItem 生成代码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 生成CHM帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 插入测试数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表别名设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出CHM文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入PDM文件ToolStripMenuItem;
    }
}