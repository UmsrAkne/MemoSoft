﻿using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models
{
    public class Comment : BindableBase {
        public int ID { get; set; }

        public DateTime CreationDateTime { get; set; }

        public String TextContent { get => textContent; set => SetProperty(ref textContent, value); }
        private String textContent = "";

        /// <summary>
        /// このコメントをビューに入力して表示した際、このコメントにあたる行の背景色に色を付けるかどうかを表します。
        /// 要するに表示時の見た目を整えるためのプロパティです。
        /// </summary>
        public bool LinePaint { get; set; }

        public String CreationDateShortString { get => CreationDateTime.ToString("MM/dd HH:mm"); }

        /// <summary>
        /// RemoteDB に格納されているコメントのIDです。デフォルトは -1 です。
        /// RemoteDB の select で生成された場合はそのままのIDが入ります。
        /// </summary>
        public int RemoteID { get; set; } = -1 ;

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
        public static Comment toComment(Hashtable h) {
            Comment c = new Comment();
            c.ID = (int)h[nameof(ID).ToLower()];
            c.CreationDateTime = (DateTime)h[nameof(CreationDateTime).ToLower()];
            c.TextContent = (String)h[nameof(TextContent).ToLower()];
            return c;
        }
    }
}
