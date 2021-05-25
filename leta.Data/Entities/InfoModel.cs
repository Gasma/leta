using System;

namespace leta.Data.Entities
{
    public class InfoModel : Entity<int>
    {
        public void Update(InfoModel info)
        {
            Update(info. Message, info.LastTraining, info.QuantData);
        }        
        
        public void Update(string Message, DateTime? LastTraining, int QuantData)
        {
            this.Message = Message;
            this.LastTraining = LastTraining;
            this.QuantData = QuantData;
        }
        public string Message { get; set; }
        public DateTime? LastTraining { get; set; }
        public int QuantData { get; set; }
    }
}
