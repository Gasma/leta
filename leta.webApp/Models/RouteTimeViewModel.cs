using Microsoft.ML.Data;
using System;

namespace leta.webApp.Models
{
    public class RouteTimeViewModel
    {
        public int Id { get; set; }

        [ColumnName("Hora Do Dia"), LoadColumn(0)]
        public DateTime HoraDoDia { get; set; }

        [ColumnName("Dia semana"), LoadColumn(1)]
        public string DiaDaSemana { get; set; }


        [ColumnName("Tempo Min"), LoadColumn(2)]
        public float Tempo { get; set; }
    }
}
