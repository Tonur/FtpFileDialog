using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using System.Text;
using System.Threading;
using static System.String;

namespace FtpFileDialog
{
  public partial class BrowseDialog : Form
  {
    #region Private Fields

    private BackgroundWorker _fsLoader;
    private FtpBrowseProgressDialog _progress;
    private FtpWebRequest _req;
    private FtpWebResponse _rsp;
    private readonly bool _promptForServer;

    #endregion

    #region Public Properties
    public ConnectionDetails Connection { get; set; }
    public bool TestMode { get; set; }

    public string BaseAddress => Connection.BaseAddress;

    public string ConnectionAddress => $@"ftp://{Connection.FtpCred.UserName}:{Connection.FtpCred.Password}@{Connection.Host}:{Connection.Port}/";

    public string StartPath => Connection.Path;

    public string StartAddress => $@"{BaseAddress}:{Connection.Port}/{StartPath}";

    /// <summary>
    /// Returns the selected file full path
    /// Example ftp://test.com/FromAcubiz/regnskab2000/regnskab254.csv
    /// </summary>
    public string SelectedFileDisplay => SelectedPathDisplay + "/" + (IsDirectory ? Empty : SelectedFileNameDisplay);

    /// <summary>
    /// 
    /// Example /FromAcubiz/regnskab2000/
    /// </summary>
    public string SelectedPathDisplay { get; set; }

    /// <summary>
    /// Returns the selected file full path encoded as to escape uri unfriendly characters
    /// Example ftp://test.com/FromAAge/regnskabs%E5r2000/
    /// </summary>
    public string SelectedPathUriEncoded => BaseAddress + RelativePathUriEncoded;

    /// <summary>
    /// 
    /// Example regnskab254.csv
    /// </summary>
    public string SelectedFileNameDisplay { get; private set; }

    /// <summary>
    /// 
    /// Example regnskabops%E6tning254.csv
    /// </summary>
    public string SelectedFileNameUriEncoded => Uri.EscapeUriString(SelectedFileNameDisplay);

    /// <summary>
    /// 
    /// Example ftp://test.com/FromAAge/regnskabs%E5r2000/regnskabops%E6tning254.csv
    /// </summary>
    public string SelectedFileUriEncoded => SelectedPathUriEncoded + (IsDirectory ? Empty : "/" + SelectedFileNameUriEncoded.TrimEnd('/'));

    /// <summary>
    /// 
    /// Example /FromAcubiz/regnskab2000/regnskab254.csv
    /// </summary>
    public string RelativeFileDisplay => SelectedFileDisplay.Replace(BaseAddress, "");

    /// <summary>
    /// 
    /// Example /FromAAge/regnskabs%E5r2000/regnskabops%E6tning254.csv
    /// </summary>
    public string RelativeFileUriEncoded => SelectedPathDisplay.Replace(BaseAddress, "") + (IsDirectory ? Empty : "/" + SelectedFileNameUriEncoded);

    /// <summary>
    /// The relative path of the selected items parrent
    /// Example /FromAcubiz/regnskab2000/
    /// </summary>
    public string RelativePathDisplay => IsDirectory ? RelativeFileDisplay : SelectedPathDisplay.Replace(BaseAddress, "");

    /// <summary>
    /// The relative path of the selected items parrent
    /// Example /FromAAge/regnskabs%E5r2000/
    /// </summary>
    public string RelativePathUriEncoded => Uri.EscapeUriString(SelectedPathDisplay.Replace(BaseAddress, ""));

    /// <summary>
    /// Returns the relative path or file, depending on if a file has been selected
    /// Example ftp://test.com/From%E5ge/regnskabs%E5r2000/ or ftp://test.com/From%E5ge/regnskabs%E5r2000/regnskabsops%E6tning254.csv
    /// </summary>
    public string DisplayShortName => string.IsNullOrWhiteSpace(SelectedFileNameDisplay) ? RelativePathDisplay : SelectedFileNameDisplay;

