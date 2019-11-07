using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemoSoft.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace MemoSoft
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppFunctions appFunctions = new AppFunctions();
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow() {
            InitializeComponent();
            this.mainWindowViewModel = new MainWindowViewModel();
            this.DataContext = mainWindowViewModel;
        }

        private void App_Activated(object sender, EventArgs e) {
            Keyboard.Focus(this.textBox);
        }

        private void keyDownEventHandler(object sender, KeyEventArgs e) {
            if((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None){
                if(e.Key == Key.Q) {
                    appFunctions.exitApplication();
                }
            }
        }
    }


    public class MainWindowViewModel : INotifyPropertyChangedBase{
        private string inputString;
        public string InputString {
            get { return inputString; }
            set { if (SetProperty(ref this.inputString, value)) ; }
        }
    }
}
