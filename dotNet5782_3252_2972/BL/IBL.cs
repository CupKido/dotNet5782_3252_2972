using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi  
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


        public void DeleteCustomer(int Id);
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
        /// <param name="predicate">the condition in order for the drone to be returned</param>
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

        public void DeleteDrone(int Id);

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

        public DroneToList TurnDroneToList(Drone drone);

        #endregion

        #region Parcels

        /// <summary>
        /// Adds a parcel to the data base
        /// </summary>
        /// <param name="SenderId">the package sender's ID</param>
        /// <param name="TargetId">the package reciever ID</param>
        /// <param name="PackageWight">the weight of the package</param>
        /// <param name="priority">how urgent is getting the package to the target</param>
        public int AddParcel( int SenderId, int TargetId, BO.WeightCategories PackageWight, BO.Priorities priority);

        /// <summary>
        /// returns all parcels in data base, as BL parcels
        /// </summary>
        /// <returns>All parcels in data base, as BL parcels</returns>
        public IEnumerable<ParcelToList> GetAllParcels();

        /// <summary>
        /// returns all parcels that have not been assosiated with a drone
        /// </summary>
        /// <returns>all parcels that have not been assosiated with a drone</returns>
        public IEnumerable<ParcelToList> GetParcelsWithNoDrone();

        /// <summary>
        /// returns a parcel with the specified ID from the data base
        /// </summary>
        /// <param name="Id">the wanted parcel's ID</param>
        /// <returns>A parcel with the specified ID from the data base</returns>
        public Parcel GetParcel(int Id);

        public void DeleteParcel(int Id);

        public ParcelToList TurnParcelToList(Parcel parcel);

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

        /// <summary>
        /// Adds a base station to the data base
        /// </summary>
        /// <param name="Id">the base station's ID number</param>
        /// <param name="Name">The base Station's name</param>
        /// <param name="StationLocation">The base station's location</param>
        /// <param name="ChargeSlots">the amount of charge slots that the new base station has</param>
        public void AddBaseStations(int Id, String Name, Location StationLocation, int ChargeSlots);

        /// <summary>
        /// returns all base stations in the data base as BaseToList
        /// </summary>
        /// <returns>All base stations in the data base as BaseStationToList</returns>
        public IEnumerable<BaseStationToList> GetAllBaseStations();

        /// <summary>
        /// returns all base stations in data base as BaseStationToList if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">The condition in order for the base station to be returned</param>
        /// <returns>All base stations in data base as BaseStationToList if they're up to the condition in the predicate</returns>
        public IEnumerable<BaseStationToList> GetAllBaseStationsBy(Predicate<BaseStation> predicate);

        public IEnumerable<BaseStationToList> GetAvailibleBaseStations();

        /// <summary>
        /// updates the name and the amount slots for a base station in the data base
        /// </summary>
        /// <param name="Id">the wanted base station's ID to update</param>
        /// <param name="Name">The new name for the Base station</param>
        /// <param name="ChargeSlots">the new amout of charge slots for the base station</param>
        public void UpdateBaseStation(int Id, string Name, int? ChargeSlots);

        /// <summary>
        /// gathers all the required information, and returns the base station with the specified ID from data base as a BL BaseStation
        /// </summary>
        /// <param name="Id">The wanted base station's ID to return</param>
        /// <returns>The base station with the specified ID from data base as a BL BaseStation</returns>
        public BaseStation GetBaseStation(int Id);
        

        //public IDAL.DO.BaseStation GetBaseStation(int Id);

        #endregion

    }
}
