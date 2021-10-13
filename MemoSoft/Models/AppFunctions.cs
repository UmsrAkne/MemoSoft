namespace MemoSoft.Models
{
    using System.Windows;

    public class AppFunctions
    {
        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
