using leta.Data.Entities;
using System;

namespace leta.Data
{
    public class RouteTime : Entity<int>
    {

        public DateTime HoraDoDia { get; set; }

        public int DiaDaSemana { get; set; }

        public float Tempo { get; set; }

    }
}
