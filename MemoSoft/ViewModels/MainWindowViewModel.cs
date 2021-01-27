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
        public PostgreSQLDBHelper PostgreSQLDatabaseHelper { get; private set; } = new PostgreSQLDBHelper();
        public UIColors UIColors { get; private set; } = new UIColors();

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
    }
}
