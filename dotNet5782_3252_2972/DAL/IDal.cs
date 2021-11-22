using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        #region Customers

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);

        public void SetCustomer(Customer customer);

        public Customer GetCustomer(int Id);

        public IEnumerable<Customer> GetAllCustomers();

        public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        public void AddDrone(int Id, String Model, IDAL.DO.WeightCategories MaxWeight);

        public IEnumerable<Drone> GetAllDrones();

        public IDAL.DO.Drone GetDrone(int Id);

        public void SetDrone(IDAL.DO.Drone newDrone);

        public IDAL.DO.Drone RemoveDrone(int Id);

        public Double[] AskForElectricity();

        #endregion

        #region Parcels

        public void AddParcel(int Id, int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority, DateTime created);

        public IEnumerable<Parcel> GetAllParcels();

        public IDAL.DO.Parcel GetParcel(int Id);

        public void SetParcel(IDAL.DO.Parcel newParcel);

        public Parcel RemoveParcel(int Id);

        #endregion

        #region Drone Charges

        public void AddDroneCharge(int DroneId, int BaseStationId);

        public DroneCharge GetDroneCharge(int DroneId);

        public void SetDroneCharge(DroneCharge newDC);

        public IEnumerable<DroneCharge> GetAllDroneCharges();

        public DroneCharge RemoveDroneCharge(int DroneId);

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, double Longitude, double Latitude, int ChargeSlots);

        public IEnumerable<IDAL.DO.BaseStation> GetAllBaseStations();
        public IDAL.DO.BaseStation GetBaseStation(int Id);

        public void SetBaseStation(BaseStation newBS);

        public BaseStation RemoveBaseStation(int Id);
        #endregion
    }
}
