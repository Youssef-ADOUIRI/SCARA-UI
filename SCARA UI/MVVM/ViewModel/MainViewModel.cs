using SCARA_UI.core;
using System;

namespace SCARA_UI.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand TestingServoCommand { get; set; }
        public RelayCommand CinematicsCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public TestingServoViewModel STVM { get; set; }
        public CinematicsViewModel CVM { get; set; }

        private object _currentView;

        public object CurentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            STVM = new TestingServoViewModel();
            CVM = new CinematicsViewModel();

            CurentView = HomeVM;

            TestingServoCommand = new RelayCommand(new Action<object>(goTestServo));

            HomeViewCommand = new RelayCommand(o =>
            {
                CurentView = HomeVM;
            });

            CinematicsCommand = new RelayCommand(o =>
            {
                CurentView = CVM;
            });
            
        }

        public void goTestServo(object obj)
        {
            CurentView = STVM;
        }

        
    }
}
