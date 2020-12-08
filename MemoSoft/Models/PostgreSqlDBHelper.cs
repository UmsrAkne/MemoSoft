using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    class PostgreSQLDBHelper {
        private NpgsqlExecuter Executer { get; } = new NpgsqlExecuter();
    }
}
