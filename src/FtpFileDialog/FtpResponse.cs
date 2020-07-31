using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FtpFileDialog
{
  public class FtpResponse
  {
    public FileType ItemType
    {
      get
      {
        if (FileChar != 'd') //.Contains("d")
          return FtpFileDialog.FileType.File;
        switch (FileName)
        {
          case ".":
            return FtpFileDialog.FileType.CurrentDirectory;
          case "..":
            return FtpFileDialog.FileType.UpstreamDirectory;
          default:
            return FtpFileDialog.FileType.Directory;
        }
      }
    }

    public char FileChar { get; set; }
    public string FilePermissions { get; set; }
    public int FileSize { get; set; }
    public DateTime Date { get; set; }
    public string FileName { get; set; }
    public string FileType => ItemType == FtpFileDialog.FileType.File ? FileName.Split('.').LastOrDefault() : string.Empty;

    public string PrettyName => FileName.Replace(FileType, "");

    private static DateTime _lastDateTime = DateTime.Today;

    private static Regex _regex = new Regex(
      @"^ (?<FileType>[d-])
                (?<FilePermission>[rwxt-]{3}){3}\s+\d{1,}\s+
                (?<User>[\w]*)?\s+
                (?<Group>[\w]*)?\s+
                (?<FileSize>\d{1,})\s+
                (?<Date>\w+\s+\d{1,2}\s+(?:\d{4})?)
                (?<Time>\d{1,2}:\d{2})?\s+
                (?<FileName>.+?)\s?$",
    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);


    public static List<FtpResponse> ReadFtpLinesToObject(StreamReader streamReader)
    {
      var result = new List<FtpResponse>();
      var stringLines = streamReader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in stringLines)
      {
        //drwxrwxrwx    1 user  group     0           Feb 27  2019          FromAcubiz
        //drwxrwxrwx    1 user  group     0           Jan 29  2019          ToAcubiz
        //d--x--x--x    2 ftp   ftp       4096        Mar 07  2002          bin
        //-rw-r--r--    1 ftp   ftp       659450      Jun 15        05:07   TEST.TXT
        //-rw-r--r--    1 ftp   ftp       101786380   Sep 08  2008          TEST03-05.TXT
        //drwxrwxr-x    2 ftp   ftp       4096        May 06        12:24   dropoff

        var lineTokens = _regex.Match(line);

        //var dateCollection = lineTokens.Groups[4].Value.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
        //var timeCollection = lineTokens.Groups[5].Value.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

        //int.TryParse(dateCollection[2], out var year);
        //Enum.TryParse(dateCollection[0], out Month month);
        //int.TryParse(dateCollection[1], out var day);
        //int.TryParse(timeCollection[0], out var hour);
        //int.TryParse(timeCollection[1], out var minute);

        var ftpResponse = new FtpResponse()
        {
          FileChar = lineTokens.Groups["FileType"].Value.FirstOrDefault(),
          FilePermissions = lineTokens.Groups["FilePermission"].Value,
          FileSize = int.TryParse(lineTokens.Groups["FileSize"].Value, out var size) ? size : 0,
          Date = _lastDateTime = 
            (DateTime.TryParse(lineTokens.Groups["Date"].Value, out var date) ? date : _lastDateTime)
            .Add(TimeSpan.TryParse(lineTokens.Groups["Time"].Value, out var time) ? time : TimeSpan.Zero),
          FileName = lineTokens.Groups["FileName"].Value
        };
        result.Add(ftpResponse);
      }
      return result;
    }
  }

  public enum FileType
  {
    File,
    Directory,
    CurrentDirectory,
    UpstreamDirectory
  }

  public enum Month
  {
    Jan = 1,
    Feb = 2,
    Mar = 3,
    Apr = 4,
    May = 5,
    Jun = 6,
    Jul = 7,
    Aug = 8,
    Sep = 9,
    Oct = 10,
    Nov = 11,
    Dec = 12
  }
}