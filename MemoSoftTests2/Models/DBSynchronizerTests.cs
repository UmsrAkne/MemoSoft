namespace MemoSoft.Models.Tests
{
    using System;
    using MemoSoft.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DBSynchronizerTests
    {
        [TestMethod]
        public void DownloadTest()
        {
            var remoteDB = new PostgreSQLDBHelper();
            var localDB = new DatabaseHelper(DatabaseHelper.DatabaseNameEachPC);

            var synchronizer = new DBSynchronizer(remoteDB, localDB);
            synchronizer.Download();
        }

        [TestMethod]
        public void UploadTest()
        {
            var remoteDB = new PostgreSQLDBHelper();
            var localDB = new DatabaseHelper(DatabaseHelper.DatabaseNameEachPC);
            var testComment = new Comment();
            testComment.CreationDateTime = DateTime.Now;
            testComment.TextContent = "test用コメント";

            var synchronizer = new DBSynchronizer(remoteDB, localDB);
            synchronizer.Upload();
        }
    }
}