using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    public interface IDBHelper {
        List<Comment> loadComments();
        void insertComment(Comment comment);
        List<Hashtable> select(String sql);
        bool Connected { get; }
        string SystemMessage { get; }
        long Count { get; }
    }

    public enum DBType { Local,Remote }
}
