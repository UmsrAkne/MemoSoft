using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    class PostgreSQLDBHelper : BindableBase{
        // フィールド
        private List<Comment> comments = new List<Comment>();

        // コンストラクタ
        public PostgreSQLDBHelper() {
            loadComments();
        }

        // プロパティ

        public List<Comment> Comments {
            get => comments;
            private set => SetProperty(ref comments, value);
        } 

        private NpgsqlExecuter Executer { get; } = new NpgsqlExecuter();

        public void loadComments() {
            var sql = $"SELECT * FROM comments ORDER BY {nameof(Comment.CreationDateTime)} DESC;";
            var commentHashTables = Executer.select(sql , new List<Npgsql.NpgsqlParameter>());

            var commentList = new List<Comment>();

            commentHashTables.ForEach(h => {
                commentList.Add(Comment.toComment(h));
            });

            Comments = commentList;
        }
    }
}
