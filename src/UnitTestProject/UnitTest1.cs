using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void FtpFileDialog_ConnectionDetails_UseFtpPathURL()
    {
      var ftpCred = new FtpFileDialog.ConnectionDetails("ftp://AdvosysDK2:9vgnJRK2fGF8@test.acubizems.com/");
    }
  }
}
