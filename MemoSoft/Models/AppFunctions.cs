namespace MemoSoft.Models
{
    using System.Windows;

    public class AppFunctions
    {
        public void exitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
