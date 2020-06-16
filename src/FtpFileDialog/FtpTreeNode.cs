using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace FtpFileDialog
{
  public class FtpTreeNode : TreeNode
  {
    #region Public Properties
    public StringCollection Files { get; }
    public StringCollection Directories { get; }
    public string Server { get; }

    public string Path { get; }
    public int Port { get; }

    public string Directory
    {
      get
      {
        var pathTokens = Path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        return pathTokens[pathTokens.Length - 1];
      }
    }
    public string FtpPath => ("ftp://" + Server + "/" + Path).TrimEnd('/');

    #endregion
    #region Constructor
    public FtpTreeNode(string hostUrl, string startPath, int ftpPort)
    {
      Files = new StringCollection();
      Directories = new StringCollection();
      Server = hostUrl.TrimEnd('/');
      Path = startPath.TrimStart('/').TrimEnd('/');
      Port = ftpPort;
      Text = FtpPath;
    }
    #endregion


    #region Public Methods
    public int AddFile(string file)
    {
      return Files.Add(file);
    }
    public int AddDirectory(string dir)
    {
      return Directories.Add(dir);
    }
    public void Select()
    {
      TreeView.SelectedNode = this;
      this.EnsureVisible();
    }
    #endregion
  }
}
