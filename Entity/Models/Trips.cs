using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Trips
    {
        public string Id { get; set; }
        public int? ClientId { get; set; }
        public int? DriverId { get; set; }
        public int? CityId { get; set; }
        public string Status { get; set; }
        public DateTime? RequestAt { get; set; }
    }
}
