namespace MemoSoft.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum DBType
    {
        Local, Remote
    }

    public interface IDBHelper
    {
        bool Connected { get; }

        string SystemMessage { get; }

        long Count { get; }

        List<Comment> LoadComments();

        void InsertComment(Comment comment);

        List<Hashtable> Select(string sql);
    }
}
