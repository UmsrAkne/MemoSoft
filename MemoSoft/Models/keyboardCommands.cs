using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemoSoft.Models
{
    public class keyboardCommands {

        private MainWindowViewModel mainWindowViewModel;
        public MainWindowViewModel MainWindowViewModel {
            private get { return mainWindowViewModel; }
            set { mainWindowViewModel = mainWindowViewModel ?? value; }
        }

        private TextSaver textSaver = new TextSaver();
        private TextLoader textLoader = new TextLoader();

        private DelegateCommand saveFileCommand;
        public DelegateCommand SaveFileCommand {
            get {
                return saveFileCommand ?? (saveFileCommand = new DelegateCommand(
                    () => {
                        this.textSaver.Text = mainWindowViewModel.InputString;
                        this.textSaver.saveText();
                        MainWindowViewModel.InputString = "";
                        textLoader.loadLastComment();
                        MainWindowViewModel.PostedComments.Insert(0, textLoader.CommentList[0]);

                        if(MainWindowViewModel.PostedComments.Count > 20) {
                            MainWindowViewModel.PostedComments.RemoveAt(MainWindowViewModel.PostedComments.Count - 1);
                        }
                    },
                    () => { return true; })
                    );
            }
        }
    }
}
