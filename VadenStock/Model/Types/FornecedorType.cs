using System;



namespace VadenStock.Model.Types
{
    public struct FornecedorType
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string Fantasia { get; set; }
        public string Email { get; set; }
        public string Contato { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
