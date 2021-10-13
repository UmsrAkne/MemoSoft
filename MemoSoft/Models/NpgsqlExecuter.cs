namespace MemoSoft.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Npgsql;

    public class NpgsqlExecuter
    {
        public NpgsqlExecuter()
        {
            string basePath =
                Environment.GetEnvironmentVariable("HOMEDRIVE") +
                Environment.GetEnvironmentVariable("HOMEPATH") + @"\ec2db\";

            string readText(string path)
            {
                using (var sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }

            ConnectionBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = readText(basePath + "hostName.txt"),
                Username = readText(basePath + "user.txt"),
                Password = readText(basePath + "pass.txt"),
                Port = int.Parse(readText(basePath + "port.txt")),
                Database = "postgres",
                Timeout = 5
            };
        }

        private NpgsqlConnectionStringBuilder ConnectionBuilder { get; set; }

        private NpgsqlConnection Connection { get => new NpgsqlConnection(ConnectionBuilder.ConnectionString); }

        public void ExecuteNonQuery(string sql, List<NpgsqlParameter> parameters)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var command = new NpgsqlCommand(sql, conn);
                parameters.ForEach(p => command.Parameters.Add(p));
                command.ExecuteNonQuery();
            }
        }

        public List<Hashtable> Select(string sql, List<NpgsqlParameter> parameters)
        {
            using (var con = Connection)
            {
                List<Hashtable> resultList = new List<Hashtable>();
                con.Open();
                var command = new NpgsqlCommand(sql, con);
                parameters.ForEach(p => command.Parameters.Add(p));
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var hashtable = new Hashtable();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        hashtable[dataReader.GetName(i)] = dataReader.GetValue(i);
                    }

                    resultList.Add(hashtable);
                }

                return resultList;
            }
        }
    }
}
