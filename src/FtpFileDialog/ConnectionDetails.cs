using System.Net;
using System.Text.RegularExpressions;

namespace FtpFileDialog
{
  public class ConnectionDetails
  {
    private readonly Regex _regexPattern =
      new Regex("[ftp\\:\\/\\/]?([A-ø0-9]*)?[ ]?[\\:]?[ ]?([A-ø0-9]*)?[ ]?[\\@]?[ ]?([A-ø0-9\\.]*)[\\/]?([A-ø0-9]*)?(\\:[0-9]*)?");

    public string Host { get; set; }
    public string StartPath { get; set; }
    public NetworkCredential FtpCred { get; set; }
    public int FtpPort { get; set; }
    public bool Passive { get; set; }

    /// <summary>
    /// Allows for parsing an ftp address with the credentials in the URL.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="passive"></param>
    /// <param name="ftpCred"></param>
    /// <param name="startPath"></param>
    /// <param name="ftpPort"></param>
    public ConnectionDetails(string host, bool passive = false, NetworkCredential ftpCred = null,
      string startPath = null, int? ftpPort = null)
    {
      var match = _regexPattern.Match(host);

      Host = match.Success ? match.Groups[3].Value : host;
      StartPath = startPath ?? match.Groups[4].Value;
      FtpCred = ftpCred ?? new NetworkCredential(match.Groups[1].Value, match.Groups[2].Value);
      Passive = passive;
      FtpPort = ftpPort ?? (match.Success ? int.Parse(match.Groups[5].Value) : 21);
    }

    public ConnectionDetails(string host, string startPath, NetworkCredential ftpCred, int ftpPort = 21, bool passive = false)
    {
      Host = host;
      StartPath = startPath;
      FtpCred = ftpCred;
      FtpPort = ftpPort;
      Passive = passive;
    }
  }
}