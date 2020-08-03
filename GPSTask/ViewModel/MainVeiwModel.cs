namespace GPSTask
{
    internal class MainVeiwModel:BaseViewModel
    {
        #region Привязки
        private bool pathTab;
        private PathViewModel pathModel;

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
        public PathViewModel PathModel { get => pathModel; set { pathModel = value; OnPropertyChanged("PathModel") } }
        #endregion


        public MainVeiwModel()
        {
            PathModel = new PathViewModel();

        }
    }
}