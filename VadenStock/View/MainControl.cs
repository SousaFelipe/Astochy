using System;

using VadenStock.Core;



namespace VadenStock.View
{
    public class MainControl : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }

            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }



        public MainControl()
        {

        }
    }
}
