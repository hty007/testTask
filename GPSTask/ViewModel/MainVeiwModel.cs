using System;

namespace GPSTask
{
    internal class MainVeiwModel:BaseViewModel
    {
        #region Привязки
        private bool pathTab;
        private PathViewModel pathModel;
        private MainView View;

        public bool PathTab
        {
            get => pathTab;
            set
            {
                pathTab = value;
                OnPropertyChanged("PathTab");
                OnPropertyChanged("EmulTab");
            }
        }
        public bool EmulTab
        {
            get => !pathTab;
            set
            {
                pathTab = !value;
                OnPropertyChanged("PathTab");
                OnPropertyChanged("EmulTab");
            }
        }


        public PathViewModel PathModel { get => pathModel; set { pathModel = value; OnPropertyChanged("PathModel"); } }
        #endregion


        public MainVeiwModel(MainView mainView)
        {
            PathModel = new PathViewModel();
            
        }
        internal void SetView(MainView view)
        {
            View = view;
            // Это выбивается из паттерна MVVM, но это необходимо для отрисовки пути.
            PathControl.SetView(View.pathControl)
        }
    }
}