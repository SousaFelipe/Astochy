using System;



namespace VadenStock.Model.Types
{
    public struct ProdutoType
    {
        public int Id { get; set; }
        public TipoType Tipo { get; set; }
        public MarcaType Marca { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
