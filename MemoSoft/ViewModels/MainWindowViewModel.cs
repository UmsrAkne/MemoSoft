using MemoSoft.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.ViewModels {
    class MainWindowViewModel : BindableBase {
        public PostgreSQLDBHelper PostgreSQLDatabaseHelper { get; private set; } = new PostgreSQLDBHelper();
    }
}
