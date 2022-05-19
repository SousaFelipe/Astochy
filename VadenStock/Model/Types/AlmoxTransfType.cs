using System;



namespace VadenStock.Model.Types
{
    public class AlmoxTransfType : IModelType
    {
        public int Id { get; set; }
        public ItemType Item { get; set; }
        public AlmoxType From { get; set; }
        public AlmoxType? To { get; set; }
        public ItemType.Status Action { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
