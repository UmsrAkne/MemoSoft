namespace MemoSoft.Models
{
    using System;
    using System.Collections;
    using Prism.Mvvm;

    public class Comment : BindableBase
    {
        private string textContent = string.Empty;

        public int ID { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string TextContent { get => textContent; set => SetProperty(ref textContent, value); }

        /// <summary>
        /// このコメントをビューに入力して表示した際、このコメントにあたる行の背景色に色を付けるかどうかを表します。
        /// 要するに表示時の見た目を整えるためのプロパティです。
        /// </summary>
        public bool LinePaint { get; set; }

        public string CreationDateShortString { get => CreationDateTime.ToString("MM/dd HH:mm"); }

        /// <summary>
        /// RemoteDB に格納されているコメントのIDです。デフォルトは -1 です。
        /// RemoteDB の select で生成された場合はそのままのIDが入ります。
        /// </summary>
        public int RemoteID { get; set; } = -1;

        /// <summary>
        /// このコメントオブジェクトが RemoteDB にアップロード済みかどうかを示します。
        /// RemoteDB の select で生成されてコメントオブジェクトの場合は常に true となります。
        /// </summary>
        public bool Uploaded { get; set; }

        /// <summary>
        /// コメントオブジェクトの情報が入ったハッシュテーブルオブジェクトから情報を抜き出し、コメントオブジェクトを生成します。
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Comment ToComment(Hashtable h)
        {
            Comment c = new Comment();

            object commentID = h.ContainsKey(nameof(ID)) ? h[nameof(ID)] : h[nameof(ID).ToLower()];

            if (commentID is long)
            {
                c.ID = Convert.ToInt32(commentID);
            }
            else
            {
                c.ID = (int)commentID;
            }

            object creationDateTimeObject = h.ContainsKey(nameof(CreationDateTime)) ? h[nameof(CreationDateTime)] : h[nameof(CreationDateTime).ToLower()];

            if (creationDateTimeObject is DateTime)
            {
                c.CreationDateTime = (DateTime)creationDateTimeObject;
            }
            else
            {
                c.CreationDateTime = DateTime.Parse((string)creationDateTimeObject);
            }

            c.TextContent = h.ContainsKey(nameof(TextContent)) ? (string)h[nameof(TextContent)] : (string)h[nameof(TextContent).ToLower()];
            return c;
        }
    }
}
