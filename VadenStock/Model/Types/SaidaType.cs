using System;



namespace VadenStock.Model.Types
{
    public struct SaidaType
    {
        public int Id { get; set; }
        public TransfType? Transferencia { get; set; }
        public string Responsavel { get; set; }
        public ItemType.Status Tipo { get; set; }
        public DateTime CreatedDate { get; set; }



        public static object? GetResponsavel(int id, ItemType.Status tipo)
        {
            return tipo switch
            {
                ItemType.Status.Comodato => Almoxarifado.Model.Where("id", id).Select()[0],
                _ => null
            };
        }
    }
}
