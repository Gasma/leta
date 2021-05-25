using leta.Data.Entities;
using System;

namespace leta.Data
{
    public class RouteTime : Entity<int>
    {

        public DateTime TimeOfDay { get; set; }

        public float Time { get; set; }

    }
}
