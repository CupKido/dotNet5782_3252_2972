using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {

        public DalObject()
        {
            DataSource.Initialize();
        }

        #region Customers

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude)
        {
            if (DataSource.Config.FirstDrone == 100)
            {
                throw new Exception("Customer Array Full");
            }
            int CN = DataSource.Config.FirstCustomer;
            
            for (int i = 0; i < CN; i++)
            {
                if (DataSource.Drones[i].Id == Id)
                {
                    throw new Exception("Id already taken");
                }
            }


            DataSource.Customers[CN].Id = Id;
            DataSource.Customers[CN].Name = Name;
            DataSource.Customers[CN].Phone = Phone;
            DataSource.Customers[CN].Longitude = Longitude;
            DataSource.Customers[CN].Latitude = Latitude;

            DataSource.Config.FirstCustomer++;
        }
        public IDAL.DO.Customer GetCustomer(int Id)
        {
            for (int i = 0; i < DataSource.Config.FirstCustomer; i++)
            {
                if (DataSource.Customers[i].Id == Id)
                    return DataSource.Customers[i];
            }
            throw new Exception("Customer Not Found!");
        }

        public List<IDAL.DO.Customer> GetAllCustomers()
        {
            List<IDAL.DO.Customer> list = new List<IDAL.DO.Customer>();
            for(int i = 0; i < DataSource.Config.FirstCustomer; i++)
            {
                list.Add(DataSource.Customers[i]);
            }
            return list;
        }

        public IDAL.DO.Drone RemoveCustomer(int Id)
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

        #endregion

        #region Drones
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
        public List<IDAL.DO.Drone> GetAllDrones()
        {
            List<IDAL.DO.Drone> list = new List<IDAL.DO.Drone>();

            for(int i =0; i< DataSource.Config.FirstDrone; i++)
            {
                list.Add(DataSource.Drones[i]);
            }
            return list;

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
        #endregion
    }
}
