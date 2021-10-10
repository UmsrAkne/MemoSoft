using Prism.Mvvm;
using System.Windows.Media;

namespace MemoSoft.Models
{
    class UIColors : BindableBase
    {

        public UIColors()
        {
            changeTheme(ColorTheme.Light);
        }

        public UIColors(ColorTheme theme)
        {
            changeTheme(theme);
        }

        private SolidColorBrush backgroundBrush;
        public SolidColorBrush BackgroundBrush
        {
            get => backgroundBrush;
            set => SetProperty(ref backgroundBrush, value);
        }

        private SolidColorBrush darkBackgroundBrush;
        public SolidColorBrush DarkBackgroundBrush
        {
            get => darkBackgroundBrush;
            set => SetProperty(ref darkBackgroundBrush, value);
        }

        private SolidColorBrush foregroundBrush;
        public SolidColorBrush ForegroundBrush
        {
            get => foregroundBrush;
            set => SetProperty(ref foregroundBrush, value);
        }

        public void changeTheme(ColorTheme theme)
        {
            if (theme == ColorTheme.Light)
            {
                BackgroundBrush = new SolidColorBrush(Colors.White);
                DarkBackgroundBrush = new SolidColorBrush(Colors.LightGray);
                ForegroundBrush = new SolidColorBrush(Colors.Black);
            }
            else
            {
                BackgroundBrush = new SolidColorBrush(Color.FromRgb(33, 33, 33));
                DarkBackgroundBrush = new SolidColorBrush(Colors.Black);
                ForegroundBrush = new SolidColorBrush(Colors.White);
            }
        }

    }

    public enum ColorTheme { Light, Dark };
}
