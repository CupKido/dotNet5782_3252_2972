﻿using System;
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

        public void SetCustomer(IDAL.DO.Customer customer)
        {
            for(int i = 0; i < DataSource.Config.FirstCustomer; i++)
            {
                if(DataSource.Customers[i].Id == customer.Id)
                {
                    DataSource.Customers[i] = customer;
                }
            }
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
            for(int i = 0; i < DataSource.Config.FirstDrone; i++)
            {
                if (DataSource.Drones[i].Id == Id)
                    return DataSource.Drones[i];
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

        #region Parcels

        public void AddParcel(int Id, int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority)
        {
            if (DataSource.Config.FirstParcel == 10)
            {
                throw new Exception("Parcel Array Full");
            }
            int PN = DataSource.Config.FirstParcel;

            for (int i = 0; i < PN; i++)
            {
                if (DataSource.Parcels[i].Id == Id)
                {
                    throw new Exception("Id already taken");
                }
            }

            if (SenderId == TargetId)
                throw new Exception("Can't send a Package to yourself!");

            try
            {
                GetCustomer(SenderId);
            }
            catch
            {
                throw new Exception("The Sender is not signed in the system");
            }
            try
            {
                GetCustomer(TargetId);
            }
            catch
            {
                throw new Exception("Target customer is not signed in the system");
            }

            DataSource.Parcels[PN].Id = Id;
            DataSource.Parcels[PN].SenderId = SenderId;
            DataSource.Parcels[PN].TargetId =TargetId;
            DataSource.Parcels[PN].Weight = PackageWight;
            DataSource.Parcels[PN].Priority = priority;
            DataSource.Parcels[PN].Requested = DateTime.Now;


            DataSource.Config.FirstParcel++;
        }

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, double Longitude, double Latitude, int ChargeSlots)
        {
            if (DataSource.Config.FirstBaseStation == 5)
            {
                throw new Exception("Base Stations Array Full");
            }
            int BSN = DataSource.Config.FirstBaseStation;

            for (int i = 0; i < BSN; i++)
            {
                if (DataSource.BaseStations[i].Id == Id)
                {
                    throw new Exception("Id already taken");
                }
            }


            DataSource.BaseStations[BSN].Id = Id;
            DataSource.BaseStations[BSN].Name = Name;
            DataSource.BaseStations[BSN].Longitude = Longitude;
            DataSource.BaseStations[BSN].Latitude = Latitude;
            DataSource.BaseStations[BSN].ChargeSlots = ChargeSlots;


            DataSource.Config.FirstBaseStation++;
        }

        public List<IDAL.DO.BaseStation> GetAllBaseStations()
        {
            List<IDAL.DO.BaseStation> list = new List<IDAL.DO.BaseStation>();

            for (int i = 0; i < DataSource.Config.FirstBaseStation; i++)
            {
                list.Add(DataSource.BaseStations[i]);
            }
            return list;

        }

        public IDAL.DO.BaseStation GetBaseStation(int Id)
        {
            for (int i = 0; i < DataSource.Config.FirstBaseStation; i++)
            {
                if (DataSource.BaseStations[i].Id == Id)
                    return DataSource.BaseStations[i];
            }
            throw new Exception("Base Station Not Found!");
        }


        #endregion

        #region Drone Charges

        public List<IDAL.DO.DroneCharge> GetAllDroneCharges()
        {
            List<IDAL.DO.DroneCharge> list = new List<IDAL.DO.DroneCharge>();

            return list;
        }

        #endregion
    }
}
