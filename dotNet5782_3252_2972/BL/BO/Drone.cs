using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone : IComparable
    {
        public int Id { get; set; }

        public String Model { get; set; }

        public WeightCategories MaxWeight { get; set; }

        public DroneStatuses Status { get; set; }

        public double Battery { get; set; }

        public Location CurrentLocation = new Location();

        public ParcelInDelivery CurrentParcel { get; set; }

        public override string ToString()
        {
            return "ID: " + Id + "   Model: " + Model + "\nMax Weight: " + MaxWeight + "     Status: " + Status + "\nBattery: " + Battery + "\nLocation: " + CurrentLocation + ((CurrentParcel == null) ? "" : ("\nCurrent Parcel: " + CurrentParcel.Id));
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Drone)obj).Id);
        }
    }
}
