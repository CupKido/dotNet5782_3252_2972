using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
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
            try
            {
                GetCustomer(Id);
                throw new Exception("Id already taken");
            }
            catch
            {

            }
            
            Customer newCustomer = new Customer();

            newCustomer.Id = Id;
            newCustomer.Name = Name;
            newCustomer.Phone = Phone;
            newCustomer.Longitude = Longitude;
            newCustomer.Latitude = Latitude;
            DataSource.Customers.Add(newCustomer);
        }

        public void SetCustomer(Customer customer)
        {
            foreach (Customer exCustomer in DataSource.Customers)
            {
                if (exCustomer.Id == customer.Id)
                {
                    DataSource.Customers.Remove(exCustomer);
                    DataSource.Customers.Add(customer);
                    return;
                }
            }
            throw new Exception("Customer Not Found!");
        }

        public Customer GetCustomer(int Id)
        {
            foreach (Customer customer in DataSource.Customers)
            {
                if (customer.Id == Id)
                {
                    return customer;
                }
            }
            throw new Exception("Customer Not Found!");
        }

        public List<Customer> GetAllCustomers()
        {
            return DataSource.Customers;
        }

        public Customer RemoveCustomer(int Id)
        {
            foreach (Customer customer in DataSource.Customers)
            {
                if (customer.Id == Id)
                {
                    DataSource.Customers.Remove(customer);
                    return customer;
                }
            }
            throw new Exception("Customer Not Found!");
        }

        #endregion

        #region Drones

        public void AddDrone(int Id, String Model, IDAL.DO.WeightCategories MaxWeight)
        {

            try
            {
                GetDrone(Id);
                throw new Exception("Id already taken");
            }
            catch
            {

            }

            Drone newDrone = new Drone(); 

            newDrone.Id = Id;
            newDrone.Model = Model;
            newDrone.MaxWeight = MaxWeight;
            newDrone.Status = IDAL.DO.DroneStatuses.Availible;
            newDrone.Battery = 100;
            DataSource.Drones.Add(newDrone);
        }

        public List<IDAL.DO.Drone> GetAllDrones()
        {
            return DataSource.Drones;
        }

        public IDAL.DO.Drone GetDrone(int Id)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == Id)
                    return drone;
            }
            throw new Exception("Drone Not Found!");
        }

        public void SetDrone(IDAL.DO.Drone newDrone)
        {
            foreach (Drone exDrone in DataSource.Drones)
            {
                if (exDrone.Id == newDrone.Id)
                {
                    DataSource.Drones.Remove(exDrone);
                    DataSource.Drones.Add(newDrone);
                    return;
                }
            }
            throw new Exception("Drone Not Found!");
        }

        public IDAL.DO.Drone RemoveDrone(int Id)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == Id)
                {
                    DataSource.Drones.Remove(drone);
                    return drone;
                }
                
            }
            throw new Exception("Drone Not Found!");
        }
        #endregion

        #region Parcels

        public void AddParcel(int Id, int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority)
        {
            try
            {
                GetParcel(Id);
                throw new Exception("Id already taken");
            }
            catch
            {

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
            Parcel parcel = new Parcel();
            parcel.Id = Id;
            parcel.SenderId = SenderId;
            parcel.TargetId = TargetId;
            parcel.Weight = PackageWight;
            parcel.Priority = priority;
            parcel.Requested = DateTime.Now;
            DataSource.Parcels.Add(parcel);

        }

        public List<IDAL.DO.Parcel> GetAllParcels()
        {
            return DataSource.Parcels;
        }

        public IDAL.DO.Parcel GetParcel(int Id)
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id == Id)
                    return parcel;
            }
            throw new Exception("Package Not Found!");
        }

        public void SetParcel(IDAL.DO.Parcel newParcel)
        {
            foreach (Parcel exParcel in DataSource.Parcels)
            {
                if (exParcel.Id == newParcel.Id)
                {
                    DataSource.Parcels.Remove(exParcel);
                    DataSource.Parcels.Add(newParcel);
                    return;
                }
            }
            throw new Exception("Package Not Found!");
        }

        public Parcel RemoveParcel(int Id)
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id == Id)
                {
                    DataSource.Parcels.Remove(parcel);
                    return parcel;
                }
            }
            throw new Exception("Package Not Found!");
        }

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, double Longitude, double Latitude, int ChargeSlots)
        {
            try
            {
                GetBaseStation(Id);
                throw new Exception("Id already taken");
            }
            catch
            {

            }

            BaseStation baseStation = new BaseStation();

            baseStation.Id = Id;
            baseStation.Name = Name;
            baseStation.Longitude = Longitude;
            baseStation.Latitude = Latitude;
            baseStation.ChargeSlots = ChargeSlots;

            DataSource.BaseStations.Add(baseStation);
            
        }

        public List<IDAL.DO.BaseStation> GetAllBaseStations()
        {
            return DataSource.BaseStations;
        }

        public IDAL.DO.BaseStation GetBaseStation(int Id)
        {
            foreach (BaseStation baseStation in DataSource.BaseStations)
            {
                if (baseStation.Id == Id)
                    return baseStation;
            }
            throw new Exception("Base Station Not Found!");
        }


        #endregion

        #region Drone Charges

        public void AddDroneCharge(int DroneId, int BaseStationId)
        {
            IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge();

            droneCharge.DroneId = DroneId;
            droneCharge.BaseStationId = BaseStationId;

            DataSource.DroneCharges.Add(droneCharge);
        }

        public DroneCharge GetDroneCharge(int DroneId)
        {
            foreach(DroneCharge droneCharge in DataSource.DroneCharges)
            {
                if(droneCharge.DroneId == DroneId)
                {
                    return droneCharge;
                }
            }
            throw new Exception("drone " + DroneId + " is not being charged!");
        }

        public void SetDroneCharge(DroneCharge newDC)
        {
            DataSource.DroneCharges.Remove(GetDroneCharge(newDC.DroneId));
            DataSource.DroneCharges.Add(newDC);
        }

        public List<DroneCharge> GetAllDroneCharges()
        {
            return DataSource.DroneCharges;
        }


        public DroneCharge RemoveDroneCharge(int DroneId)
        {
            DroneCharge DC = GetDroneCharge(DroneId);
            DataSource.DroneCharges.Remove(DC);
            return DC;
        }

        #endregion
    }
}
