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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            _configListBox = new System.Windows.Forms.CheckedListBox();
            _configListContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            _openToolFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _environmentLabel = new System.Windows.Forms.Label();
            _mainToolBar = new System.Windows.Forms.ToolStrip();
            _mainMenu = new System.Windows.Forms.MenuStrip();
            _editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _integrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _runAsContext = new System.Windows.Forms.ContextMenuStrip(components);
            _configListContextMenu.SuspendLayout();
            _mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // _configListBox
            // 
            _configListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _configListBox.ContextMenuStrip = _configListContextMenu;
            _configListBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _configListBox.FormattingEnabled = true;
            _configListBox.Location = new System.Drawing.Point(16, 178);
            _configListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            _configListBox.Name = "_configListBox";
            _configListBox.Size = new System.Drawing.Size(697, 38);
            _configListBox.TabIndex = 0;
            _configListBox.SelectedIndexChanged += _configListBox_SelectedIndexChanged;
            // 
            // _configListContextMenu
            // 
            _configListContextMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _configListContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            _configListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _openToolFolderToolStripMenuItem });
            _configListContextMenu.Name = "envListContextMenu";
            _configListContextMenu.Size = new System.Drawing.Size(274, 40);
            // 
            // _openToolFolderToolStripMenuItem
            // 
            _openToolFolderToolStripMenuItem.Name = "_openToolFolderToolStripMenuItem";
            _openToolFolderToolStripMenuItem.Size = new System.Drawing.Size(273, 36);
            _openToolFolderToolStripMenuItem.Text = "Open Tool Folder";
            _openToolFolderToolStripMenuItem.Click += openToolFolderToolStripMenuItem_Click;
            // 
            // _environmentLabel
            // 
            _environmentLabel.AutoSize = true;
            _environmentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _environmentLabel.Location = new System.Drawing.Point(12, 138);
            _environmentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _environmentLabel.Name = "_environmentLabel";
            _environmentLabel.Size = new System.Drawing.Size(184, 20);
            _environmentLabel.TabIndex = 1;
            _environmentLabel.Text = "Select Environment :";
            // 
            // _mainToolBar
            // 
            _mainToolBar.AutoSize = false;
            _mainToolBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            _mainToolBar.Location = new System.Drawing.Point(0, 42);
            _mainToolBar.Name = "_mainToolBar";
            _mainToolBar.Size = new System.Drawing.Size(728, 69);
            _mainToolBar.TabIndex = 2;
            // 
            // _mainMenu
            // 
            _mainMenu.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            _mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _editorsToolStripMenuItem, _toolsToolStripMenuItem, _integrationToolStripMenuItem, _helpToolStripMenuItem });
            _mainMenu.Location = new System.Drawing.Point(0, 0);
            _mainMenu.Name = "_mainMenu";
            _mainMenu.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            _mainMenu.Size = new System.Drawing.Size(728, 42);
            _mainMenu.TabIndex = 3;
            _mainMenu.Text = "menuStrip1";
            // 
            // _editorsToolStripMenuItem
            // 
            _editorsToolStripMenuItem.Name = "_editorsToolStripMenuItem";
            _editorsToolStripMenuItem.Size = new System.Drawing.Size(100, 36);
            _editorsToolStripMenuItem.Text = "Editors";
            _editorsToolStripMenuItem.Visible = false;
            // 
            // _toolsToolStripMenuItem
            // 
            _toolsToolStripMenuItem.Name = "_toolsToolStripMenuItem";
            _toolsToolStripMenuItem.Size = new System.Drawing.Size(83, 36);
            _toolsToolStripMenuItem.Text = "Tools";
            _toolsToolStripMenuItem.Visible = false;
            // 
            // _integrationToolStripMenuItem
            // 
            _integrationToolStripMenuItem.Name = "_integrationToolStripMenuItem";
            _integrationToolStripMenuItem.Size = new System.Drawing.Size(145, 36);
            _integrationToolStripMenuItem.Text = "Integration";
            _integrationToolStripMenuItem.Visible = false;
            // 
            // _helpToolStripMenuItem
            // 
            _helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _aboutToolStripMenuItem });
            _helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
            _helpToolStripMenuItem.Size = new System.Drawing.Size(78, 36);
            _helpToolStripMenuItem.Text = "Help";
            // 
            // _aboutToolStripMenuItem
            // 
            _aboutToolStripMenuItem.Name = "_aboutToolStripMenuItem";
            _aboutToolStripMenuItem.Size = new System.Drawing.Size(167, 36);
            _aboutToolStripMenuItem.Text = "About";
            _aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // _runAsContext
            // 
            _runAsContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            _runAsContext.Name = "runAsContext";
            _runAsContext.Size = new System.Drawing.Size(61, 4);
            // 
            // mainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(728, 263);
            Controls.Add(_mainToolBar);
            Controls.Add(_mainMenu);
            Controls.Add(_environmentLabel);
            Controls.Add(_configListBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = _mainMenu;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(746, 976);
            Name = "mainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Launch Environment";
            Load += mainForm_Load;
            _configListContextMenu.ResumeLayout(false);
            _mainMenu.ResumeLayout(false);
            _mainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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

