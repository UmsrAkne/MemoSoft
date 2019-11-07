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



    public class MainWindowViewModel : INotifyPropertyChanged{
        private string inputString;
        public string InputString {
            get { return inputString; }
            set {
                inputString = value;
                System.Diagnostics.Debug.WriteLine(inputString);
                OnPropertyChanged("InputString");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
