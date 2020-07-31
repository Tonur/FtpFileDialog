using System.Net;
using System.Text.RegularExpressions;

namespace FtpFileDialog
{
  public class ConnectionDetails
  {
    public static readonly Regex FtpAddressPattern =
      new Regex(@"(?:ftp\:\/\/)+(?<Username>[^\:]+)\s*\:\s*(?<Password>[^\@]+)\s*\@\s*(?<Host>[^\/\:]*)\:?(?<Port>[\d]+)?\/?(?<Path>.*)?");

    public string Host { get; set; }
    public string Path { get; set; }
    public NetworkCredential FtpCred { get; set; }
    public int Port { get; set; }
    public bool Passive { get; set; }
    public string BaseAddress => $@"ftp://{Host}";

    /// <summary>
    /// Allows for parsing an ftp address with the credentials in the URL.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="passive"></param>
    /// <param name="ftpCred"></param>
    /// <param name="path"></param>
    /// <param name="port"></param>
    public ConnectionDetails(string host, bool passive = false, NetworkCredential ftpCred = null,
      string path = null, int port = 21)
    {
      var match = FtpAddressPattern.Match(host);

      Host = match.Success ? match.Groups["Host"].Value : host;
      Path = match.Success ? match.Groups["Path"].Value : path;
      FtpCred = match.Success ? new NetworkCredential(match.Groups["Username"].Value, match.Groups["Password"].Value) : ftpCred;
      Passive = passive;
      Port = match.Success ? int.TryParse(match.Groups["Port"].Value, out var result) ? result : 21 : port;
    }

    public ConnectionDetails(string host, string path, NetworkCredential ftpCred, int port = 21, bool passive = false)
    {
      Host = host;
      Path = path;
      FtpCred = ftpCred;
      Port = port;
      Passive = passive;
    }
  }
}