using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace MemoSoft
{
    public class DatabaseHelper
    {
        private String dbFileName;

        public DatabaseHelper(string dbFileName) {
            this.dbFileName = dbFileName;
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
                    if (i == values.Length - 1) sql += values[i] + ");";
                    else sql += values[i] + ", ";
                }
                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
