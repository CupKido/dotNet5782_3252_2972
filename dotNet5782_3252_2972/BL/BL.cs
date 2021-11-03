using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public  List<IBL.BO.Drone> Drones = new List<IBL.BO.Drone>();
        public IDAL.IDal dal ;
        public Double AvailbleElec { get; set; }
        public Double LightElec { get; set; }
        public Double IntermediateElec { get; set; }
        public Double HeavyElec { get; set; }
        public Double ChargePerHours { get; set; }
        public BL()
        {
            dal = new DalObject.DalObject();
            double[] arr= dal.AskForElectricity();
            AvailbleElec = arr[0];
            LightElec = arr[1];
            IntermediateElec = arr[2];
            HeavyElec = arr[3];
            ChargePerHours = arr[4];
            List<IDAL.DO.Drone> DalDrones = dal.GetAllDrones();
        

        }
    }
}
