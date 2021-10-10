namespace MemoSoft.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum DBType { Local, Remote }

    public interface IDBHelper
    {
        bool Connected { get; }
        string SystemMessage { get; }
        long Count { get; }

        List<Comment> loadComments();
        void insertComment(Comment comment);
        List<Hashtable> select(string sql);
    }
}
