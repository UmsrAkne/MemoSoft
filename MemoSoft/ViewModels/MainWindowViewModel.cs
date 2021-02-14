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
        public IDBHelper DBHelper { get; private set; } = new PostgreSQLDBHelper();
        public UIColors UIColors { get; private set; } = new UIColors();

        public MainWindowViewModel() {
            LoadCommand.Execute();
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

                UIColors.changeTheme((ColorTheme)theme);
            }));
        }
        private DelegateCommand<object> changeThemeCommand;
        #endregion


        public DelegateCommand LoadCommand {
            #region
            get => loadCommand ?? (loadCommand = new DelegateCommand(() => {
                Comments = DBHelper.loadComments();
            }));
        }
        private DelegateCommand loadCommand;
        #endregion

    }
}
