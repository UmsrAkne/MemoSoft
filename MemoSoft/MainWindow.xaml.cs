﻿using MemoSoft.Models;
using MemoSoft.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MemoSoft
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppFunctions appFunctions = new AppFunctions();
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.mainWindowViewModel = new MainWindowViewModel();
            RecoverWindowBounds();
        }

        private void App_Activated(object sender, EventArgs e)
        {
            Keyboard.Focus(this.textBox);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // ウィンドウのサイズを保存
            SaveWindowBounds();
            base.OnClosing(e);
        }

        /// <summary>
        /// ウィンドウの位置・サイズを保存します。
        /// </summary>
        private void SaveWindowBounds()
        {
            var settings = Settings.Default;
            settings.WindowMaximized = WindowState == WindowState.Maximized;
            WindowState = WindowState.Normal; // 最大化解除
            settings.WindowLeft = Left;
            settings.WindowTop = Top;
            settings.WindowWidth = Width;
            settings.WindowHeight = Height;
            settings.Save();
        }

        /// <summary>
        /// ウィンドウの位置・サイズを復元します。
        /// </summary>
        private void RecoverWindowBounds()
        {
            var settings = Settings.Default;

            // 左
            if (settings.WindowLeft >= 0 &&
                (settings.WindowLeft + settings.WindowWidth) < SystemParameters.VirtualScreenWidth)
            { Left = settings.WindowLeft; }

            // 上
            if (settings.WindowTop >= 0 &&
                (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight)
            { Top = settings.WindowTop; }

            // 幅
            if (settings.WindowWidth > 0 &&
                settings.WindowWidth <= SystemParameters.WorkArea.Width)
            { Width = settings.WindowWidth; }

            // 高さ
            if (settings.WindowHeight > 0 &&
                settings.WindowHeight <= SystemParameters.WorkArea.Height)
            { Height = settings.WindowHeight; }

            // 最大化
            if (settings.WindowMaximized)
            {
                // ロード後に最大化
                Loaded += (o, e) => WindowState = WindowState.Maximized;
            }
        }
    }

    public class MainWindowViewModel : INotifyPropertyChangedBase
    {
        private string inputString;
        private keyboardCommands keyCommands = new keyboardCommands();
        private TextLoader textLoader = new TextLoader();

        public MainWindowViewModel()
        {
            this.keyCommands.MainWindowViewModel = this;
            textLoader.loadInNewOrder(20);
            PostedComments = textLoader.CommentList;
        }

        public string InputString
        {
            get { return inputString; }
            set { SetProperty(ref this.inputString, value); }
        }

        private ObservableCollection<Comment> postedComments;
        public ObservableCollection<Comment> PostedComments
        {
            get { return postedComments; }
            private set { postedComments = value; }
        }

        public keyboardCommands KeyCommands
        {
            get { return this.keyCommands; }
        }
    }
}
