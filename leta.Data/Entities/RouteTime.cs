using leta.Data.Entities;
using System;

namespace leta.Data
{
    public class RouteTime : Entity<int>
    {
        public RouteTime()
        {

        }

        public DateTime HoraDoDia { get; set; }

        public string DiaDaSemana { get; set; }

        public float Tempo { get; set; }

    }
}
