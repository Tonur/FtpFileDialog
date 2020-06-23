using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FtpFileDialog;

namespace WindowsFormsApp1
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      var ting = new BrowseDialog();
      if (ting.ShowDialog() == DialogResult.OK)
      {
        var test = ting.SelectedFileNameUriEncoded;
      }
    }
  }
}
