using System;

namespace leta.Data.Entities
{
    public class InfoModelo : Entity<int>
    {
        public void Update(InfoModelo info)
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
