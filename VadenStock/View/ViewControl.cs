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
        public RelayCommand ProdutosCommand { get; set; }
        public RelayCommand ComprasCommand { get; set; }
        public RelayCommand FornecedoresCommand { get; set; }



        public ViewControl()
        {
            CurrentView = new Dashboard();

            DashbordCommand = new RelayCommand(o =>
            {
                CurrentView = new Dashboard();
            });

            ProdutosCommand = new RelayCommand(o =>
            {
                CurrentView = new Produtos();
            });

            ComprasCommand = new RelayCommand(o =>
            {
                CurrentView = new Compras();
            });

            FornecedoresCommand = new RelayCommand(o =>
            {
                CurrentView = new Fornecedores();
            });
        }
    }
}
