namespace LaunchEnvironment
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.envList = new System.Windows.Forms.CheckedListBox();
            this.envlistContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.environmentLabel = new System.Windows.Forms.Label();
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runAsContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.envlistContextMenu.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // envList
            // 
            this.envList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.envList.ContextMenuStrip = this.envlistContextMenu;
            this.envList.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.envList.FormattingEnabled = true;
            this.envList.Location = new System.Drawing.Point(12, 116);
            this.envList.Name = "envList";
            this.envList.Size = new System.Drawing.Size(524, 32);
            this.envList.TabIndex = 0;
            // 
            // envlistContextMenu
            // 
            this.envlistContextMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.envlistContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolFolderToolStripMenuItem});
            this.envlistContextMenu.Name = "envListContextMenu";
            this.envlistContextMenu.Size = new System.Drawing.Size(229, 34);
            // 
            // openToolFolderToolStripMenuItem
            // 
            this.openToolFolderToolStripMenuItem.Name = "openToolFolderToolStripMenuItem";
            this.openToolFolderToolStripMenuItem.Size = new System.Drawing.Size(228, 30);
            this.openToolFolderToolStripMenuItem.Text = "Open Tool Folder";
            this.openToolFolderToolStripMenuItem.Click += new System.EventHandler(this.openToolFolderToolStripMenuItem_Click);
            // 
            // environmentLabel
            // 
            this.environmentLabel.AutoSize = true;
            this.environmentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.environmentLabel.Location = new System.Drawing.Point(9, 90);
            this.environmentLabel.Name = "environmentLabel";
            this.environmentLabel.Size = new System.Drawing.Size(149, 16);
            this.environmentLabel.TabIndex = 1;
            this.environmentLabel.Text = "Select Environment :";
            // 
            // mainToolBar
            // 
            this.mainToolBar.AutoSize = false;
            this.mainToolBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.mainToolBar.Location = new System.Drawing.Point(0, 33);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(548, 45);
            this.mainToolBar.TabIndex = 2;
            // 
            // mainMenu
            // 
            this.mainMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.integrationToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(548, 33);
            this.mainMenu.TabIndex = 3;
            this.mainMenu.Text = "menuStrip1";
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(82, 29);
            this.editorsToolStripMenuItem.Text = "Editors";
            this.editorsToolStripMenuItem.Visible = false;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(67, 29);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // integrationToolStripMenuItem
            // 
            this.integrationToolStripMenuItem.Name = "integrationToolStripMenuItem";
            this.integrationToolStripMenuItem.Size = new System.Drawing.Size(117, 29);
            this.integrationToolStripMenuItem.Text = "Integration";
            this.integrationToolStripMenuItem.Visible = false;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(63, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(135, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // runAsContext
            // 
            this.runAsContext.Name = "runAsContext";
            this.runAsContext.Size = new System.Drawing.Size(61, 4);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 171);
            this.Controls.Add(this.mainToolBar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.environmentLabel);
            this.Controls.Add(this.envList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(564, 651);
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launch Environment";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.envlistContextMenu.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox envList;
        private System.Windows.Forms.Label environmentLabel;
        private System.Windows.Forms.ToolStrip mainToolBar;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ContextMenuStrip envlistContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem integrationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip runAsContext;
    }
}

