using System;
using System.Collections.Generic;

using VadenStock.Model;



namespace VadenStock.Controller
{
    public class Categorias
    {
        public static List<Categoria.Contract> Listar()
        {
            List<Categoria.Contract> categorias = ( (Categoria)Categoria.New.Select() ).Get();
            return categorias;
        }
    }
}
