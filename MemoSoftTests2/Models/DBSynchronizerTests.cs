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
    }
}