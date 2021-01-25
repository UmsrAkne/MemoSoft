using Npgsql;
using Prism.Commands;
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
        private String enteringComment = "";
        private DelegateCommand<String> insertCommentCommand;

        // コンストラクタ
        public PostgreSQLDBHelper() {
            try {
                loadComments();
            }catch(TimeoutException) {
                SystemMessage = "DBへの接続に失敗しました";
            }
        }

        // -------------------------------------------------- 
        // プロパティ

        public List<Comment> Comments {
            get => comments;
            private set => SetProperty(ref comments, value);
        }

        public String EnteringComment {
            get => enteringComment;
            set => SetProperty(ref enteringComment, value);
        }

        public DelegateCommand<String> InsertCommentCommand {
            get => insertCommentCommand ?? (insertCommentCommand = new DelegateCommand<String>((text) => {
                insertComment(new Comment() {
                    TextContent = text,
                    CreationDateTime = DateTime.Now
                });

                loadComments();
                EnteringComment = "";
            }));
        }

        public String SystemMessage {
            get => systemMessage;
            set => SetProperty(ref systemMessage, value);
        }

        private NpgsqlExecuter Executer { get; } = new NpgsqlExecuter();
        private string CommentTableName => "comments";
        private string systemMessage = "";

        // -------------------------------------------------- 
        // メソッド

        public void loadComments() {
            var sql = $"SELECT * FROM comments ORDER BY {nameof(Comment.CreationDateTime)} DESC;";
            var commentHashTables = Executer.select(sql , new List<Npgsql.NpgsqlParameter>());

            var commentList = new List<Comment>();

            int lastElementCreationDate = 0;
            bool linePaint = false;

            commentHashTables.ForEach(h => {
                var c = Comment.toComment(h);

                // 同じ日付のコメントには同じ背景色をつけるため、日付毎で linePaint の値が入れ替わるようにする。
                if(lastElementCreationDate != c.CreationDateTime.Day) {
                    linePaint = !linePaint;
                    lastElementCreationDate = c.CreationDateTime.Day;
                }

                c.LinePaint = linePaint;
                commentList.Add(c);
            });

            Comments = commentList;
        }

        public void insertComment(Comment comment) {

            int nextID = getMaxID() + 1;

            var ps = new List<NpgsqlParameter>();

            // 不正な値が入る余地がある TextContent 列のみパラメーターにして入力する。
            // CreationDateTime, ID 列に関しては内部で数値を処理して設定するため不正な値は入らないはず。
            var textParameter = new NpgsqlParameter(nameof(Comment.TextContent), NpgsqlTypes.NpgsqlDbType.Text) {
                Value = comment.TextContent
            };
            ps.Add(textParameter);

            Executer.executeNonQuery(
                $"INSERT INTO {CommentTableName} (" +
                $"{nameof(Comment.ID)}, " +
                $"{nameof(Comment.CreationDateTime)}," +
                $"{nameof(Comment.TextContent)} )" +
                $"VALUES (" +
                $"{nextID}," +
                $"'{comment.CreationDateTime}', " +
                $":{nameof(comment.TextContent)} " +
                $");"
                ,ps
            );

        }

        /// <summary>
        /// id列の最大値を取得します。
        /// </summary>
        /// <returns></returns>
        private int getMaxID() {
            var sql = $"SELECT MAX ({nameof(Comment.ID)}) FROM {CommentTableName};";
            return (int)Executer.select(sql, new List<NpgsqlParameter>())[0]["max"];
        }
    }
}
