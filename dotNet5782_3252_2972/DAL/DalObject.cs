using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public void AddDrone(int Id, String Model, IDAL.DO.WeightCategories MaxWeight)
        {
            if (DataSource.Config.FirstDrone == 10)
            {
                throw new Exception("Drone Array Full");
            }
            int DN = DataSource.Config.FirstDrone;

            for (int i = 0; i < DN; i++)
            {
                if (DataSource.Drones[i].Id == Id)
                {
                    throw new Exception("Id already taken");
                }
            }


            DataSource.Drones[DN].Id = Id;
            DataSource.Drones[DN].Model = Model;
            DataSource.Drones[DN].MaxWeight = MaxWeight;
            DataSource.Drones[DN].Status = IDAL.DO.DroneStatuses.Availible;
            DataSource.Drones[DN].Battery = 100;

            DataSource.Config.FirstDrone++;
        }
        public IDAL.DO.Drone GetDrone(int Id)
        {
            foreach(IDAL.DO.Drone DR in DataSource.Drones)
            {
                if (DR.Id == Id)
                    return DR;
            }
            throw new Exception("Drone Not Found!");
        }
        public IDAL.DO.Drone RemoveDrone(int Id)
        {
            for (int i = 0; i < DataSource.Config.FirstDrone; i++)
            {
                if (DataSource.Drones[i].Id == Id)
                {
                    //Add Func in next targil
                }
            }
            throw new Exception("Drone Not Found!");
        }

    }
}
