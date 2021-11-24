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

        /// <summary>
        /// Adds a customer to the data base
        /// </summary>
        /// <param name="Id">Customer's ID</param>
        /// <param name="Name">Customer's name</param>
        /// <param name="Phone">Customer's phone number as string</param>
        /// <param name="Longitude">Customer's address's longitude</param>
        /// <param name="Latitude">Customer's address's latitude</param>
        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);



        /// <summary>
        /// Updates the Customer's name and phone in the data base by ID
        /// </summary>
        /// <param name="Id">Customer's ID</param>
        /// <param name="name">Customer's new name</param>
        /// <param name="phone">Customer's new phone number as string</param>
        public void UpdateCustomer(int Id, string name, string phone);


        /// <summary>
        /// returns the customer with the specified ID from the data base
        /// </summary>
        /// <param name="Id">The wanted customer's ID</param>
        /// <returns></returns>

        public Customer GetCustomer(int Id);


        /// <summary>
        /// returns all customers in data base as BL customers
        /// </summary>
        /// <returns>all customers in data base as BL customers</returns>

        public IEnumerable<CustomerToList> GetAllCustomers();

        //public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        /// <summary>
        /// Adds a drone to the data base
        /// </summary>
        /// <param name="Id">Drone's ID</param>
        /// <param name="Model">Drone's model</param>
        /// <param name="MaxWeight">Drone's maximum weight</param>
        /// <param name="stationId">Drone's starting point (as a station id number)</param>
        public void AddDrone(int Id, string Model, WeightCategories MaxWeight, int stationId);


        /// <summary>
        /// returns all drones in data base as BL drones
        /// </summary>
        /// <returns>all drones in data base as BL drones</returns>
        public IEnumerable<DroneToList> GetAllDrones();

        /// <summary>
        /// returns all drones in data base as BL drones if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order to be returned</param>
        /// <returns>all drones in data base as BL drones if they're up to the condition in the predicate</returns>
        public IEnumerable<DroneToList> GetAllDronesBy(Predicate<Drone> predicate);

        /// <summary>
        /// gets all the required data and returns the drone with the specified ID
        /// </summary>
        /// <param name="Id">the wanted drone's ID</param>
        /// <returns>the wanted drone as a BL drone</returns>
        public Drone GetDrone(int Id);

        /// <summary>
        /// updates a drone's model
        /// </summary>
        /// <param name="Id">the wanted drone's ID</param>
        /// <param name="Model">the model the new drone will be as</param>
        public void UpdateDrone(int Id, string Model);


        /// <summary>
        /// sends a drone to get charged in the nearest base station
        /// </summary>
        /// <param name="Id">the wanted drone's ID</param>
        public void ChargeDrone(int Id);

        /// <summary>
        /// takes the specified drone out of charging
        /// </summary>
        /// <param name="id">the wanted drone's ID</param>
        /// <param name="time">the amount of time the drone was in charging</param>
        public void DisChargeDrone(int id, float time);

        /// <summary>
        /// assosiates a drone to a package
        /// </summary>
        /// <param name="id">the wanted drone's id to assosiate with</param>
        public void AttributionParcelToDrone(int id);

        /// <summary>
        /// picks up a package from a customer
        /// </summary>
        /// <param name="id">the wanted drone to pick up a package</param>
        public void PickUpParcelByDrone(int id);

        /// <summary>
        /// deliver a parcel to it's target
        /// </summary>
        /// <param name="id"></param>
        public void SupplyParcel(int id);

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
