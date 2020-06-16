using System;
using System.Globalization;
using System.Windows.Forms;

namespace FtpFileDialog
{
  public partial class FtpBrowseProgressDialog : Form
  {
    public bool Cancelled { get; private set; }

    public FtpBrowseProgressDialog()
    {
      Cancelled = false;
      InitializeComponent();
      LoadingLabel.Text = Properties.Resources.ResourceManager.GetString("ProgressDialog_LoadingLabel", CultureInfo.CurrentUICulture); 
      CancelButton.Text = Properties.Resources.ResourceManager.GetString("Global_CancelButton", CultureInfo.CurrentUICulture);
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      Cancelled = true;
    }

  }
}