using System;

using VadenStock.Core;



namespace VadenStock.View
{
    public class ViewControl : ObservableObject
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



        public RelayCommand DashbordCommand { get; set; }
        public RelayCommand AlmoxarifadosCommand { get; set; }
        public RelayCommand ProdutosCommand { get; set; }



        public ViewControl()
        {
            CurrentView = new Dashboard();

            DashbordCommand = new RelayCommand(o => {
                CurrentView = new Dashboard();
            });

            AlmoxarifadosCommand = new RelayCommand(o => {
                CurrentView = new Almoxarifados();
            });

            ProdutosCommand = new RelayCommand(o => {
                CurrentView = new Produtos();
            });
        }
    }
}
