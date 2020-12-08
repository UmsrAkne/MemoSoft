using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MemoSoft.Models.Tests {
    [TestClass()]
    public class NpgsqlExecuterTests {
        [TestMethod()]
        public void executeNonQueryTest() {
            var l = new List<SqlParameter>();
            l.Add(new SqlParameter());
            new NpgsqlExecuter().executeNonQuery("sql", l);
        }
    }
}