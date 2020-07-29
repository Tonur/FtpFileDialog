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
    public FileType Type
    {
      get
      {
        if (ObjectType != 'd') //.Contains("d")
          return FileType.File;
        switch (FileName)
        {
          case ".":
            return FileType.CurrentDirectory;
          case "..":
            return FileType.UpstreamDirectory;
          default:
            return FileType.Directory;
        }
      }
    }

    public char ObjectType { get; set; }
    public string FilePermissions { get; set; }
    public int FileSize { get; set; }
    public DateTime Date { get; set; }
    public string FileName { get; set; }

    private static DateTime _lastDateTime = DateTime.MinValue;
    private static Regex _regex = new Regex(
      @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);


    public static List<FtpResponse> ReadFtpLinesToObject(StreamReader streamReader)
    {
      var result = new List<FtpResponse>();
      var stringLines = streamReader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in stringLines)
      {
        //d--x--x--x    2 ftp ftp       4096        Mar 07  2002          bin
        //-rw-r--r--    1 ftp ftp       659450      Jun 15        05:07   TEST.TXT
        //-rw-r--r--    1 ftp ftp       101786380   Sep 08  2008          TEST03-05.TXT
        //drwxrwxr-x    2 ftp ftp       4096        May 06        12:24   dropoff

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
          ObjectType = lineTokens.Groups[1].Value[0],
          FilePermissions = lineTokens.Groups[2].Value,
          FileSize = int.TryParse(lineTokens.Groups[3].Value, out var size) ? size : 0,
          Date = _lastDateTime = 
            (DateTime.TryParse(lineTokens.Groups[4].Value, out var date) ? date : DateTime.Today)
            .Add(TimeSpan.TryParse("20:20", out var time) ? time : TimeSpan.MinValue),
          FileName = lineTokens.Groups[6].Value
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