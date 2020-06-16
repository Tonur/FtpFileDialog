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
    public string FilePermissions { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string GroupName { get; set; }
    public int GroupNumber { get; set; }
    public DateTime Date { get; set; }
    public string FileName { get; set; }

    private static DateTime _lastDateTime = DateTime.MinValue;

    public static List<FtpResponse> ReadFtpLinesToObject(StreamReader streamReader)
    {
      var result = new List<FtpResponse>();
      var stringLines = streamReader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in stringLines)
      {
        var lineTokens = Regex.Match(line,
@"([-a-zA-z]*)[\s]*([0-9]*)[\s]*([a-zA-Z]*)[\s]*([a-zA-Z]*)[\s]*([0-9]*)[\s]*([a-zA-Z0-9]*)[\s]*([a-zA-Z0-9]*)[\s]*([a-zA-Z0-9\S]*)[\s]*([a-øA-Ø0-9 \.]*)");
        var ftpResponse = new FtpResponse()
        {
          FilePermissions = lineTokens.Groups[1].Value,
          UserId = Convert.ToInt32(lineTokens.Groups[2].Value),
          UserName = lineTokens.Groups[3].Value,
          GroupName = lineTokens.Groups[4].Value,
          GroupNumber = Convert.ToInt32(lineTokens.Groups[5].Value),
          Date = _lastDateTime =
            DateTime.TryParse($"{lineTokens.Groups[6].Value} {lineTokens.Groups[7].Value} {lineTokens.Groups[8].Value}", out var date1) 
              ? date1
              : DateTime.TryParse($"{lineTokens.Groups[6].Value} {lineTokens.Groups[7].Value} {_lastDateTime.Year}", out var date2)
                ? date2
                : _lastDateTime,
          FileName = string.Join("", lineTokens.Groups[9].Value)
        };
        result.Add(ftpResponse);
      }
      return result;
    }
  }
}