    /// <summary>
    /// Returns the whole path or whole file, depending on if a file has been selected
    /// Example ftp://test.com/From%E5ge/regnskabs%E5r2000/ or ftp://test.com/From%E5ge/regnskabs%E5r2000/regnskabsops%E6tning254.csv
    /// </summary>
    public string DisplayLongName => string.IsNullOrWhiteSpace(SelectedFileNameDisplay) ? SelectedPathDisplay : SelectedFileDisplay;

    public bool IsDirectory { get; set; } = true;

    private string _currentPath;

    #endregion

    #region Constructors
    public BrowseDialog()
      : this(
        new ConnectionDetails
        (
          Empty,
          Empty,
          new NetworkCredential(Empty, Empty),
          21,
          true
        ), true)
    {

    }

    public BrowseDialog(string hostUrl, string path, int port, string username, string password, bool passiveMode,
      bool promptForServer = false, CultureInfo cultureInfo = null)
      : this(new ConnectionDetails(hostUrl, path, new NetworkCredential(username, password), port, passiveMode),
        promptForServer, cultureInfo)
    {

    }

    public BrowseDialog(ConnectionDetails connectionDetails, bool promptForServer = false, CultureInfo cultureInfo = null)
    {
      InitializeComponent();
      if (cultureInfo != null)
      {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
      }

      _promptForServer = promptForServer;
      Connection = connectionDetails ?? new ConnectionDetails
      (
        Empty,
        Empty,
        new NetworkCredential(Empty, Empty),
        21,
        true
      );

      var directoryImages = new ImageList();
      directoryImages.Images.Add(Properties.Resources.FolderClosed_16x);
      directoryImages.Images.Add(Properties.Resources.FolderOpened_16x);
      DirectoryTree.ImageList = directoryImages;
      DirectoryTree.ImageIndex = 0;
      DirectoryTree.SelectedImageIndex = 0;
      var fileImages = new ImageList();
      fileImages.Images.Add(Properties.Resources.FolderClosed_16x);
      fileImages.Images.Add(Properties.Resources.Document_16x);
      FileList.SmallImageList = fileImages;

      SetupLocalization();
    }

    private void SetupLocalization()
    {
      CancelButton.Text = Properties.Resources.ResourceManager.GetString("Global_CancelButton", CultureInfo.CurrentUICulture);
      ChooseButton.Text = Properties.Resources.ResourceManager.GetString("BrowseDialog_ChooseButton", CultureInfo.CurrentUICulture);
      UpDirectoryButton.ToolTipText = Properties.Resources.ResourceManager.GetString("BrowseDialog_UpDirectoryButton", CultureInfo.CurrentUICulture);
      LoadNewHostButton.ToolTipText = Properties.Resources.ResourceManager.GetString("BrowseDialog_LoadNewHostButton", CultureInfo.CurrentUICulture);
    }
    #endregion

    #region Private Methods
    private void LoadTestNodes()
    {
      var root = new FtpTreeNode("root", "", 21);
      root.AddDirectory("Folder1");
      root.AddDirectory("Folder2");
      var node1 = new FtpTreeNode("root", "Folder1", 21) { Text = "Folder1" };
      var subnode1 = new FtpTreeNode("root", "Folder1/SubfolderA", 21) { Text = "SubfolderA" };
      node1.Directories.Add("SubfolderA");
      subnode1.Files.Add("SubfileA");
      node1.Nodes.Add(subnode1);
      node1.AddFile("File1");
      node1.AddFile("File2");
      var node2 = new FtpTreeNode("root", "Folder2", 21) { Text = "Folder2" };
      node2.AddFile("File3");
      root.Nodes.Add(node1);
      root.Nodes.Add(node2);
      root.Expand();
      DirectoryTree.Nodes.Add(root);
    }

