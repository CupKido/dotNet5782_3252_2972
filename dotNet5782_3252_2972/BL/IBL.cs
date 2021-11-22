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

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);

        public void UpdateCustomer(int Id, string name, string phone);

        public Customer GetCustomer(int Id);

        public IEnumerable<CustomerToList> GetAllCustomers();

        //public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        public void AddDrone(int Id, string Model, WeightCategories MaxWeight, int stationId);

        public IEnumerable<DroneToList> GetAllDrones();
        public IEnumerable<DroneToList> GetAllDronesBy(Predicate<Drone> predicate);

        public Drone GetDrone(int Id);

        public void UpdateDrone(int Id, string Model);

        public void ChargeDrone(int Id);
        public void DisChargeDrone(int id, float time);
        public void AttributionParcelToDrone(int id);

        //public IDAL.DO.Drone GetDrone(int Id);

        //public void SetDrone(IDAL.DO.Drone newDrone);

        //public IDAL.DO.Drone RemoveDrone(int Id);

        //public Double[] AskForElectricity();

        #endregion

        #region Parcels

        public void AddParcel( int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority);

        public IEnumerable<ParcelToList> GetAllParcels();

        public IEnumerable<ParcelToList> GetParcelsWithNoDrone();

        public Parcel GetParcel(int Id);

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

        public IEnumerable<BaseStationToList> GetAllBaseStationsBy(Predicate<BaseStation> predicate);

        public void UpdateBaseStation(int Id, string Name, int? ChargeSlots);

        public BaseStation GetBaseStation(int Id);
        

        //public IDAL.DO.BaseStation GetBaseStation(int Id);

        #endregion

    }
}
