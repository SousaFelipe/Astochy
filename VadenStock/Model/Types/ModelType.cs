using System;



namespace VadenStock.Model.Types
{
    public interface IModelType
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
