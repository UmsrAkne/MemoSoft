namespace MemoSoft.Models
{
    using System;

    /// <summary>
    /// このクラスは入力されたテキストを保存します。
    /// テキストの入力には、プロパティを使用してください。
    /// </summary>
    public class TextSaver
    {
        private DatabaseHelper dbhelper;

        public TextSaver()
        {
            dbhelper = new DatabaseHelper(DatabaseHelper.DatabaseNameEachPC);
            dbhelper.CreateDatabase();

            dbhelper.CreateTable(
                DatabaseHelper.DatabaesTableName,
                new string[] { DatabaseHelper.DatabaseColumnNameDate, DatabaseHelper.DabataseColumnNameText });
        }

        public string Text { get; set; }

        public void SaveText()
        {
            dbhelper.InsertData(
                DatabaseHelper.DatabaesTableName,
                new string[] { DatabaseHelper.DatabaseColumnNameDate, DatabaseHelper.DabataseColumnNameText },
                new string[] { DateTime.Now.ToString("yyyyMMddHHmmssff"), Text });
        }
    }
}
