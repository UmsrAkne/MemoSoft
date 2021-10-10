namespace MemoSoft.Models
{
    using System.Collections.ObjectModel;

    public class TextLoader
    {

        private ObservableCollection<Comment> commentList;
        private DatabaseHelper dbHelper = new DatabaseHelper(DatabaseHelper.DATABASE_NAME_EACH_PC);

        public ObservableCollection<Comment> CommentList
        {
            get { return this.commentList; }
            private set { this.commentList = value; }
        }

        /// <summary>
        /// 既存のテキストを新着順に読み込みます。
        /// </summary>
        /// <param name="loadCount">新着順に読み込む個数を指定してください。デフォルトではintの最大値が指定されます。</param>
        public void loadInNewOrder(int loadCount = int.MaxValue)
        {
            dbHelper.select();
            CommentList = new ObservableCollection<Comment>();

            var loadedCount = 0;
            for (int i = dbHelper.CommentList.Count - 1; i > 0; i--)
            {
                CommentList.Add(dbHelper.CommentList[i]);
                loadedCount++;
                if (loadedCount >= loadCount) break;
            }
        }

        public void loadLastComment()
        {
            dbHelper.getLastUpdateRow();
            commentList = new ObservableCollection<Comment>();
            commentList.Add(dbHelper.CommentList[0]);
        }

    }
}
