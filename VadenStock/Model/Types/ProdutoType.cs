using System;



namespace VadenStock.Model.Types
{
    public class ProdutoType
    {
        public int Id { get; set; }
        public CategoriaType Categoria { get; set; }
        public TipoType Tipo { get; set; }
        public MarcaType Marca { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Valor { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
