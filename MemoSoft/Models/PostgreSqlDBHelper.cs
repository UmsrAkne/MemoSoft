using Npgsql;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models {
    public class PostgreSQLDBHelper : BindableBase , IDBHelper{
        // フィールド
        private DelegateCommand reloadCommand;

        // コンストラクタ
        public PostgreSQLDBHelper() {
            try {
                loadComments();
                Connected = true;
                SystemMessage = "DBサーバーへ接続しました";
            }catch(TimeoutException) {
                SystemMessage = "DBへの接続に失敗しました";
            } catch (SocketException) {
                SystemMessage = "DBへの接続に失敗しました";
            }
        }

        // -------------------------------------------------- 
        // プロパティ

        public DelegateCommand ReloadCommand {
            get => reloadCommand ?? (reloadCommand = new DelegateCommand(() => {
                try {
                    loadComments();
                    SystemMessage = $"{DateTime.Now.ToString() } コメントをリロードしました ";
                }catch(TimeoutException) {
                    SystemMessage = "DBへの接続に失敗しました";
                }
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

        public List<Comment> loadComments() {
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
                c.Uploaded = true;
                c.RemoteID = c.ID;
                commentList.Add(c);
            });

            return commentList;
        }

        /// <summary>
        /// DBにコメントを挿入します。
        /// 挿入の際、引数に渡された commentオブジェクトの RemoteID を、DB上のIDの値で上書きします。
        /// </summary>
        /// <param name="comment"></param>
        public void insertComment(Comment comment) {

            int nextID = getMaxID() + 1;
            comment.RemoteID = nextID;

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

        public long Count {
            get {
                var value = select($"SELECT COUNT(*) FROM {CommentTableName};");
                return (long)(value[0]["count"]);
            }
        }

        public List<Hashtable> select(string sql) {
            return Executer.select(sql, new List<NpgsqlParameter>());
        }

        public bool Connected { get; private set; }
    }
}
