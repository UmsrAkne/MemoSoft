using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    interface IDBHelper {
        List<Comment> loadComments();
        void insertComment(Comment comment);
    }
}
