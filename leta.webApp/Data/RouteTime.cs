using System;

namespace leta.webApp.Data
{
    public class RouteTime
    {
        public int Id { get; set; }
        public DateTime DiaDoMes { get; set; }
        public string DiaDaSemana { get; set; }
        public int Tempo { get; set; }
        public TimeSpan HoraDoDia { get; set; }
    }
}
