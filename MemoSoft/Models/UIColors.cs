namespace MemoSoft.Models
{
    using System.Windows.Media;
    using Prism.Mvvm;

    public enum ColorTheme
    {
        Light, Dark
    }

    public class UIColors : BindableBase
    {
        private SolidColorBrush backgroundBrush;
        private SolidColorBrush darkBackgroundBrush;
        private SolidColorBrush foregroundBrush;

        public UIColors()
        {
            ChangeTheme(ColorTheme.Light);
        }

        public UIColors(ColorTheme theme)
        {
            ChangeTheme(theme);
        }

        public SolidColorBrush BackgroundBrush
        {
            get => backgroundBrush;
            set => SetProperty(ref backgroundBrush, value);
        }

        public SolidColorBrush DarkBackgroundBrush
        {
            get => darkBackgroundBrush;
            set => SetProperty(ref darkBackgroundBrush, value);
        }

        public SolidColorBrush ForegroundBrush
        {
            get => foregroundBrush;
            set => SetProperty(ref foregroundBrush, value);
        }

        public void ChangeTheme(ColorTheme theme)
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
}
