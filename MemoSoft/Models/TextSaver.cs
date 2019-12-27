using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models
{
    /// <summary>
    /// このクラスは入力されたテキストを保存します。
    /// テキストの入力には、プロパティを使用してください。
    /// </summary>
    class TextSaver
    {
        private DatabaseHelper dbHelper;

        private readonly String DATABASE_NAME = "Diarydb";
        private readonly String DATABASE_TABLE_NAME = "diary";
        private readonly String DATABASE_COLUMN_NAME_DATE = "date";
        private readonly String DATABASE_COLUMN_NAME_TEXT = "text";

        public TextSaver() {
            this.dbHelper = new DatabaseHelper(DATABASE_NAME);
            this.dbHelper.createDatabase();

            this.dbHelper.createTable(DATABASE_TABLE_NAME,
                new String[] { DATABASE_COLUMN_NAME_DATE, DATABASE_COLUMN_NAME_TEXT }
                );
        }

        public string Text { get; set; }

        public void saveText() {
            dbHelper.insertData(
                DATABASE_TABLE_NAME,
                new String[] { DATABASE_COLUMN_NAME_DATE, DATABASE_COLUMN_NAME_TEXT },
                new String[] {DateTime.Now.ToString("yyyyMMddHHmmssff"), Text });
        }
    }
}
