using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Customers

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);

        public void SetCustomer(Customer customer);

        public Customer GetCustomer(int Id);

        public IEnumerable<Customer> GetAllCustomers();

        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate);

        public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        public void AddDrone(int Id, String Model, DO.WeightCategories MaxWeight);

        public IEnumerable<Drone> GetAllDrones();

        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate);

        public DO.Drone GetDrone(int Id);

        public void SetDrone(DO.Drone newDrone);

        public DO.Drone RemoveDrone(int Id);

        public Double[] AskForElectricity();

        #endregion

        #region Parcels

        public void AddParcel(int Id, int SenderId, int TargetId, DO.WeightCategories PackageWight, DO.Priorities priority, DateTime created);

        public IEnumerable<Parcel> GetAllParcels();

        public IEnumerable<Parcel> GetAllParcelsBy(Predicate<Parcel> predicate);

        public DO.Parcel GetParcel(int Id);

        public void SetParcel(DO.Parcel newParcel);

        public Parcel RemoveParcel(int Id);

        #endregion

        #region Drone Charges

        public void AddDroneCharge(int DroneId, int BaseStationId, DateTime started);

        public DroneCharge GetDroneCharge(int DroneId);

        public void SetDroneCharge(DroneCharge newDC);

        public IEnumerable<DroneCharge> GetAllDroneCharges();

        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate);

        public DroneCharge RemoveDroneCharge(int DroneId);

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, double Longitude, double Latitude, int ChargeSlots);

        public IEnumerable<BaseStation> GetAllBaseStations();
        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate);
        public DO.BaseStation GetBaseStation(int Id);

        public void SetBaseStation(BaseStation newBS);

        public BaseStation RemoveBaseStation(int Id);
        #endregion
    }
}
