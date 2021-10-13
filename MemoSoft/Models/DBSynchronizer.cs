namespace MemoSoft.Models
{
    public class DBSynchronizer
    {
        public DBSynchronizer(IDBHelper remoteDB, IDBHelper localDB)
        {
            RemoteDB = remoteDB;
            LocalDB = localDB;
        }

        private IDBHelper RemoteDB { get; set; }

        private IDBHelper LocalDB { get; set; }

        /// <summary>
        /// Remote DB からデータを取得し、Local DB に入力します。
        /// </summary>
        public void Download()
        {
            DatabaseHelper dbhelper = (DatabaseHelper)LocalDB;
            var maxRemoteID = dbhelper.GetMaxRemoteID();

            var remoteComments = RemoteDB.Select($"SELECT * FROM COMMENTS WHERE {nameof(Comment.ID)} > {maxRemoteID};");

            remoteComments.ForEach(hash =>
            {
                var comment = Comment.ToComment(hash);
                comment.RemoteID = comment.ID;
                comment.Uploaded = true;
                LocalDB.InsertComment(comment);
            });
        }

        public void Upload()
        {
            // コメントをアップする処理の前にリモートのコメントを取得する。
            // これを行わないと、後々整合性が取れなくなる。
            Download();

            DatabaseHelper localDBHelper = (DatabaseHelper)LocalDB;
            var sql = $"SELECT * FROM {DatabaseHelper.DatabaesTableName} WHERE {nameof(Comment.RemoteID)} < 0 AND {nameof(Comment.Uploaded)} = 'False'";
            var uploadComments = localDBHelper.Select(sql);

            uploadComments.ForEach(hash =>
            {
                var comment = Comment.ToComment(hash);
                RemoteDB.InsertComment(comment);

                comment.Uploaded = true;

                // RemoteID, Uploaded が書き換わるのでそれをローカルDBに反映する。
                localDBHelper.Update(comment);
            });
        }
    }
}
