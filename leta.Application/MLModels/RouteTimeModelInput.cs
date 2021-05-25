using Microsoft.ML.Data;

namespace leta.Application.MLModels
{
    public class RouteTimeModelInput
    {
        [ColumnName("Year"), LoadColumn(0)]
        public float Year { get; set; }

        [ColumnName("WeekDay"), LoadColumn(1)]
        public float WeekDay { get; set; }

        [ColumnName("Time"), LoadColumn(2)]
        public float Time { get; set; }

        [ColumnName("Hour"), LoadColumn(3)]
        public float Hour { get; set; }

        [ColumnName("Month"), LoadColumn(4)]
        public float Month { get; set; }

    }
}
