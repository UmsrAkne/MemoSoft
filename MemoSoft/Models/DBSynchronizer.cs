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
    }
}
