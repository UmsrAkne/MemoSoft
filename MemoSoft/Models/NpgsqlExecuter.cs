using System;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    public class NpgsqlExecuter{

        public NpgsqlExecuter() {

            string basePath = 
                Environment.GetEnvironmentVariable("HOMEDRIVE") +
                Environment.GetEnvironmentVariable("HOMEPATH")  + @"\awsrds\" ;

            string readText(string path) {
                using (var sr = new StreamReader(path)) {
                    return sr.ReadToEnd();
                }
            }

            ConnectionBuilder = new NpgsqlConnectionStringBuilder() {
                Host = readText(basePath + "hostName.txt" ),
                Username = readText(basePath + "user.txt"),
                Password = readText(basePath + "pass.txt"),
                Port = int.Parse(readText(basePath + "port.txt")),
                Database = "postgres"
            };
        }

        public void executeNonQuery(string sql, List<NpgsqlParameter> parameters) {
            using (var conn = Connection) {
                conn.Open();
                var command = new NpgsqlCommand(sql, conn);
                parameters.ForEach(p => command.Parameters.Add(p));
                command.ExecuteNonQuery();
            }
        }

        public List<Hashtable> select(string sql, List<NpgsqlParameter> parameters) {
            using (var con = Connection) {
                List<Hashtable> resultList = new List<Hashtable>();
                con.Open();
                var command = new NpgsqlCommand(sql, con);
                parameters.ForEach(p => command.Parameters.Add(p));
                var dataReader = command.ExecuteReader();

                while (dataReader.Read()) {
                    var hashtable = new Hashtable();
                    for (int i = 0; i < dataReader.FieldCount; i++) {
                        hashtable[dataReader.GetName(i)] = dataReader.GetValue(i);
                    }
                    resultList.Add(hashtable);
                }

                return resultList;
            };
        }

        private NpgsqlConnectionStringBuilder ConnectionBuilder { get; set; }
        private NpgsqlConnection Connection { get => new NpgsqlConnection(ConnectionBuilder.ConnectionString); }

    }
}
