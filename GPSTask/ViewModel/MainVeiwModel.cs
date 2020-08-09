using System;

namespace GPSTask
{
    internal class MainVeiwModel:BaseViewModel
    {
        #region Привязки
        private bool pathTab;
        private PathViewModel pathModel;
        private MainView View;
        private EmulatorViewModel emulModel;

        public static bool CanMove { get; private set; }
        public bool PathTab
        {
            get => pathTab;
            set
            {
                CanMove = !(pathTab = value);
                OnPropertyChanged("PathTab");
                OnPropertyChanged("EmulTab");
            }
        }
        public bool EmulTab
        {
            get => !pathTab;
            set
            {
                CanMove = pathTab = !value;

                OnPropertyChanged("PathTab");
                OnPropertyChanged("EmulTab");
            }
        }


        public PathViewModel PathModel { get => pathModel; set { pathModel = value; OnPropertyChanged("PathModel"); } }
        public EmulatorViewModel EmulModel { get => emulModel; set { emulModel = value; OnPropertyChanged("EmulModel"); } }

        internal void AutorClick()
        {
            System.Diagnostics.Process.Start("https://kazan.hh.ru/resume/4985a6efff05ad09bb0039ed1f323157313378");
        }
        #endregion


        public MainVeiwModel(MainView mainView)
        {
            PathModel = new PathViewModel();
            EmulModel = new EmulatorViewModel();
            PathTab = true;
            
        }
        internal void SetView(MainView view)
        {
            View = view;
            // Это выбивается из паттерна MVVM, но это необходимо для отрисовки пути.
            PathModel.SetView(View.pathControl);
            EmulModel.SetView(View.emulControl);
        }
    }
}