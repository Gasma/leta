using System;

namespace leta.Data.Entities
{
    public class InfoModel : Entity<int>
    {
        public void Update(InfoModel info)
        {
            Update(info. Mensagem, info.UltimoTreinamento, info.QuantDados);
        }        
        
        public void Update(string Mensagem, DateTime? UltimoTreinamento, int QuantDados)
        {
            this.Mensagem = Mensagem;
            this.UltimoTreinamento = UltimoTreinamento;
            this.QuantDados = QuantDados;
        }
        public string Mensagem { get; set; }
        public DateTime? UltimoTreinamento { get; set; }
        public int QuantDados { get; set; }
    }
}
