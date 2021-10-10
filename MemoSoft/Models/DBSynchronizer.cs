namespace MemoSoft.Models
{
    public class DBSynchronizer
    {

        private IDBHelper RemoteDB { get; set; }
        private IDBHelper LocalDB { get; set; }

        public DBSynchronizer(IDBHelper remoteDB, IDBHelper localDB)
        {
            RemoteDB = remoteDB;
            LocalDB = localDB;
        }

        /// <summary>
        /// Remote DB からデータを取得し、Local DB に入力します。
        /// </summary>
        public void download()
        {
            DatabaseHelper dbHelper = (DatabaseHelper)LocalDB;
            var maxRemoteID = dbHelper.getMaxRemoteID();

            var remoteComments = RemoteDB.select($"SELECT * FROM COMMENTS WHERE {nameof(Comment.ID)} > {maxRemoteID};");

            remoteComments.ForEach(hash =>
            {
                var comment = Comment.toComment(hash);
                comment.RemoteID = comment.ID;
                comment.Uploaded = true;
                LocalDB.insertComment(comment);
            });

        }

        public void upload()
        {
            // コメントをアップする処理の前にリモートのコメントを取得する。
            // これを行わないと、後々整合性が取れなくなる。
            download();

            DatabaseHelper localDBHelper = (DatabaseHelper)LocalDB;
            var sql = $"SELECT * FROM {DatabaseHelper.DATABASE_TABLE_NAME} WHERE {nameof(Comment.RemoteID)} < 0 AND {nameof(Comment.Uploaded)} = 'False'";
            var uploadComments = localDBHelper.select(sql);

            uploadComments.ForEach(hash =>
            {
                var comment = Comment.toComment(hash);
                RemoteDB.insertComment(comment);

                comment.Uploaded = true;

                // RemoteID, Uploaded が書き換わるのでそれをローカルDBに反映する。
                localDBHelper.update(comment);
            });
        }
    }
}
