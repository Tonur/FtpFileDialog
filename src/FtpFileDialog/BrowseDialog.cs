using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.String;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

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

    public string BaseAddress { get; set; }

    /// <summary>
    /// Returns the selected file full path
    /// Example ftp://test.com/FromAcubiz/regnskab2000/regnskab254.csv
    /// </summary>
    public string SelectedFile => SelectedPath + "/" + SelectedFileName + (IsDirectory ? "/" : "");

    /// <summary>
    /// 
    /// Example /FromAcubiz/regnskab2000/
    /// </summary>
    public string SelectedPath { get; set; }
    
    /// <summary>
    /// Returns the selected file full path encoded as to escape uri unfriendly characters
    /// Example ftp://test.com/FromAAge/regnskabs%E5r2000/
    /// </summary>
    public string SelectedPathUriEncoded => BaseAddress + RelativePathUriEncoded;

    /// <summary>
    /// 
    /// Example regnskab254.csv
    /// </summary>
    public string SelectedFileName { get; private set; } 

    /// <summary>
    /// 
    /// Example regnskabops%E6tning254.csv
    /// </summary>
    public string SelectedFileNameUriEncoded => Uri.EscapeUriString(SelectedFileName) + (IsDirectory ? "/" : "");

    /// <summary>
    /// 
    /// Example ftp://test.com/FromAAge/regnskabs%E5r2000/regnskabops%E6tning254.csv
    /// </summary>
    public string SelectedFileUriEncoded => SelectedPathUriEncoded + SelectedFileNameUriEncoded;

    /// <summary>
    /// 
    /// Example /FromAcubiz/regnskab2000/regnskab254.csv
    /// </summary>
    public string RelativeFile => SelectedFile.Replace(Connection.Host, "").Replace("ftp://", "");

    /// <summary>
    /// 
    /// Example /FromAAge/regnskabs%E5r2000/regnskabops%E6tning254.csv
    /// </summary>
    public string RelativeFileUriEncoded => SelectedPath.Replace(Connection.Host, "") + "/" + SelectedFileNameUriEncoded;

    /// <summary>
    /// 
    /// Example /FromAcubiz/regnskab2000/
    /// </summary>
    public string RelativePath => SelectedPath.Replace(Connection.Host, "");

    /// <summary>
    /// 
    /// Example /FromAAge/regnskabs%E5r2000/
    /// </summary>
    public string RelativePathUriEncoded => Uri.EscapeUriString(SelectedPath.Replace(Connection.Host, "").Replace("ftp://", "")) + "/";

    /// <summary>
    /// Returns the relative path or file, depending on if a file has been selected
    /// Example ftp://test.com/From%E5ge/regnskabs%E5r2000/ or ftp://test.com/From%E5ge/regnskabs%E5r2000/regnskabsops%E6tning254.csv
    /// </summary>
    public string DisplayShortName => string.IsNullOrWhiteSpace(SelectedFileName) ? RelativePath : SelectedFileName;

    /// <summary>
    /// Returns the whole path or whole file, depending on if a file has been selected
    /// Example ftp://test.com/From%E5ge/regnskabs%E5r2000/ or ftp://test.com/From%E5ge/regnskabs%E5r2000/regnskabsops%E6tning254.csv
    /// </summary>
    public string DisplayLongName => string.IsNullOrWhiteSpace(SelectedFileName) ? SelectedPath : SelectedFile;

    public bool IsDirectory { get; set; } = true;

    private string _currentPath;

    #endregion

    #region Constructors
    public BrowseDialog(ConnectionDetails connectionDetails, bool? promptForServer = null, CultureInfo cultureInfo = null)
    {
      InitializeComponent();
      if (cultureInfo != null)
      {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
      }

      Connection = connectionDetails ?? new ConnectionDetails
      {
        Host = Empty,
        StartPath = Empty,
        FtpPort = 21,
        FtpCred = new NetworkCredential(Empty, Empty),
        Passive = true,
      };

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
      _promptForServer = promptForServer ?? true;

      SetupLocalization();
    }

    private void SetupLocalization()
    {
      CancelButton.Text = Properties.Resources.ResourceManager.GetString("Global_CancelButton", CultureInfo.CurrentUICulture);
      ChooseButton.Text = Properties.Resources.ResourceManager.GetString("BrowseDialog_ChooseButton", CultureInfo.CurrentUICulture);
      UpDirectoryButton.ToolTipText = Properties.Resources.ResourceManager.GetString("BrowseDialog_UpDirectoryButton", CultureInfo.CurrentUICulture);
      LoadNewHostButton.ToolTipText = Properties.Resources.ResourceManager.GetString("BrowseDialog_LoadNewHostButton", CultureInfo.CurrentUICulture);
    }

    public BrowseDialog(string hostUrl, string path, int port, string username, string password, bool passiveMode,
      bool? promptForServer = null)
      : this(
        new ConnectionDetails
        {
          Host = hostUrl,
          StartPath = path,
          FtpPort = port,
          FtpCred = new NetworkCredential(username, password),
          Passive = passiveMode,
        }, false)
    {

    }

    public BrowseDialog(bool? promptForServer = true)
      : this(
        new ConnectionDetails
        {
          Host = Empty,
          StartPath = Empty,
          FtpPort = 21,
          FtpCred = new NetworkCredential(Empty, Empty),
          Passive = true,
        }, promptForServer)
    {

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
      var ub = rootNode.Path != Empty
        ? new UriBuilder("ftp", rootNode.Server, rootNode.Port, rootNode.Path)
        : new UriBuilder("ftp", rootNode.Server, rootNode.Port);
      var uriString = ub.Uri.OriginalString;
      _req = (FtpWebRequest)WebRequest.Create(ub.Uri);
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
          if (response.FilePermissions.StartsWith("d"))
          {
            if (response.FileName == ".")
              continue;
            var subnode = new FtpTreeNode(rootNode.Server,
              rootNode.Path + "/" + response.FileName, rootNode.Port);
            subnode.Text = subnode.Directory;
            if (response.FileName != "..")
            {
              rootNode.Nodes.Add(subnode);
              LoadSubNodes(subnode);
            }
            rootNode.AddDirectory(response.FileName);
          }
          else
          {
            rootNode.AddFile(response.FileName);
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
        ? new FtpTreeNode(Connection.Host, Connection.StartPath, Connection.FtpPort)
        : new FtpTreeNode(Connection.Host, _currentPath, Connection.FtpPort);
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
        SelectedFileName = FileList.SelectedItems[0].Text;
        //ChooseButton.Enabled = true;
      }
      else
      {
        SelectedFileName = Empty;
        //ChooseButton.Enabled = false;
      }
    }

    private void FileList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (FileList.SelectedItems.Count > 0)
      {
        SelectedFileName = FileList.SelectedItems[0].Text;
        IsDirectory = FileList.SelectedItems[0].ImageIndex == 0;
        //ChooseButton.Enabled = true;
      }
      else
      {
        SelectedFileName = Empty;
        //ChooseButton.Enabled = false;
      }
    }

    private void FileList_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      var list = new List<ListViewItem>();
      var skip = FileList.Items.Cast<ListViewItem>().Any(i => i.Text == "..");

      var lvi = FileList.SelectedItems?[0];
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
      SelectedPath = clickNode.FullPath;
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

      var addressStringMatches =
        Regex.Match(newLogin.Server,
        @"ftp\:\/\/([a-zA-Z1-9]*)[ ]?\:[ ]?([a-zA-Z1-9]*)[ ]?\@[ ]?([a-zA-Z1-9\.]*)[\/]?([a-zA-Z1-9]*)?");


      var serverAddressParse = Regex.Match(newLogin.Server, @"[ ]?([a-zA-Z1-9\.]*)[\/]?([a-zA-Z1-9/]*)?");

      BaseAddress = @"ftp://" +
                    (addressStringMatches.Success
                      ? addressStringMatches.Groups[3].Value
                      : serverAddressParse.Groups[1].Value);

      Connection = new ConnectionDetails
      {
        FtpCred = addressStringMatches.Success
          ? new NetworkCredential(addressStringMatches.Groups[1].Value, addressStringMatches.Groups[2].Value)
          : new NetworkCredential(newLogin.Username, newLogin.Password),
        Host = addressStringMatches.Success ? addressStringMatches.Groups[3].Value : serverAddressParse.Groups[1].Value,
        Passive = newLogin.PassiveMode,
        FtpPort = newLogin.Port,
        StartPath = SelectedPath = !IsNullOrEmpty(newLogin.StartPath)
          ? newLogin.StartPath
          : _currentPath = addressStringMatches.Success
            ? addressStringMatches.Groups[4].Value
            : serverAddressParse.Groups[2].Value
      };
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
      if (SelectedFileName != Empty && FileList.SelectedItems.Count <= 0)
        FileList_MouseDoubleClick(null, null);
    }
    #endregion
  }
}