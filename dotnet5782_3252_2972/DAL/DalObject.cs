using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    internal class DalObject : DalApi.IDal
    {
        internal static DalObject instance = null;
        private static object locker = new object();
        private DalObject()
        {
            DataSource.Initialize();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static DalObject GetInstance()
        {

            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new DalObject();
                    }
                }
            }
            return instance;
        }


        #region Customers

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude)
        {
            try
            {
                GetCustomer(Id);
                throw new ItemAlreadyExistsException(Id, "Customer Id already taken");
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers()
        {
            return DataSource.Customers;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate)
        {
            return from Customer c in GetAllCustomers()
                   where predicate(c)
                   select c;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return from Drone d in GetAllDrones()
                   where predicate(d)
                   select d;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(int SenderId, int TargetId, DO.WeightCategories PackageWight, DO.Priorities priority, DateTime created)
        {

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
            parcel.Id = DataSource.Config.runningNumForParcels;
            DataSource.Config.runningNumForParcels++;
            parcel.SenderId = SenderId;
            parcel.TargetId = TargetId;
            parcel.Weight = PackageWight;
            parcel.Priority = priority;
            parcel.Requested = created;
            DataSource.Parcels.Add(parcel);
            return parcel.Id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels()
        {
            return DataSource.Parcels;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcelsBy(Predicate<Parcel> predicate)
        {
            return from Parcel p in GetAllParcels()
                   where predicate(p)
                   select p;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Parcel GetParcel(int Id)
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id == Id)
                    return parcel;
            }
            throw new ItemNotFoundException(Id, "Package Not Found!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            return DataSource.BaseStations;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate)
        {
            return from BaseStation b in GetAllBaseStations()
                   where predicate(b)
                   select b;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.BaseStation GetBaseStation(int Id)
        {
            foreach (BaseStation baseStation in DataSource.BaseStations)
            {
                if (baseStation.Id == Id)
                    return baseStation;
            }
            throw new ItemNotFoundException(Id, "Base Station Not Found!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(int DroneId, int BaseStationId, DateTime started)
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
            droneCharge.Started = started;

            DataSource.DroneCharges.Add(droneCharge);
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetDroneCharge(DroneCharge newDC)
        {
            DataSource.DroneCharges.Remove(GetDroneCharge(newDC.DroneId));
            DataSource.DroneCharges.Add(newDC);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDroneCharges()
        {
            return DataSource.DroneCharges;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge RemoveDroneCharge(int DroneId)
        {
            DroneCharge DC = GetDroneCharge(DroneId);
            DataSource.DroneCharges.Remove(DC);

            return DC;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate)
        {
            return from DroneCharge dc in GetAllDroneCharges()
                   where predicate(dc)
                   select dc;
        }

        

        #endregion
    }
}
