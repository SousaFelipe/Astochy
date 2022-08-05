using System.IO;

using VadenStock.Model.Types;
using VadenStock.Tools;



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
        public double Valor;
        public string Description;



        public ProdutoStruct(ProdutoType produto)
        {
            Id = produto.Id;
            Name = produto.Name;
            Categoria = produto.Categoria.Id;
            Tipo = produto.Tipo.Id;
            Marca = produto.Marca.Id;
            Valor = produto.Valor;
            Description = produto.Description;
            Image.FileExtension = Path.GetExtension(produto.Image);
            Image.FileName = produto.Image.Replace(Path.GetExtension(produto.Image), "");
            Image.Origin = Src.Resource.Bind(Src.Resource.Root, Src.Resource.Storage) + produto.Image;
        }
    }
}
