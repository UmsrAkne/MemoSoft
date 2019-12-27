﻿using System;
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

        private DelegateCommand saveFileCommand;
        public DelegateCommand SaveFileCommand {
            get {
                return saveFileCommand ?? (saveFileCommand = new DelegateCommand(
                    () => {
                        this.textSaver.Text = mainWindowViewModel.InputString;
                        this.textSaver.saveText();
                    },
                    () => { return true; })
                    );
            }
        }
    }
}
