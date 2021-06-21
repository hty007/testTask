using System;
using System.Windows;
using System.Windows.Threading;
using WPFStorage.Base;

namespace WPFStorage.Dialogs
{
    public class InputBoxModel : ObservableObject
    {
        private string title = "Окно ввода";
        public string Title { get => title; set { SetProperty(ref title, value); } }

        private string question = "Введите текст";
        public string Question { get => question; set { SetProperty(ref question, value); } }

        private string input;
        public string Input { get => input; set { SetProperty(ref input, value); } }

        private string okText = "Ввод";
        public string OkText { get => okText; set { SetProperty(ref okText, value); } }

        private string cancelText = "Отмена";
        public string CancelText { get => cancelText; set { SetProperty(ref cancelText, value); } }

        private string button3Text;
        public string Button3Text { get => button3Text; set { SetProperty(ref button3Text, value); } }

        private HorizontalAlignment questionAlignment;
        public HorizontalAlignment QuestionAlignment { get => questionAlignment; set { SetProperty(ref questionAlignment, value); } }

        private HorizontalAlignment buttonsAlignment;
        public HorizontalAlignment ButtonsAlignment { get => buttonsAlignment; set { SetProperty(ref buttonsAlignment, value); } }
        public bool PressOk { get; set; }
        private long timeout;
        public long Timeout { get => timeout; set { SetProperty(ref timeout, value); } }

        public RelayCommand OkCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand Button3Command { get; private set; }
        public Action Close { get; internal set; }
        public Action Success { get; internal set; }
        public Visibility[] Visibilities { get => visibilities; set { SetProperty(ref visibilities, value); } }


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
