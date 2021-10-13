namespace MemoSoft.Tests
{
    using System;
    using MemoSoft;
    using MemoSoft.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DatabaseHelperTests
    {
        [TestMethod]
        public void ExecuteNonQueryTest()
        {
            var dbhelper = new DatabaseHelper("testDB");

            var comment = new Comment();
            comment.TextContent = "testText";

            var nextID = 2;

            var sql = $"INSERT INTO {DatabaseHelper.DatabaesTableName} (" +
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

            dbhelper.ExecuteNonQuery(sql);
        }

        [TestMethod]
        public void SelectTest()
        {
            var dbhelper = new DatabaseHelper("testDB");

            var sql = $"select count(*) as count from {DatabaseHelper.DatabaesTableName};";
            var hashs = dbhelper.Select(sql);
            System.Diagnostics.Debug.WriteLine(hashs);
        }

        [TestMethod]
        public void InsertCommentTest()
        {
            var dbhelper = new DatabaseHelper("testDB");
            dbhelper.InsertComment(new Comment());
        }

        [TestMethod]
        public void UpdateTest()
        {
            var dbhelper = new DatabaseHelper("testDB");
            var comment = new Comment();

            dbhelper.InsertComment(comment);

            comment.CreationDateTime = DateTime.Now;
            comment.TextContent = "updateTest";
            dbhelper.Update(comment);
        }
    }
}