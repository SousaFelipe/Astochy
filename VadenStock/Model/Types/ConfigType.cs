using System;



namespace VadenStock.Model.Types
{
    public struct ConfigType : IModelType
    {
        public int Id { get; set; }
        public string ProductionPath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
