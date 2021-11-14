using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class DroneToList
    {
        public int Id { get; set; }
        
        public String Model { get; set; }

        public WeightCategories Weight { get; set; }

        public double Battery { get; set; }

        public DroneStatuses Status { get; set; }

        public Location CurrentLocation { get; set; }

        public int CarriedParcelId { get; set; }
    }
}
