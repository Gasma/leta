using Microsoft.ML.Data;

namespace leta.Application.MLModels
{
    public class RouteTimeModelInput
    {
        [ColumnName("Ano"), LoadColumn(0)]
        public float Ano { get; set; }

        [ColumnName("DiaSemana"), LoadColumn(1)]
        public float DiaSemana { get; set; }

        [ColumnName("Tempo"), LoadColumn(2)]
        public float Tempo { get; set; }

        [ColumnName("Hora"), LoadColumn(3)]
        public float Hora { get; set; }

        [ColumnName("Mes"), LoadColumn(4)]
        public float Mes { get; set; }

    }
}
