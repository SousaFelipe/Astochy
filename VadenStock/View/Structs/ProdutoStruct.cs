

namespace VadenStock.View.Structs
{
    public struct ProdutoStruct
    {
        public struct Img
        {
            public string Origin;
            public string FileName;
            public string FileExtension;
        }


        public string Name;
        public int Categoria;
        public int Tipo;
        public int Marca;
        public Img Image;
        public decimal Price;
        public string Description;
    }
}