    private void LoadSubNodes(FtpTreeNode rootNode)
    {
      var uri = Uri.TryCreate(rootNode.FtpPath, UriKind.Absolute, out var tryParseUri) 
        ? tryParseUri : rootNode.Path != Empty
          ? new UriBuilder("ftp", rootNode.Server, rootNode.Port, rootNode.Path).Uri
          : new UriBuilder("ftp", rootNode.Server, rootNode.Port).Uri;
      var uriString = uri.OriginalString;
      _req = (FtpWebRequest)WebRequest.Create(uri);
      _req.Credentials = Connection.FtpCred;
      _req.UsePassive = Connection.Passive;
      _req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
      try
      {
        _rsp = (FtpWebResponse)_req.GetResponse();
        var streamReader = new StreamReader(_rsp.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);

        var ftpResponse = FtpResponse.ReadFtpLinesToObject(streamReader);

        //var rspTokens = streamReader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //foreach (var str in rspTokens)
        //{
        foreach (var response in ftpResponse)
        {
          switch (response.ItemType)
          {
            case FileType.Directory:
              {
                var subnode = new FtpTreeNode(rootNode.Server,
                  rootNode.Path + "/" + response.FileName, rootNode.Port);
                subnode.Text = subnode.Directory;
                rootNode.Nodes.Add(subnode);
                LoadSubNodes(subnode);
                //rootNode.AddDirectory(response.FileName);
                goto case FileType.UpstreamDirectory; //Fallthrough, because it will not work for me, fucking stupid
              }
            case FileType.UpstreamDirectory:
              rootNode.AddDirectory(response.FileName);
              break;
            case FileType.CurrentDirectory:
              break;
            case FileType.File:
              rootNode.AddFile(response.FileName);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (WebException wEx)
      {

      }
    }

    private static void SetSubnodeBackColor(TreeNode node, Color c)
    {
      foreach (FtpTreeNode n in node.Nodes)
      {
        SetSubnodeBackColor(n, c);
        n.BackColor = c;
      }
      node.BackColor = c;
    }
    #endregion

    #region Event Triggers
    protected override void OnShown(EventArgs e)
    {
      if (TestMode)
      {
        LoadTestNodes();
      }
      else if (_promptForServer)
      {
        LoadNewHostButton.PerformClick();
      }
      else
      {
        _fsLoader = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        _fsLoader.DoWork += FsLoader_DoWork;
        _fsLoader.RunWorkerCompleted += FsLoader_RunWorkerCompleted;
        _progress = new FtpBrowseProgressDialog();
        _progress.FormClosing += Progress_FormClosing;
        _fsLoader.RunWorkerAsync();
        _progress.ShowDialog();
      }
      base.OnShown(e);
    }

    private void Progress_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_progress.Cancelled && (_fsLoader.IsBusy)) _fsLoader.CancelAsync();
    }

    private void FsLoader_DoWork(object sender, DoWorkEventArgs e)
    {
      var rootNode =
      IsNullOrEmpty(_currentPath)
        ? new FtpTreeNode(Connection.Host, Connection.Path, Connection.Port)
        : new FtpTreeNode(Connection.Host, _currentPath, Connection.Port);
      LoadSubNodes(rootNode);
      e.Result = rootNode;
    }

    public void FsLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      DirectoryTree.Nodes.Clear();
      FileList.Items.Clear();
      var rootNode = (FtpTreeNode)e.Result;
      DirectoryTree.Nodes.Add(rootNode);
      rootNode.Expand();
      DirectoryTree.SelectedNode = rootNode;
      _progress.Close();
    }

    private void DirectoryTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {

    }

    private void DirectoryTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {

    }

    private void FileList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (FileList.SelectedItems.Count > 0)
      {
        SelectedFileNameDisplay = FileList.SelectedItems[0].Text;
        //ChooseButton.Enabled = true;
      }
      else
      {
        SelectedFileNameDisplay = Empty;
        //ChooseButton.Enabled = false;
      }
    }

