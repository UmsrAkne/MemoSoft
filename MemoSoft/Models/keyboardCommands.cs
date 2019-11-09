using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoSoft.Models
{
    public class keyboardCommands {

        public DelegateCommand saveFile = new DelegateCommand(
            () => { System.Diagnostics.Debug.WriteLine("saveFile"); },
            () => { return true; }
            );
        private MainWindowViewModel mainWindowViewModel;
        public MainWindowViewModel MainWindowViewModel {
            private get { return MainWindowViewModel; }
            set { mainWindowViewModel = mainWindowViewModel ?? value; }
        }

        public ICommand SaveFile {
            get { return this.saveFile; }
        } 
    }
}
