using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FtpFileDialog
{
  public class ConnectionDetails
  {
    public string Host { get; set; }
    public NetworkCredential FtpCred { get; set; }
    public string StartPath { get; set; }
    public int FtpPort { get; set; }
    public bool Passive { get; set; }
  }
}
