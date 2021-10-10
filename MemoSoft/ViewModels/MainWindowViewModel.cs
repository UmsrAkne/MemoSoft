
namespace MemoSoft.ViewModels
{
    using System;
    using System.Collections.Generic;
    using MemoSoft.Models;
    using Prism.Commands;
    using Prism.Mvvm;
    using static MemoSoft.Models.UIColors;

    public class MainWindowViewModel : BindableBase
    {
        private DelegateCommand syncCommand;
        private string enteringComment = string.Empty;
        private IDBHelper dbHelper;

        private string systemMessage = "system message";
        private List<Comment> comments = new List<Comment>();
        private DelegateCommand loadCommand;
        private DelegateCommand<object> changeThemeCommand;
        private DelegateCommand<object> switchDBCommand;
        private long recordCount;
        private DelegateCommand<string> insertCommentCommand;


        public MainWindowViewModel()
        {
            DBHelper = new PostgreSQLDBHelper();

            // PostgreSQL の方がつながっていなければオフラインの sqlite に切り替え。
            if (!DBHelper.Connected)
            {
                DBHelper = new DatabaseHelper("Diarydb");
                SwitchDBCommand.Execute(DBType.Local);
            }

            LoadCommand.Execute();

            UIColors.changeTheme((ColorTheme)Enum.ToObject(typeof(ColorTheme), Properties.Settings.Default.ColorTheme));
        }

        public IDBHelper DBHelper
        {
            get => dbHelper;
            private set => SetProperty(ref dbHelper, value);
        }

        public UIColors UIColors { get; private set; } = new UIColors();

        public List<Comment> Comments
        {
            get => comments;
            private set => SetProperty(ref comments, value);
        }

        public string EnteringComment
        {
            get => enteringComment;
            set => SetProperty(ref enteringComment, value);
        }


        public string SystemMessage
        {
            get => systemMessage;
            set => SetProperty(ref systemMessage, value);
        }


        public long RecordCount
        {
            get => recordCount;
            set => SetProperty(ref recordCount, value);
        }

        public DelegateCommand<string> InsertCommentCommand
        {
            get => insertCommentCommand ?? (insertCommentCommand = new DelegateCommand<string>((text) =>
            {
                DBHelper.insertComment(new Comment()
                {
                    TextContent = text,
                    CreationDateTime = DateTime.Now
                });

                Comments = DBHelper.loadComments();
                RecordCount = DBHelper.Count;
                EnteringComment = string.Empty;
            }));
        }

        public DelegateCommand<object> ChangeThemeCommand
        {
            get => changeThemeCommand ?? (changeThemeCommand = new DelegateCommand<object>((object theme) =>
            {
                // ジェネリクスが object 型になっているが、列挙型は nullable ではないらしく、
                // nullable でない型をパラメーターで指定するとエラーになるため。
                // xaml の方で直接生成してパラメーターに渡すので、null になることも間違った値が入ることもないけど……。

                ColorTheme destTheme = (ColorTheme)theme;
                UIColors.changeTheme(destTheme);
                Properties.Settings.Default.ColorTheme = (int)theme;
                Properties.Settings.Default.Save();
            }));
        }

        public DelegateCommand LoadCommand
        {
            get => loadCommand ?? (loadCommand = new DelegateCommand(() =>
            {
                Comments = DBHelper.loadComments();
                RecordCount = DBHelper.Count;
                SystemMessage = DBHelper.SystemMessage;
            }));
        }

        public DelegateCommand<object> SwitchDBCommand
        {
            get => switchDBCommand ?? (switchDBCommand = new DelegateCommand<object>((object dbType) =>
            {

                DBType type = (DBType)dbType;

                if (type == DBType.Local)
                {
                    DBHelper = new DatabaseHelper("Diarydb");
                }
                else if (type == DBType.Remote)
                {
                    IDBHelper helper = new PostgreSQLDBHelper();
                    DBHelper = helper.Connected ? helper : new DatabaseHelper("Diarydb");
                }

                Comments = DBHelper.loadComments();

            }));
        }

        public DelegateCommand SyncCommand
        {
            get => syncCommand ?? (syncCommand = new DelegateCommand(() =>
            {
                var remoteDB = new PostgreSQLDBHelper();
                var localDB = new DatabaseHelper("Diarydb");

                if (remoteDB.Connected && localDB.Connected)
                {
                    DBSynchronizer = new DBSynchronizer(remoteDB, localDB);
                    DBSynchronizer.upload();
                }
            }));
        }

        private DBSynchronizer DBSynchronizer { get; set; }
    }
}
