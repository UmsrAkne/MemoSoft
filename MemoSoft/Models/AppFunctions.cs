using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models
{
    class AppFunctions
    {

        public void exitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
