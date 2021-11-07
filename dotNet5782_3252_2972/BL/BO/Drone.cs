using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }

        public String Model { get; set; }

        public IDAL.DO.WeightCategories MaxWeight { get; set; }

        public DroneStatuses Status { get; set; }
        
        public double Battery { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return "ID: " + Id + "\nModel: " + Model + "\nMax Weight: " + MaxWeight + "\nStatus: " /*+ Status + "\nBattery: " + Battery*/;
        }
    }
}
