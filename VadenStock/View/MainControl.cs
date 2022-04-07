using System;

using VadenStock.Core;



namespace VadenStock.View
{
    public class MainControl : ObservableObject
    {
        private object _currentView = new();
        public object CurrentView
        {
            get { return _currentView; }

            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }



        public MainView View { get; private set; }



        public MainControl()
        {
            View = new MainView();
            CurrentView = View;
        }
    }
}
