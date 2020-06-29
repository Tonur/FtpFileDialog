namespace FtpFileDialog
{
  partial class BrowseDialog
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
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.UpDirectoryButton = new System.Windows.Forms.ToolStripButton();
      this.LoadNewHostButton = new System.Windows.Forms.ToolStripButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.CancelButton = new System.Windows.Forms.Button();
      this.ChooseButton = new System.Windows.Forms.Button();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.DirectoryTree = new System.Windows.Forms.TreeView();
      this.FileList = new System.Windows.Forms.ListView();
      this.toolStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpDirectoryButton,
            this.LoadNewHostButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(903, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // UpDirectoryButton
      // 
      this.UpDirectoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.UpDirectoryButton.Enabled = false;
      this.UpDirectoryButton.Image = global::FtpFileDialog.Properties.Resources.ParentFolder_16x;
      this.UpDirectoryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.UpDirectoryButton.Name = "UpDirectoryButton";
      this.UpDirectoryButton.Size = new System.Drawing.Size(23, 22);
      this.UpDirectoryButton.Text = "toolStripButton1";
      this.UpDirectoryButton.ToolTipText = "Go back";
      this.UpDirectoryButton.Click += new System.EventHandler(this.UpDirectoryButton_Click);
      // 
      // LoadNewHostButton
      // 
      this.LoadNewHostButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.LoadNewHostButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.LoadNewHostButton.Image = global::FtpFileDialog.Properties.Resources.OpenFolder_16x;
      this.LoadNewHostButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.LoadNewHostButton.Name = "LoadNewHostButton";
      this.LoadNewHostButton.Size = new System.Drawing.Size(23, 22);
      this.LoadNewHostButton.Text = "Indlæs ny adresse";
      this.LoadNewHostButton.ToolTipText = "Åben ny adresse";
      this.LoadNewHostButton.Click += new System.EventHandler(this.LoadNewHostButton_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.CancelButton);
      this.panel1.Controls.Add(this.ChooseButton);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 621);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(903, 29);
      this.panel1.TabIndex = 1;
      // 
      // CancelButton
      // 
      this.CancelButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelButton.Location = new System.Drawing.Point(735, 3);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 1;
      this.CancelButton.Text = "Annuller";
      this.CancelButton.UseVisualStyleBackColor = true;
      // 
      // ChooseButton
      // 
      this.ChooseButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.ChooseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.ChooseButton.Location = new System.Drawing.Point(816, 3);
      this.ChooseButton.Name = "ChooseButton";
      this.ChooseButton.Size = new System.Drawing.Size(75, 23);
      this.ChooseButton.TabIndex = 0;
      this.ChooseButton.Text = "Vælg";
      this.ChooseButton.UseVisualStyleBackColor = true;
      this.ChooseButton.Click += new System.EventHandler(this.ChooseButton_Click);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 25);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.DirectoryTree);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.FileList);
      this.splitContainer1.Size = new System.Drawing.Size(903, 596);
      this.splitContainer1.SplitterDistance = 301;
      this.splitContainer1.TabIndex = 2;
      this.splitContainer1.TabStop = false;
      // 
      // DirectoryTree
      // 
      this.DirectoryTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DirectoryTree.Location = new System.Drawing.Point(0, 0);
      this.DirectoryTree.Name = "DirectoryTree";
      this.DirectoryTree.PathSeparator = "/";
      this.DirectoryTree.ShowLines = false;
      this.DirectoryTree.ShowRootLines = false;
      this.DirectoryTree.Size = new System.Drawing.Size(301, 596);
      this.DirectoryTree.TabIndex = 0;
      this.DirectoryTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.DirectoryTree_BeforeCollapse);
      this.DirectoryTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DirectoryTree_BeforeExpand);
      this.DirectoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DirectoryTree_AfterSelect);
      this.DirectoryTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DirectoryTree_NodeMouseClick);
      this.DirectoryTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DirectoryTree_NodeMouseDoubleClick);
      // 
      // FileList
      // 
      this.FileList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FileList.HideSelection = false;
      this.FileList.Location = new System.Drawing.Point(0, 0);
      this.FileList.MultiSelect = false;
      this.FileList.Name = "FileList";
      this.FileList.Size = new System.Drawing.Size(598, 596);
      this.FileList.TabIndex = 0;
      this.FileList.UseCompatibleStateImageBehavior = false;
      this.FileList.View = System.Windows.Forms.View.List;
      this.FileList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FileList_ItemSelectionChanged);
      this.FileList.SelectedIndexChanged += new System.EventHandler(this.FileList_SelectedIndexChanged);
      this.FileList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FileList_KeyPress);
      this.FileList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FileList_MouseDoubleClick);
      // 
      // BrowseDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(903, 650);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.toolStrip1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "BrowseDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "FtpFileDialog";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button CancelButton;
    private System.Windows.Forms.Button ChooseButton;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView DirectoryTree;
    private System.Windows.Forms.ListView FileList;
    private System.Windows.Forms.ToolStripButton LoadNewHostButton;
    private System.Windows.Forms.ToolStripButton UpDirectoryButton;
  }
}