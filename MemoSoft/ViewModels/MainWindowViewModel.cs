using MemoSoft.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.ViewModels {
    class MainWindowViewModel : BindableBase {

        public IDBHelper DBHelper {
            get => dbHelper;
            private set => SetProperty(ref dbHelper, value);
        }
        private IDBHelper dbHelper;

        private DBSynchronizer DBSynchronizer{get; set;}
        public UIColors UIColors { get; private set; } = new UIColors();

        public MainWindowViewModel() {
            DBHelper = new PostgreSQLDBHelper();

            // PostgreSQL の方がつながっていなければオフラインの sqlite に切り替え。
            if (!DBHelper.Connected) {
                DBHelper = new DatabaseHelper("Diarydb");
                SwitchDBCommand.Execute(DBType.Local);
            }

            LoadCommand.Execute();

            UIColors.changeTheme((ColorTheme)Enum.ToObject(typeof(ColorTheme), Properties.Settings.Default.ColorTheme));
            RecordCount = DBHelper.Count;
        }

        public List<Comment> Comments {
            #region
            get => comments;
            private set => SetProperty(ref comments, value);
        }

        private List<Comment> comments = new List<Comment>();
        #endregion

        public String EnteringComment {
            #region
            get => enteringComment;
            set => SetProperty(ref enteringComment, value);
        }

        private String enteringComment = "";
        #endregion

        public String SystemMessage {
            get => systemMessage;
            set => SetProperty(ref systemMessage, value);
        }

        private String systemMessage = "system message";

        public long RecordCount {
            get => recordCount;
            set => SetProperty(ref recordCount, value);
        }

        private long recordCount;

        public DelegateCommand<String> InsertCommentCommand {
            #region
            get => insertCommentCommand ?? (insertCommentCommand = new DelegateCommand<String>((text) => {
                DBHelper.insertComment(new Comment() {
                    TextContent = text,
                    CreationDateTime = DateTime.Now
                });

                Comments = DBHelper.loadComments();
                EnteringComment = "";
            }));
        }

        private DelegateCommand<String> insertCommentCommand;
        #endregion

        public DelegateCommand<object> ChangeThemeCommand {
            #region
            get => changeThemeCommand ?? (changeThemeCommand = new DelegateCommand<object>((object theme) => {
                // ジェネリクスが object 型になっているが、列挙型は nullable ではないらしく、
                // nullable でない型をパラメーターで指定するとエラーになるため。
                // xaml の方で直接生成してパラメーターに渡すので、null になることも間違った値が入ることもないけど……。

                ColorTheme destTheme = (ColorTheme)theme;
                UIColors.changeTheme(destTheme);
                Properties.Settings.Default.ColorTheme = (int)theme;
                Properties.Settings.Default.Save();
            }));
        }
        private DelegateCommand<object> changeThemeCommand;
        #endregion


        public DelegateCommand LoadCommand {
            #region
            get => loadCommand ?? (loadCommand = new DelegateCommand(() => {
                Comments = DBHelper.loadComments();
                SystemMessage = DBHelper.SystemMessage;
            }));
        }
        private DelegateCommand loadCommand;
        #endregion


        public DelegateCommand<object> SwitchDBCommand {
            #region
            get => switchDBCommand ?? (switchDBCommand = new DelegateCommand<object>((object dbType) => {

                DBType type = (DBType)dbType;

                if(type == DBType.Local) {
                    DBHelper = new DatabaseHelper("Diarydb");
                }else if(type == DBType.Remote) {
                    IDBHelper helper = new PostgreSQLDBHelper();
                    DBHelper = (helper.Connected) ? helper : new DatabaseHelper("Diarydb");
                }

                Comments = DBHelper.loadComments();

            }));
        }
        private DelegateCommand<object> switchDBCommand;
        #endregion


        public DelegateCommand SyncCommand {
            #region
            get => syncCommand ?? (syncCommand = new DelegateCommand(() => {
                var remoteDB = new PostgreSQLDBHelper();
                var localDB = new DatabaseHelper("Diarydb");

                if(remoteDB.Connected && localDB.Connected) {
                    DBSynchronizer = new DBSynchronizer(remoteDB, localDB);
                    DBSynchronizer.upload();
                }
            }));
        }
        private DelegateCommand syncCommand;
        #endregion


    }
}
