using System.Collections.Generic;

using VadenStock.Model;



namespace VadenStock.View.Models
{
    public class DashboardViewModel
    {
        public static List<Categoria.Contract> Categorias
        {
            get {
                return ((Categoria)Categoria.New.Select()).Get();
            }
        }



        public static List<Tipo.Contract> GetTipos(int categoria = 0)
        {
            return ((Tipo)Tipo.New.Select().Where("categoria", "=", categoria)).Get();
        }
    }
}
