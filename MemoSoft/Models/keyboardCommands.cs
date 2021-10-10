namespace MemoSoft.Models
{
    using Prism.Commands;

    public class KeyboardCommands
    {
        private TextSaver textSaver = new TextSaver();
        private TextLoader textLoader = new TextLoader();
        private MainWindowViewModel mainWindowViewModel;
        private DelegateCommand saveFileCommand;

        public MainWindowViewModel MainWindowViewModel
        {
            private get { return mainWindowViewModel; }
            set { mainWindowViewModel = mainWindowViewModel ?? value; }
        }

        public DelegateCommand SaveFileCommand
        {
            get
            {
                return saveFileCommand ?? (saveFileCommand = new DelegateCommand(
                    () =>
                    {
                        this.textSaver.Text = mainWindowViewModel.InputString;
                        this.textSaver.saveText();
                        MainWindowViewModel.InputString = string.Empty;
                        textLoader.loadLastComment();
                        MainWindowViewModel.PostedComments.Insert(0, textLoader.CommentList[0]);

                        if (MainWindowViewModel.PostedComments.Count > 20)
                        {
                            MainWindowViewModel.PostedComments.RemoveAt(MainWindowViewModel.PostedComments.Count - 1);
                        }
                    },
                    () => { return true; }));
            }
        }
    }
}
