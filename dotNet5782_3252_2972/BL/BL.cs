using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public  List<IDAL.DO.Drone> Drones = new List<IDAL.DO.Drone>();
        IDAL.IDal dal ;
        public BL()
        {
            dal = new DalObject.DalObject();
            dal.AskForElectricity();
        }
    }
}
