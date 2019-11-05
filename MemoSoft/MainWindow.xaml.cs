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

namespace MemoSoft
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppFunctions appFunctions = new AppFunctions();

        public MainWindow() {
            InitializeComponent();
        }

        private void App_Activated(object sender, EventArgs e) {
            Keyboard.Focus(this.textBox);
        }

        private void keyDownEventHandler(object sender, KeyEventArgs e) {
            System.Diagnostics.Debug.WriteLine("test");
            if((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None){
                if(e.Key == Key.Q) {
                    appFunctions.exitApplication();
                }
            }
        }
    }
}
