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

        public TextSaver()
        {
            this.dbHelper = new DatabaseHelper(DatabaseHelper.DATABASE_NAME_EACH_PC);
            this.dbHelper.createDatabase();

            this.dbHelper.createTable(DatabaseHelper.DATABASE_TABLE_NAME,
                new String[] { DatabaseHelper.DATABASE_COLUMN_NAME_DATE, DatabaseHelper.DATABASE_COLUMN_NAME_TEXT }
                );
        }

        public string Text { get; set; }

        public void saveText()
        {
            dbHelper.insertData(
                DatabaseHelper.DATABASE_TABLE_NAME,
                new String[] { DatabaseHelper.DATABASE_COLUMN_NAME_DATE, DatabaseHelper.DATABASE_COLUMN_NAME_TEXT },
                new String[] { DateTime.Now.ToString("yyyyMMddHHmmssff"), Text });
        }
    }
}
