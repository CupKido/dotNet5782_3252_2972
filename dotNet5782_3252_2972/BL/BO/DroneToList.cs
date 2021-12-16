using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneToList : IComparable
    {
        public int Id { get; set; }
        
        public String Model { get; set; }

        public WeightCategories MaxWeight { get; set; }

        public double Battery { get; set; }

        public DroneStatuses Status { get; set; }

        public Location CurrentLocation { get; set; }

        public int CarriedParcelId { get; set; }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((DroneToList)obj).Id);
        }

        public override string ToString()
        {
            return "ID: " + Id + "  Model: " + Model + "  Max Weight: " + MaxWeight + "  Status: " + Status + "  Battery: " + Battery;
        }
    }
}
