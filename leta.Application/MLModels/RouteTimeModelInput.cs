using Microsoft.ML.Data;

namespace leta.Application.MLModels
{
    public class RouteTimeModelInput
    {
        [ColumnName("ano"), LoadColumn(0)]
        public float Ano { get; set; }

        [ColumnName("diaSemana"), LoadColumn(1)]
        public float DiaSemana { get; set; }

        [ColumnName("tempo"), LoadColumn(2)]
        public float Tempo { get; set; }

        [ColumnName("hora"), LoadColumn(3)]
        public float Hora { get; set; }

        [ColumnName("mes"), LoadColumn(4)]
        public float Mes { get; set; }

    }
}
