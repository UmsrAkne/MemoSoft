﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using MemoSoft.Models;

namespace MemoSoft
{
    public class DatabaseHelper : IDBHelper
    {
        public static readonly String DATABASE_NAME_EACH_PC =
            Environment.MachineName + "_" + Environment.UserName;
        public static readonly String DATABASE_TABLE_NAME = "diary";
        public static readonly String DATABASE_COLUMN_NAME_DATE = "date";
        public static readonly String DATABASE_COLUMN_NAME_TEXT = "text";

        private String dbFileName;
        private String DataSourceSyntax => $"Data Source={dbFileName}.sqlite";

        private List<Comment> commentList;
        public List<Comment> CommentList {
            get { return this.commentList; }
            private set { commentList = value; }
        }

        public DatabaseHelper(string dbFileName) {
            this.dbFileName = dbFileName;

            // テーブルの作成。IF NOT EXISTS を入れてあるので、テーブルが存在しない初回のみ実行する。
            using (var con = new SQLiteConnection(DataSourceSyntax)) {
                con.Open();
                var sql = $"CREATE TABLE IF NOT EXISTS {DATABASE_TABLE_NAME} (" +
                          $"{nameof(Comment.ID)} INTEGER PRIMARY KEY," +
                          $"{nameof(Comment.CreationDateTime)} TEXT NOT NULL," +
                          $"{nameof(Comment.Uploaded)} BOOLEAN NOT NULL," +
                          $"{nameof(Comment.RemoteID)} INTEGER NOT NULL," +
                          $"{nameof(Comment.TextContent)} TEXT NOT NULL);";

                new SQLiteCommand(sql, con).ExecuteNonQuery();
            }
        }

        public void createDatabase() {
            using (var conn = new SQLiteConnection("Data Source=" + dbFileName + ".sqlite")) {
                conn.Open();
                conn.Close();
            }
        }

        public void createTable(String tableName,String[] columnDatas) {
            using (var connection = new SQLiteConnection("Data Source=" + dbFileName + ".sqlite")) {
                connection.Open();
                var sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (";
                for(int i = 0; i < columnDatas.Length; i++) {
                    if (i != columnDatas.Length - 1) sql += columnDatas[i] + ", ";
                    else sql += columnDatas[i] + ")";
                }

                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public void insertData(String tableName, String[] columnNames, String[] values) {
            using (var connection = new SQLiteConnection("Data Source=" + dbFileName + ".sqlite")) {
                connection.Open();
                var sql = "INSERT INTO " + tableName + " (";
                for (int i = 0; i < columnNames.Length; i++) {
                    if (i == columnNames.Length - 1) sql += columnNames[i] + ")";
                    else sql += columnNames[i] + ", ";
                }

                sql += " values (";

                for (int i = 0; i < values.Length; i++) {
                    if (i == values.Length - 1) sql += "'" + values[i] + "'" + ");";
                    else sql += values[i] + ", ";
                }
                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public void select() {
            using (var connection = new SQLiteConnection("Data Source=" + dbFileName + ".sqlite")) {
                connection.Open();
                string sql = "select * from " + DATABASE_TABLE_NAME;

                SQLiteCommand com = new SQLiteCommand(sql, connection);
                SQLiteDataReader sdr = com.ExecuteReader();
                CommentList = new List<Comment>();

                while (sdr.Read() == true) {
                    var comment = new Comment();
                    comment.TextContent = (String)sdr[nameof(Comment.TextContent)];
                    DateTime resultD;

                    if(
                        DateTime.TryParseExact(sdr[nameof(Comment.CreationDateTime)].ToString(), "yyyyMMddHHmmssff", null,
                        DateTimeStyles.AllowWhiteSpaces, out resultD)) {
                        comment.CreationDateTime = resultD;
                    }
                    CommentList.Add(comment);
                }
            }
        }

        public void getLastUpdateRow() {
            using (var connection = new SQLiteConnection("Data Source=" + dbFileName + ".sqlite")) {
                connection.Open();

                string sql = "select * from diary where date = (select max(date) from diary)";
                SQLiteCommand com = new SQLiteCommand(sql, connection);
                SQLiteDataReader sdr = com.ExecuteReader();

                CommentList = new List<Comment>();
                var comment = new Comment();

                if(sdr.Read()) {
                    comment.TextContent = (String)sdr[DATABASE_COLUMN_NAME_TEXT];
                    DateTime resultD;

                    if(DateTime.TryParseExact(sdr[DATABASE_COLUMN_NAME_DATE].ToString(),"yyyyMMddHHmmssff",null,
                        DateTimeStyles.AllowWhiteSpaces, out resultD)) {
                        comment.CreationDateTime = resultD;
                    }

                    CommentList.Add(comment);
                }

                sdr.Close();
            }
        }

        public List<Comment> loadComments() {
            select();
            return CommentList;
        }

        public void insertComment(Comment comment) {
            var nextID = 0;
            if(getRecordCount() != 0) {
                nextID = getMAXID() + 1;
            }

            var tableName = DATABASE_TABLE_NAME;
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

            using (var con = new SQLiteConnection(DataSourceSyntax)) {
                con.Open();
                var command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
            }
        }

        private int getMAXID() {
            using (var con = new SQLiteConnection(DataSourceSyntax)) {
                con.Open();
                var sql = $"SELECT MAX({nameof(Comment.ID)}) FROM {DATABASE_TABLE_NAME};";
                var command = new SQLiteCommand(sql, con);
                SQLiteDataReader sdr = command.ExecuteReader();

                var count = 0;

                if (sdr.Read()) {
                    count = (int)sdr["MAX"];
                }

                sdr.Close();
                return count;
            }
        }

        private int getRecordCount() {
            using (var con = new SQLiteConnection(DataSourceSyntax)) {
                con.Open();
                var sql = $"SELECT COUNT(*) FROM {DATABASE_TABLE_NAME};";
                var command = new SQLiteCommand(sql, con);
                SQLiteDataReader sdr = command.ExecuteReader();

                var count = 0;

                if (sdr.Read()) {
                    var v = sdr.GetValue(0);
                }

                sdr.Close();
                return count;
            }
        }

        public bool Connected { get; private set; }

        public string SystemMessage { get; set; }

    }
}
