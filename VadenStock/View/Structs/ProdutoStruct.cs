

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


        public int Id;
        public string Name;
        public int Categoria;
        public int Tipo;
        public int Marca;
        public Img Image;
        public double Price;
        public string Description;
    }
}
