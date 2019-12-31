using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MemoSoft.Models
{
    class TextLoader
    {

        private ObservableCollection<Comment> commentList;
        public ObservableCollection<Comment> CommentList {
            get { return this.commentList; }
            private set { this.commentList = value; }
        }

        private DatabaseHelper dbHelper = new DatabaseHelper(DatabaseHelper.DATABASE_NAME);

        /// <summary>
        /// 既存のテキストを新着順に読み込みます。
        /// </summary>
        /// <param name="loadCount">新着順に読み込む個数を指定してください。デフォルトではintの最大値が指定されます。</param>
        public void loadInNewOrder(int loadCount = int.MaxValue) {
            dbHelper.select();
            CommentList = new ObservableCollection<Comment>();

            var loadedCount = 0;
            for(int i = dbHelper.CommentList.Count -1; i > 0; i--) {
                CommentList.Add( dbHelper.CommentList[i] );
                loadedCount++;
                if (loadedCount >= loadCount) break;
            }
        }

        public void loadLastComment() {
            dbHelper.getLastUpdateRow();
            commentList = new ObservableCollection<Comment>();
            commentList.Add(dbHelper.CommentList[0]);
        }

    }
}
