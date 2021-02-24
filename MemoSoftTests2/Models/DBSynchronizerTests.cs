using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models.Tests {
    [TestClass()]
    public class DBSynchronizerTests {
        [TestMethod()]
        public void downloadTest() {
            var remoteDB = new PostgreSQLDBHelper();
            var localDB = new DatabaseHelper(DatabaseHelper.DATABASE_NAME_EACH_PC);

            var synchronizer = new DBSynchronizer(remoteDB, localDB);
            synchronizer.download();
        }

        [TestMethod()]
        public void uploadTest() {
            var remoteDB = new PostgreSQLDBHelper();
            var localDB = new DatabaseHelper(DatabaseHelper.DATABASE_NAME_EACH_PC);
            var testComment = new Comment();
            testComment.CreationDateTime = DateTime.Now;
            testComment.TextContent = "test用コメント";
            // localDB.insertComment(testComment);

            var synchronizer = new DBSynchronizer(remoteDB, localDB);
            synchronizer.upload();
        }
    }
}