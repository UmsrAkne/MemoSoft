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

        public ICommand SaveFile {
            get { return this.saveFile; }
        } 
    }
}
