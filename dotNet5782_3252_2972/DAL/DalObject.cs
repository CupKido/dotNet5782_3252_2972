using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject : IDAL.IDal
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
                throw new ItemAlreadyExistsException(Id, "Id already taken");
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw;
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
            throw new ItemNotFoundException(customer.Id, "Customer Not Found!");
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
            throw new ItemNotFoundException(Id, "Customer Not Found!");
        }

        public IEnumerable<Customer> GetAllCustomers()
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
            throw new ItemNotFoundException(Id, "Customer Not Found!");
        }

        #endregion

        #region Drones

        public void AddDrone(int Id, String Model, WeightCategories MaxWeight)
        {

            try
            {
                GetDrone(Id);
                throw new ItemAlreadyExistsException(Id, "Id already taken");
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw;
            }
            catch
            {

            }

            Drone newDrone = new Drone(); 

            newDrone.Id = Id;
            newDrone.Model = Model;
            newDrone.MaxWeight = MaxWeight;
            //newDrone.Status = IDAL.DO.DroneStatuses.Availible;
            //newDrone.Battery = 100;
            DataSource.Drones.Add(newDrone);
        }

        public IEnumerable<Drone> GetAllDrones()
        {
            return DataSource.Drones;
        }

        public Drone GetDrone(int Id)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == Id)
                    return drone;
            }
            throw new ItemNotFoundException(Id, "Drone Not Found!");
        }

        public void SetDrone(Drone newDrone)
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
            throw new ItemNotFoundException(newDrone.Id, "Drone Not Found!");
        }

        public Drone RemoveDrone(int Id)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == Id)
                {
                    DataSource.Drones.Remove(drone);
                    return drone;
                }
                
            }
            throw new ItemNotFoundException(Id, "Drone Not Found!");
        }

        public Double[] AskForElectricity()
        {
            double[] array = { DataSource.Config.AvailbleElec, DataSource.Config.LightElec, DataSource.Config.IntermediateElec, DataSource.Config.HeavyElec, DataSource.Config.ChargePerHours };
            return array;
        }
        #endregion

        #region Parcels

        public void AddParcel(int Id, int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority, DateTime created)
        {
            try
            {
                GetParcel(Id);
                throw new ItemAlreadyExistsException(Id, "Parcel Id already taken");
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw;
            }
            catch
            {

            }

            if (SenderId == TargetId)
                throw new IllegalActionException("Can't send a Package to yourself!");

            try
            {
                GetCustomer(SenderId);
            }
            catch
            {
                throw new IllegalActionException("The Sender is not signed in the system");
            }
            try
            {
                GetCustomer(TargetId);
            }
            catch
            {
                throw new IllegalActionException("Target customer is not signed in the system");
            }
            Parcel parcel = new Parcel();
            parcel.Id = Id;
            parcel.SenderId = SenderId;
            parcel.TargetId = TargetId;
            parcel.Weight = PackageWight;
            parcel.Priority = priority;
            parcel.Requested = created;
            DataSource.Parcels.Add(parcel);

        }

        public IEnumerable<Parcel> GetAllParcels()
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
            throw new ItemNotFoundException(Id, "Package Not Found!");
        }

        public void SetParcel(Parcel newParcel)
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
            throw new ItemNotFoundException(newParcel.Id, "Package Not Found!");
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
            throw new ItemNotFoundException(Id, "Package Not Found!");
        }

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, double Longitude, double Latitude, int ChargeSlots)
        {
            try
            {
                GetBaseStation(Id);
                throw new ItemAlreadyExistsException(Id, "Base Station Id already taken");
            }
            catch(ItemAlreadyExistsException ex)
            {
                throw;
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

        public IEnumerable<BaseStation> GetAllBaseStations()
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
            throw new ItemNotFoundException(Id, "Base Station Not Found!");
        }

        public void SetBaseStation(BaseStation newBS)
        {
            foreach (BaseStation exBS in DataSource.BaseStations)
            {
                if (exBS.Id == newBS.Id)
                {
                    DataSource.BaseStations.Remove(exBS);
                    DataSource.BaseStations.Add(newBS);
                    return;
                }
            }
            throw new ItemNotFoundException(newBS.Id, "Base Station Not Found!");
        }

        public BaseStation RemoveBaseStation(int Id)
        {
            try
            {
                BaseStation toDeleteBS = GetBaseStation(Id);
                DataSource.BaseStations.Remove(toDeleteBS);
                return toDeleteBS;
            }
            catch
            {
                throw;
            }

        }

        #endregion

        #region Drone Charges

        public void AddDroneCharge(int DroneId, int BaseStationId)
        {
            try
            {
                GetDroneCharge(DroneId);
                throw new ItemAlreadyExistsException(DroneId, "Drone is already being charged");
            }
            catch (ItemAlreadyExistsException ex)
            {
                throw;
            }
            catch { }
            DroneCharge droneCharge = new DroneCharge();

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
            throw new ItemNotFoundException(DroneId, "Drone is not being charged!");
        }

        public void SetDroneCharge(DroneCharge newDC)
        {
            DataSource.DroneCharges.Remove(GetDroneCharge(newDC.DroneId));
            DataSource.DroneCharges.Add(newDC);
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges()
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
