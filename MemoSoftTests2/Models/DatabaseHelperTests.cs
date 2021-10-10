using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemoSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoSoft.Models;

namespace MemoSoft.Tests
{
    [TestClass()]
    public class DatabaseHelperTests
    {
        [TestMethod()]
        public void executeNonQueryTest()
        {
            var dbHelper = new DatabaseHelper("testDB");

            var comment = new Comment();
            comment.TextContent = "testText";

            var nextID = 2;

            var sql = $"INSERT INTO {DatabaseHelper.DATABASE_TABLE_NAME} (" +
                      $"{nameof(Comment.ID)}, " +
                      $"{nameof(Comment.CreationDateTime)}," +
                      $"{nameof(Comment.TextContent)}, " +
                      $"{nameof(Comment.RemoteID)}," +
                      $"{nameof(Comment.Uploaded)} )" +
                      $"VALUES (" +
                      $"{nextID}," +
                      $"'{comment.CreationDateTime}', " +
                      $"'{comment.TextContent}', " +
                      $"{comment.RemoteID}," +
                      $"'{comment.Uploaded}'" +
                      $");";

            dbHelper.executeNonQuery(sql);
        }

        [TestMethod()]
        public void selectTest()
        {
            var dbHelper = new DatabaseHelper("testDB");

            var sql = $"select count(*) as count from {DatabaseHelper.DATABASE_TABLE_NAME};";
            var hashs = dbHelper.select(sql);
            System.Diagnostics.Debug.WriteLine(hashs);
        }

        [TestMethod()]
        public void insertCommentTest()
        {
            var dbHelper = new DatabaseHelper("testDB");
            dbHelper.insertComment(new Comment());
        }

        [TestMethod()]
        public void updateTest()
        {
            var dbHelper = new DatabaseHelper("testDB");
            var comment = new Comment();

            dbHelper.insertComment(comment);

            comment.CreationDateTime = DateTime.Now;
            comment.TextContent = "updateTest";
            dbHelper.update(comment);
        }
    }
}