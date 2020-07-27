using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FtpFileDialog
{
  public partial class LoginDialog : Form
  {
    #region Public Properties
    public string Server
    {
      get => ServerTextBox.Text;
      set => ServerTextBox.Text = value;
    }
    public string StartPath
    {
      get => StartPathTextBox.Text;
      set => StartPathTextBox.Text = value;
    }
    public string Username
    {
      get => UsernameTextBox.Text;
      set => UsernameTextBox.Text = value;
    }
    public string Password
    {
      get => PasswordTextBox.Text;
      set => PasswordTextBox.Text = value;
    }
    public int Port
    {
      get => Convert.ToInt32(PortNumeric.Value);
      set => PortNumeric.Value = new decimal(value);
    }
    public bool PassiveMode
    {
      get => PassiveCheckbox.Checked;
      set => PassiveCheckbox.Checked = value;
    }

    public ConnectionDetails Connection { get; set; }
    #endregion

    #region Constructors
    public LoginDialog() 
      : this(string.Empty, string.Empty, 21, string.Empty, string.Empty, true)
    {
    }

    public LoginDialog(string host, string path, int port, string username, string password, bool passiveMode) 
      : this(new ConnectionDetails(host, path, new NetworkCredential(username, password), port, passiveMode))
    {
    }

    public LoginDialog(ConnectionDetails connectionDetails)
    {
      InitializeComponent();
      Server = string.IsNullOrEmpty(connectionDetails.StartPath) 
        ? connectionDetails.Host 
        : $"{connectionDetails.Host}/{connectionDetails.StartPath}";
      Port = connectionDetails.FtpPort;
      Username = connectionDetails.FtpCred.UserName;
      Password = connectionDetails.FtpCred.Password;
      PassiveMode = connectionDetails.Passive;

      SetupLocalization();
    }

    private void SetupLocalization()
    {
      ServerLabel.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_ServerLabel", CultureInfo.CurrentUICulture);
      StartPathLabel.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_StartPathLabel", CultureInfo.CurrentUICulture);
      PortLabel.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_PortLabel", CultureInfo.CurrentUICulture);
      UsernameLabel.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_UsernameLabel", CultureInfo.CurrentUICulture);
      PasswordLabel.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_PasswordLabel", CultureInfo.CurrentUICulture);
      PassiveCheckbox.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_PassiveCheckBox", CultureInfo.CurrentUICulture);
      CancelButton.Text = Properties.Resources.ResourceManager.GetString("Global_CancelButton", CultureInfo.CurrentUICulture);
      ConnectButton.Text = Properties.Resources.ResourceManager.GetString("DialogLogin_ConnectButton", CultureInfo.CurrentUICulture);
    }
    #endregion

    #region Event Handlers
    private void input_TextChanged(object sender, EventArgs e)
    {
      ConnectButton.Enabled = ValidateForm();
    }

    private void FtpFileDialogLogin_Load(object sender, EventArgs e)
    {
      ConnectButton.Enabled = ValidateForm();
    }
    #endregion
    private bool ValidateForm()
    {
      var addressStringMatches = Regex.Match(Server, @"ftp\:\/\/([a-zA-Z1-9]*)[ ]?\:[ ]?([a-zA-Z1-9]*)[ ]?\@[ ]?([a-zA-Z1-9\.]*)[\/]?");

      if (addressStringMatches.Success)
      {
        return true;
      }
      if (Uri.TryCreate("ftp://" + Server, UriKind.Absolute, out _) == false)
      {
        return false;
      }
      if (UsernameTextBox.Text == string.Empty)
      {
        return false;
      }
      if (PasswordTextBox.Text == string.Empty)
      {
        return false;
      }
      return true;
    }

    private void ConnectButton_Click(object sender, EventArgs e)
    {
      Connection = new ConnectionDetails(Server, PassiveCheckbox.Checked);
    }
  }
}