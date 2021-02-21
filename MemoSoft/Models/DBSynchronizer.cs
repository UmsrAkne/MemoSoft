using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    public class DBSynchronizer {

        private IDBHelper RemoteDB { get; set; }
        private IDBHelper LocalDB { get; set; }
        
        public DBSynchronizer(IDBHelper remoteDB, IDBHelper localDB) {
            RemoteDB = remoteDB;
            LocalDB = localDB;
        }
        
        /// <summary>
        /// Remote DB からデータを取得し、Local DB に入力します。
        /// </summary>
        public void download() {
            DatabaseHelper dbHelper = (DatabaseHelper)LocalDB;
            var maxRemoteID = dbHelper.getMaxRemoteID();

            var remoteComments = RemoteDB.select($"SELECT * FROM COMMENTS WHERE {nameof(Comment.ID)} > {maxRemoteID};");

            remoteComments.ForEach(hash => {
                var comment = Comment.toComment(hash);
                LocalDB.insertComment(comment);
            });
        }
    }
}
