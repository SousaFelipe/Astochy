using System;
using System.Collections.Generic;

using VadenStock.Model;
using VadenStock.Model.Types;



namespace VadenStock.View.Models
{
    public static class FornecedoresViewModel
    {
        public static List<FornecedorType> TodosOsFornecedores
        {
            get { return Fornecedor.Model.Where("id", ">", 1).Select(); }
        }



        public static bool Create(FornecedorType fornecedor)
        {
            List<object[]> inserts = new()
            {
                new object[2] { "cnpj", fornecedor.Cnpj },
                new object[2] { "tag", fornecedor.Tag },
                new object[2] { "fantasia", fornecedor.Fantasia },
                new object[2] { "email", fornecedor.Email },
                new object[2] { "contato", fornecedor.Contato },
                new object[2] { "telefone", fornecedor.Telefone },
                new object[2] { "whatsapp", fornecedor.Whatsapp }
            };

            return Fornecedor.Model.Create(inserts) > 0;
        }



        public static FornecedorType? Find(object idOrCnpj)
        {
            List<FornecedorType> fornecedores = Fornecedor.Model
                .Where("id", "=", idOrCnpj)
                .Or("cnpj", "=", idOrCnpj)
                .Select();

            return (fornecedores != null && fornecedores.Count > 0)
                ? fornecedores[0]
                : null;
        }



        public static bool Delete(int id)
        {
            return Fornecedor.Model.Delete(id);
        }
    }
}