    private void FileList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (FileList.SelectedItems.Count > 0)
      {
        SelectedFileNameDisplay = FileList.SelectedItems[0].Text;
        IsDirectory = FileList.SelectedItems[0].ImageIndex == 0;
        //ChooseButton.Enabled = true;
      }
      else
      {
        SelectedFileNameDisplay = Empty;
        //ChooseButton.Enabled = false;
      }
    }

    private void FileList_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      var list = new List<ListViewItem>();
      var skip = FileList.Items.Cast<ListViewItem>().Any(i => i.Text == "..");

      if (FileList.SelectedItems.Count != 0)
      {

        var lvi = FileList.SelectedItems[0];
        int index = skip ? lvi.Index - 1 : lvi.Index;
        switch (lvi.ImageIndex)
        {
          case 0:
            if (FileList.SelectedItems?[0].Text == "..")
            {
              var parNode = (FtpTreeNode)DirectoryTree.SelectedNode.Parent;
              parNode.Select();
            }
            else
            {
              var parNode = (FtpTreeNode)DirectoryTree.SelectedNode;
              var selectedNode = (FtpTreeNode)parNode.Nodes[index];
              selectedNode.Select();
            }
            break;
          case 1:
            ChooseButton.PerformClick();
            break;
            //}
        }
      }
      else
      {
        SelectedFileNameDisplay = Empty;
      }
    }

    private void DirectoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      e.Node.ImageIndex = 1;
      e.Node.SelectedImageIndex = 1;
    }

    private void DirectoryTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
    {
      e.Node.ImageIndex = 0;
      e.Node.SelectedImageIndex = 0;
    }

    private void DirectoryTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      var clickNode = (FtpTreeNode)e.Node;
      FileList.Items.Clear();
      SelectedPathDisplay = clickNode.FullPath;
      if (clickNode.Files.Count != 0)
      {
        //ChooseButton.Enabled = false;
      }
      foreach (var d in clickNode.Directories)
      {

        FileList.Items.Add(new ListViewItem() { Text = d, ImageIndex = 0 });
        //FileList.Items.Add(d, 0);
      }
      foreach (var f in clickNode.Files)
      {
        if (f != ".")
        {
          FileList.Items.Add(new ListViewItem() { Text = f, ImageIndex = 1 });
          //FileList.Items.Add(f, 1);
        }
      }
      UpDirectoryButton.Enabled = DirectoryTree.SelectedNode != DirectoryTree.Nodes[0];
      SetSubnodeBackColor(DirectoryTree.Nodes[0] as FtpTreeNode, DirectoryTree.BackColor);
      DirectoryTree.SelectedNode.BackColor = Color.LightGray;
    }

    private void LoadNewHostButton_Click(object sender, EventArgs e)
    {
      var newLogin = new LoginDialog(Connection);
      if (newLogin.ShowDialog() != DialogResult.OK) return;

      Connection = newLogin.Connection;
      _fsLoader = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
      _fsLoader.DoWork += FsLoader_DoWork;
      _fsLoader.RunWorkerCompleted += FsLoader_RunWorkerCompleted;
      _progress = new FtpBrowseProgressDialog();
      _progress.FormClosing += Progress_FormClosing;
      _fsLoader.RunWorkerAsync();
      _progress.ShowDialog();
    }

    private void UpDirectoryButton_Click(object sender, EventArgs e)
    {
      if (DirectoryTree.SelectedNode == DirectoryTree.Nodes[0]) return;

      var currentNode = (FtpTreeNode)DirectoryTree.SelectedNode;
      currentNode.Collapse();
      var selectedNode = (FtpTreeNode)DirectoryTree.SelectedNode.Parent;
      selectedNode.Select();
    }

    private void FileList_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\b') UpDirectoryButton.PerformClick();
    }

    private void ChooseButton_Click(object sender, EventArgs e)
    {
      if (FileList.SelectedItems.Count != 0 && IsDirectory)
      {
        FileList_MouseDoubleClick(null, null);
      }
    }
    #endregion
  }
}