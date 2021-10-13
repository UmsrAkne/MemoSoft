namespace MemoSoft
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Linq;
    using MemoSoft.Models;

    public class DatabaseHelper : IDBHelper
    {
        public static readonly string DatabaseNameEachPC = Environment.MachineName + "_" + Environment.UserName;
        public static readonly string DatabaesTableName = "diary";
        public static readonly string DatabaseColumnNameDate = "date";
        public static readonly string DabataseColumnNameText = "text";

        private string dbfileName;

        private List<Comment> commentList;

        public DatabaseHelper(string dbfileName)
        {
            this.dbfileName = dbfileName;

            // テーブルの作成。IF NOT EXISTS を入れてあるので、テーブルが存在しない初回のみ実行する。
            using (var con = new SQLiteConnection(DataSourceSyntax))
            {
                con.Open();
                var sql = $"CREATE TABLE IF NOT EXISTS {DatabaesTableName} (" +
                          $"{nameof(Comment.ID)} INTEGER PRIMARY KEY," +
                          $"{nameof(Comment.CreationDateTime)} TEXT NOT NULL," +
                          $"{nameof(Comment.Uploaded)} BOOLEAN NOT NULL," +
                          $"{nameof(Comment.RemoteID)} INTEGER NOT NULL," +
                          $"{nameof(Comment.TextContent)} TEXT NOT NULL);";

                new SQLiteCommand(sql, con).ExecuteNonQuery();
            }

            string dt = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            SystemMessage = $"{dt} ローカルサーバーに接続しました";
        }

        public long Count { get; } = 0;

        public bool Connected { get; private set; } = true;

        public string SystemMessage { get; set; }

        public List<Comment> CommentList
        {
            get { return this.commentList; }
            private set { commentList = value; }
        }

        private string DataSourceSyntax => $"Data Source={dbfileName}.sqlite";

        public void CreateDatabase()
        {
            using (var conn = new SQLiteConnection("Data Source=" + dbfileName + ".sqlite"))
            {
                conn.Open();
                conn.Close();
            }
        }

        public void CreateTable(string tableName, string[] columnDatas)
        {
            using (var connection = new SQLiteConnection("Data Source=" + dbfileName + ".sqlite"))
            {
                connection.Open();
                var sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (";
                for (int i = 0; i < columnDatas.Length; i++)
                {
                    if (i != columnDatas.Length - 1)
                    {
                        sql += columnDatas[i] + ", ";
                    }
                    else
                    {
                        sql += columnDatas[i] + ")";
                    }
                }

                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public void InsertData(string tableName, string[] columnNames, string[] values)
        {
            using (var connection = new SQLiteConnection("Data Source=" + dbfileName + ".sqlite"))
            {
                connection.Open();
                var sql = "INSERT INTO " + tableName + " (";
                for (int i = 0; i < columnNames.Length; i++)
                {
                    if (i == columnNames.Length - 1)
                    {
                        sql += columnNames[i] + ")";
                    }
                    else
                    {
                        sql += columnNames[i] + ", ";
                    }
                }

                sql += " values (";

                for (int i = 0; i < values.Length; i++)
                {
                    if (i == values.Length - 1)
                    {
                        sql += "'" + values[i] + "'" + ");";
                    }
                    else
                    {
                        sql += values[i] + ", ";
                    }
                }

                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public void Select()
        {
            using (var connection = new SQLiteConnection("Data Source=" + dbfileName + ".sqlite"))
            {
                connection.Open();
                string sql = "select * from " + DatabaesTableName;

                SQLiteCommand com = new SQLiteCommand(sql, connection);
                SQLiteDataReader sdr = com.ExecuteReader();
                CommentList = new List<Comment>();

                while (sdr.Read() == true)
                {
                    var comment = new Comment();
                    comment.TextContent = (string)sdr[nameof(Comment.TextContent)];
                    DateTime resultD;

                    if (DateTime.TryParseExact(sdr[nameof(Comment.CreationDateTime)].ToString(), "yyyyMMddHHmmssff", null, DateTimeStyles.AllowWhiteSpaces, out resultD))
                    {
                        comment.CreationDateTime = resultD;
                    }

                    CommentList.Add(comment);
                }
            }
        }

        public void Update(Comment comment)
        {
            var sql = $"UPDATE {DatabaesTableName} " +
                      $"SET " +
                      $"{nameof(Comment.Uploaded)} = '{comment.Uploaded}'," +
                      $"{nameof(Comment.TextContent)} = '{comment.TextContent}'," +
                      $"{nameof(Comment.CreationDateTime)} = '{comment.CreationDateTime}'," +
                      $"{nameof(Comment.RemoteID)} = {comment.RemoteID} " +
                      $"WHERE " +
                      $"{nameof(Comment.ID)} = {comment.ID}";

            ExecuteNonQuery(sql);
        }

        public void GetLastUpdateRow()
        {
            using (var connection = new SQLiteConnection("Data Source=" + dbfileName + ".sqlite"))
            {
                connection.Open();

                string sql = "select * from diary where date = (select max(date) from diary)";
                SQLiteCommand com = new SQLiteCommand(sql, connection);
                SQLiteDataReader sdr = com.ExecuteReader();

                CommentList = new List<Comment>();
                var comment = new Comment();

                if (sdr.Read())
                {
                    comment.TextContent = (string)sdr[DabataseColumnNameText];
                    DateTime resultD;

                    if (DateTime.TryParseExact(sdr[DatabaseColumnNameDate].ToString(), "yyyyMMddHHmmssff", null, DateTimeStyles.AllowWhiteSpaces, out resultD))
                    {
                        comment.CreationDateTime = resultD;
                    }

                    CommentList.Add(comment);
                }

                sdr.Close();
            }
        }

        public List<Comment> LoadComments()
        {
            var sql = $"SELECT * FROM {DatabaesTableName} ORDER BY {nameof(Comment.CreationDateTime)} DESC;";
            var hashs = Select(sql);

            var commentList = new List<Comment>();

            hashs.ForEach(h =>
            {
                var c = Comment.ToComment(h);
                commentList.Add(c);
            });

            return commentList;
        }

        public void InsertComment(Comment comment)
        {
            long nextID = 0;

            if (GetRecordCount() != 0)
            {
                nextID = GetMAXID() + 1;
            }

            var tableName = DatabaesTableName;
            var sql = $"INSERT INTO {tableName} (" +
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

            ExecuteNonQuery(sql);
        }

        public long GetMaxRemoteID()
        {
            if (GetRecordCount() == 0)
            {
                return -1;
            }

            var sql = $"SELECT MAX({nameof(Comment.RemoteID)}) AS MAX FROM {DatabaesTableName};";
            return (long)Select(sql).First()["MAX"];
        }

        public void ExecuteNonQuery(string sql)
        {
            using (var con = new SQLiteConnection(DataSourceSyntax))
            {
                con.Open();
                new SQLiteCommand(sql, con).ExecuteNonQuery();
            }
        }

        public List<Hashtable> Select(string sql)
        {
            using (var con = new SQLiteConnection(DataSourceSyntax))
            {
                con.Open();
                SQLiteDataReader sdr = new SQLiteCommand(sql, con).ExecuteReader();
                var resultList = new List<Hashtable>();

                while (sdr.Read())
                {
                    var hashTable = new Hashtable();
                    for (var i = 0; i < sdr.FieldCount; i++)
                    {
                        hashTable[sdr.GetName(i)] = sdr.GetValue(i);
                    }

                    resultList.Add(hashTable);
                }

                sdr.Close();
                return resultList;
            }
        }

        private long GetMAXID()
        {
            var sql = $"SELECT MAX({nameof(Comment.ID)}) AS MAX FROM {DatabaesTableName};)";
            return (long)Select(sql).First()["MAX"];
        }

        private long GetRecordCount()
        {
            var sql = $"SELECT COUNT(*) AS COUNT FROM {DatabaesTableName};";
            var h = Select(sql).First();
            return (long)Select(sql).First()["COUNT"];
        }
    }
}
