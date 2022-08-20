namespace MemoSoft.Models
{
    using System.Collections.ObjectModel;

    public class TextLoader
    {
        private ObservableCollection<Comment> commentList;
        private DatabaseHelper dbhelper = new DatabaseHelper(DatabaseHelper.DatabaseNameEachPC);

        public ObservableCollection<Comment> CommentList
        {
            get { return this.commentList; }
            private set { this.commentList = value; }
        }

        /// <summary>
        /// 既存のテキストを新着順に読み込みます。
        /// </summary>
        /// <param name="loadCount">新着順に読み込む個数を指定してください。デフォルトではintの最大値が指定されます。</param>
        public void LoadInNewOrder(int loadCount = int.MaxValue)
        {
            dbhelper.Select();
            CommentList = new ObservableCollection<Comment>();

            var loadedCount = 0;
            for (int i = dbhelper.CommentList.Count - 1; i > 0; i--)
            {
                CommentList.Add(dbhelper.CommentList[i]);
                loadedCount++;
                if (loadedCount >= loadCount)
                {
                    break;
                }
            }
        }

        public void LoadLastComment()
        {
            dbhelper.GetLastUpdateRow();
            commentList = new ObservableCollection<Comment>();
            commentList.Add(dbhelper.CommentList[0]);
        }
    }
}
