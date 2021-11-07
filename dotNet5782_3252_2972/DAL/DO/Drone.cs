using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public String Model { get; set; }
        public IDAL.DO.WeightCategories MaxWeight { get; set; }
        //public IDAL.DO.DroneStatuses Status { get; set; }
        //public double Battery { get; set; }

        public override string ToString()
        {
            return "ID: " + Id + "\nModel: " + Model + "\nMax Weight: " + MaxWeight + "\nStatus: " /*+ Status + "\nBattery: " + Battery*/;
        }
    }
}
