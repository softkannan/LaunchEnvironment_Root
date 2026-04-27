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
            this._configListBox = new System.Windows.Forms.CheckedListBox();
            this._configListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._openToolFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._environmentLabel = new System.Windows.Forms.Label();
            this._mainToolBar = new System.Windows.Forms.ToolStrip();
            this._mainMenu = new System.Windows.Forms.MenuStrip();
            this._editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._integrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._runAsContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._configListContextMenu.SuspendLayout();
            this._mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _configListBox
            // 
            this._configListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._configListBox.ContextMenuStrip = this._configListContextMenu;
            this._configListBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._configListBox.FormattingEnabled = true;
            this._configListBox.Location = new System.Drawing.Point(12, 116);
            this._configListBox.Name = "_configListBox";
            this._configListBox.Size = new System.Drawing.Size(655, 38);
            this._configListBox.TabIndex = 0;
            // 
            // _configListContextMenu
            // 
            this._configListContextMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._configListContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._configListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openToolFolderToolStripMenuItem});
            this._configListContextMenu.Name = "envListContextMenu";
            this._configListContextMenu.Size = new System.Drawing.Size(274, 40);
            // 
            // _openToolFolderToolStripMenuItem
            // 
            this._openToolFolderToolStripMenuItem.Name = "_openToolFolderToolStripMenuItem";
            this._openToolFolderToolStripMenuItem.Size = new System.Drawing.Size(273, 36);
            this._openToolFolderToolStripMenuItem.Text = "Open Tool Folder";
            // 
            // _environmentLabel
            // 
            this._environmentLabel.AutoSize = true;
            this._environmentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._environmentLabel.Location = new System.Drawing.Point(9, 90);
            this._environmentLabel.Name = "_environmentLabel";
            this._environmentLabel.Size = new System.Drawing.Size(184, 20);
            this._environmentLabel.TabIndex = 1;
            this._environmentLabel.Text = "Select Environment :";
            // 
            // _mainToolBar
            // 
            this._mainToolBar.AutoSize = false;
            this._mainToolBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            this._mainToolBar.Location = new System.Drawing.Point(0, 40);
            this._mainToolBar.Name = "_mainToolBar";
            this._mainToolBar.Size = new System.Drawing.Size(546, 45);
            this._mainToolBar.TabIndex = 2;
            // 
            // _mainMenu
            // 
            this._mainMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._editorsToolStripMenuItem,
            this._toolsToolStripMenuItem,
            this._integrationToolStripMenuItem,
            this._helpToolStripMenuItem});
            this._mainMenu.Location = new System.Drawing.Point(0, 0);
            this._mainMenu.Name = "_mainMenu";
            this._mainMenu.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this._mainMenu.Size = new System.Drawing.Size(546, 40);
            this._mainMenu.TabIndex = 3;
            this._mainMenu.Text = "menuStrip1";
            // 
            // _editorsToolStripMenuItem
            // 
            this._editorsToolStripMenuItem.Name = "_editorsToolStripMenuItem";
            this._editorsToolStripMenuItem.Size = new System.Drawing.Size(100, 36);
            this._editorsToolStripMenuItem.Text = "Editors";
            this._editorsToolStripMenuItem.Visible = false;
            // 
            // _toolsToolStripMenuItem
            // 
            this._toolsToolStripMenuItem.Name = "_toolsToolStripMenuItem";
            this._toolsToolStripMenuItem.Size = new System.Drawing.Size(83, 36);
            this._toolsToolStripMenuItem.Text = "Tools";
            this._toolsToolStripMenuItem.Visible = false;
            // 
            // _integrationToolStripMenuItem
            // 
            this._integrationToolStripMenuItem.Name = "_integrationToolStripMenuItem";
            this._integrationToolStripMenuItem.Size = new System.Drawing.Size(145, 36);
            this._integrationToolStripMenuItem.Text = "Integration";
            this._integrationToolStripMenuItem.Visible = false;
            // 
            // _helpToolStripMenuItem
            // 
            this._helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutToolStripMenuItem});
            this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            this._helpToolStripMenuItem.Size = new System.Drawing.Size(78, 36);
            this._helpToolStripMenuItem.Text = "Help";
            // 
            // _aboutToolStripMenuItem
            // 
            this._aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            this._aboutToolStripMenuItem.Size = new System.Drawing.Size(167, 36);
            this._aboutToolStripMenuItem.Text = "About";
            // 
            // _runAsContext
            // 
            this._runAsContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._runAsContext.Name = "runAsContext";
            this._runAsContext.Size = new System.Drawing.Size(61, 4);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 171);
            this.Controls.Add(this._mainToolBar);
            this.Controls.Add(this._mainMenu);
            this.Controls.Add(this._environmentLabel);
            this.Controls.Add(this._configListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._mainMenu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(564, 651);
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launch Environment";
            this._configListContextMenu.ResumeLayout(false);
            this._mainMenu.ResumeLayout(false);
            this._mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox _configListBox;
        private System.Windows.Forms.Label _environmentLabel;
        private System.Windows.Forms.ToolStrip _mainToolBar;
        private System.Windows.Forms.MenuStrip _mainMenu;
        private System.Windows.Forms.ContextMenuStrip _configListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _openToolFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _editorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _integrationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip _runAsContext;
    }
}

