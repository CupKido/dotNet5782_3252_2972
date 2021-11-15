using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    public interface IBL
    {

        #region Customers

        //public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);

        //public void SetCustomer(Customer customer);

        //public Customer GetCustomer(int Id);

        //public List<Customer> GetAllCustomers();

        //public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        //public void AddDrone(int Id, String Model, IDAL.DO.WeightCategories MaxWeight);

        //public List<IDAL.DO.Drone> GetAllDrones();

        //public IDAL.DO.Drone GetDrone(int Id);

        //public void SetDrone(IDAL.DO.Drone newDrone);

        //public IDAL.DO.Drone RemoveDrone(int Id);

        //public Double[] AskForElectricity();

        #endregion

        #region Parcels

        //public void AddParcel(int Id, int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority);

        //public List<IDAL.DO.Parcel> GetAllParcels();

        //public IDAL.DO.Parcel GetParcel(int Id);

        //public void SetParcel(IDAL.DO.Parcel newParcel);

        //public Parcel RemoveParcel(int Id);

        #endregion

        #region Drone Charges

        //public void AddDroneCharge(int DroneId, int BaseStationId);

        //public DroneCharge GetDroneCharge(int DroneId);

        //public void SetDroneCharge(DroneCharge newDC);

        //public List<DroneCharge> GetAllDroneCharges();

        //public DroneCharge RemoveDroneCharge(int DroneId);

        #endregion

        #region Base Stations

        public void AddBaseStations(int Id, String Name, Location StationLocation, int ChargeSlots);

        public IEnumerable<BaseStationToList> GetAllBaseStations();

        public BaseStation GetBaseStation(int Id);

        public Drone GetDrone(int Id);

        //public IDAL.DO.BaseStation GetBaseStation(int Id);

        #endregion

    }
}
