
namespace MemoSoft.Models
{
    using System;

    /// <summary>
    /// このクラスは入力されたテキストを保存します。
    /// テキストの入力には、プロパティを使用してください。
    /// </summary>
    public class TextSaver
    {
        private DatabaseHelper dbHelper;

        public TextSaver()
        {
            dbHelper = new DatabaseHelper(DatabaseHelper.DATABASE_NAME_EACH_PC);
            dbHelper.createDatabase();

            dbHelper.createTable(
                DatabaseHelper.DATABASE_TABLE_NAME,
                new string[] { DatabaseHelper.DATABASE_COLUMN_NAME_DATE, DatabaseHelper.DATABASE_COLUMN_NAME_TEXT });
        }

        public string Text { get; set; }

        public void saveText()
        {
            dbHelper.insertData(
                DatabaseHelper.DATABASE_TABLE_NAME,
                new string[] { DatabaseHelper.DATABASE_COLUMN_NAME_DATE, DatabaseHelper.DATABASE_COLUMN_NAME_TEXT },
                new string[] { DateTime.Now.ToString("yyyyMMddHHmmssff"), Text });
        }
    }
}
