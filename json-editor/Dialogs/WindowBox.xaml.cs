﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFStorage.Base;

namespace WPFStorage.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindowBox : Window
    {
        public WindowBox(InputBoxModel model)
        {
            InitializeComponent();
            model.Success = () => { DialogResult = true; Close(); };
            model.Close = () => { DialogResult = false; Close(); };
            DataContext = model;
            Loaded += (a, b) => { model.Initialize(); };
        }
    }

    public class InputBoxModel : BaseViewModel
    {
        private string title = "Окно ввода";
        public string Title { get => title; set { title = value; OnPropertyChanged(); } }

        private string question = "Введите текст";
        public string Question { get => question; set { question = value; OnPropertyChanged(); } }

        private string input;
        public string Input { get => input; set { input = value; OnPropertyChanged(); } }

        private string okText = "Ввод";
        public string OkText { get => okText; set { okText = value; OnPropertyChanged(); } }

        private string cancelText = "Отмена";
        public string CancelText { get => cancelText; set { cancelText = value; OnPropertyChanged(); } }

        private string button3Text;
        public string Button3Text { get => button3Text; set { button3Text = value; OnPropertyChanged(); } }

        private HorizontalAlignment questionAlignment;
        public HorizontalAlignment QuestionAlignment { get => questionAlignment; set { questionAlignment = value; OnPropertyChanged(); } }

        private HorizontalAlignment buttonsAlignment;
        public HorizontalAlignment ButtonsAlignment { get => buttonsAlignment; set { buttonsAlignment = value; OnPropertyChanged(); } }
        public bool PressOk { get; set; }
        private long timeout;
        public long Timeout { get => timeout; set { timeout = value; OnPropertyChanged(); } }

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand Button3Command { get; private set; }
        public Action Close { get; internal set; }
        public Action Success { get; internal set; }
        public Visibility[] Visibilities { get => visibilities; set { visibilities = value; OnPropertyChanged(); } }


        private Visibility[] visibilities = new Visibility[4];
        DispatcherTimer timer;

        public InputBoxModel()
        {

            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
            Button3Command = new RelayCommand(Button3);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Close?.Invoke();
        }

        private void Ok() => Success?.Invoke();
        private void Cancel() => Close?.Invoke();

        private void Button3()
        {
            //Close?.Invoke();
        }

        public void SetVisibility(bool ok, bool cancel, bool input, bool button3 = false)
        {
            Visibilities[0] = (ok) ? Visibility.Visible : Visibility.Collapsed;
            Visibilities[1] = (cancel) ? Visibility.Visible : Visibility.Collapsed;
            Visibilities[2] = (button3) ? Visibility.Visible : Visibility.Collapsed;
            Visibilities[3] = (input) ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void Initialize()
        {
            if (Timeout > 0)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(Timeout * TimeSpan.TicksPerMillisecond);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }
    }
}
