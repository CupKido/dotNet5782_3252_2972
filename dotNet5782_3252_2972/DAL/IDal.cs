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

        /// <summary>
        /// add customer to the database
        /// </summary>
        /// <param name="Id">customer's ID</param>
        /// <param name="Name">customer's name</param>
        /// <param name="Phone">customer's phone number</param>
        /// <param name="Longitude">customer's Longitude position</param>
        /// <param name="Latitude">customer's Latitude position</param>
        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude);


        /// <summary>
        /// set customr's detailes into the data file
        /// </summary>
        /// <param name="customer">customer to put in the file</param>
        public void SetCustomer(Customer customer);


        /// <summary>
        /// get a customer by his ID
        /// </summary>
        /// <param name="Id">customer's ID</param>
        /// <returns>return DAL customer whom this ID belongs </returns>
        public Customer GetCustomer(int Id);


        /// <summary>
        /// returns all customers as a DAL customers
        /// </summary>
        /// <returns>all customers as a DAL customers</returns>
        public IEnumerable<Customer> GetAllCustomers();


        /// <summary>
        /// returns all customers in data base as DAL customers if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order for the customer to be returned</param>
        /// <returns>all customers in data base as DAL customers if they're up to the condition in the predicate</returns>
        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate);

        /// <summary>
        /// remove customer whom this ID belongs from the database-files
        /// </summary>
        /// <param name="Id">Id of specific customer</param>
        /// <returns>the removed customer</returns>
        public Customer RemoveCustomer(int Id);

        #endregion

        #region Drones

        /// <summary>
        /// add drone to the database, to the drone's file
        /// </summary>
        /// <param name="Id">Drones Id </param>
        /// <param name="Model">Model of drone</param>
        /// <param name="MaxWeight">max weight the drone can carry</param>
        public void AddDrone(int Id, String Model, DO.WeightCategories MaxWeight);

        /// <summary>
        /// get all drones as DAL drones from the drone's file
        /// </summary>
        /// <returns>all drones exist in database</returns>
        public IEnumerable<Drone> GetAllDrones();

        /// <summary>
        /// returns all drones in data base as DAL drones if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order for the drones to be returned</param>
        /// <returns>all drones who meet the condition</returns>
        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate);

        /// <summary>
        /// get a specipic drone ehom this Id belongs
        /// </summary>
        /// <param name="Id">Id of the wanted drone</param>
        /// <returns>drone whom the Id belongs</returns>
        public DO.Drone GetDrone(int Id);

        /// <summary>
        /// set a drone into the database in the drone's file
        /// </summary>
        /// <param name="newDrone">the drone to save in the database</param>
        public void SetDrone(DO.Drone newDrone);

        /// <summary>
        /// remove specific drone from the database, a drone ehom the given Id belongs
        /// </summary>
        /// <param name="Id">dron's Id</param>
        /// <returns>removed drone</returns>
        public DO.Drone RemoveDrone(int Id);

        /// <summary>
        /// return an array with the details of electricity usage rate and charging rate:  
        /// Availiable Electricity, Light Electricity, Intermediate Electricity, Heavy Electricity, ChargePerHours rate
        /// </summary>
        /// <returns>array with all electricity details</returns>
        public Double[] AskForElectricity();


        #endregion

        #region Parcels

        /// <summary>
        /// add a parcel to the database
        /// </summary>
        /// <param name="SenderId">customer's Id who send the parcel</param>
        /// <param name="TargetId">customer's Id who is about to receive the parcel</param>
        /// <param name="PackageWight">weight of the parcel</param>
        /// <param name="priority">priority to supply the parcel</param>
        /// <param name="created"> time the parcel created </param>
        /// <param name="scheduled">time the parcel schedueld to a drone</param>
        /// <param name="pickedUp">time the drone picked the parcel up</param>
        /// <param name="delivered">time the parcel delivered to the target customer</param>
        /// <param name="droneId">id of the drone who take care for this parcel</param>
        /// <returns>the Id of the current parcel</returns>
        public int AddParcel(int SenderId, int TargetId, DO.WeightCategories PackageWight, DO.Priorities priority, DateTime created, DateTime? scheduled, DateTime? pickedUp, DateTime? delivered, int droneId);
        public int AddParcel(int SenderId, int TargetId, DO.WeightCategories PackageWight, DO.Priorities priority, DateTime created);

        /// <summary>
        /// returns all parcels in data base as DAL parcels 
        /// </summary>
        /// <returns> all parcels in data base as DAL parcels</returns>
        public IEnumerable<Parcel> GetAllParcels();

        /// <summary>
        /// returns all parcels in data base as DAL parcels if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order for the customer to be returned</param>
        /// <returns> all parcels in data base as DAL parcels if they're up to the condition in the predicate</returns>
        public IEnumerable<Parcel> GetAllParcelsBy(Predicate<Parcel> predicate);

        /// <summary>
        /// return DAL parcel whom the id belongs
        /// </summary>
        /// <param name="Id">id of the specific parcel</param>
        /// <returns>parcel whom the id belongs</returns>
        public DO.Parcel GetParcel(int Id);

        /// <summary>
        /// set a parcel to the database - parcel's file
        /// </summary>
        /// <param name="newParcel">parcel to save in database</param>
        public void SetParcel(DO.Parcel newParcel);

        /// <summary>
        /// remove a parcel from database
        /// </summary>
        /// <param name="Id">Id of the parcel to remove</param>
        /// <returns>the removed parcel</returns>
        public Parcel RemoveParcel(int Id);


        #endregion

        #region Drone Charges

        /// <summary>
        ///  put drone into charge mpde
        /// </summary>
        /// <param name="DroneId">drone's Id</param>
        /// <param name="BaseStationId">Base station the drone get charged</param>
        /// <param name="started">time the drone started to being charged</param>
        public void AddDroneCharge(int DroneId, int BaseStationId, DateTime started);

        /// <summary>
        /// get a specific drone whom in charged 
        /// </summary>
        /// <param name="DroneId">drone's Id</param>
        /// <returns>specific drone whom in charged</returns>
        public DroneCharge GetDroneCharge(int DroneId);

        /// <summary>
        /// set drone in charged into the database in the correct file
        /// </summary>
        /// <param name="newDC">drone to charge</param>
        public void SetDroneCharge(DroneCharge newDC);

        /// <summary>
        /// get all DronesCharge from the database
        /// </summary>
        /// <returns>all DronesCharge</returns>
        public IEnumerable<DroneCharge> GetAllDroneCharges();

        /// <summary>
        /// returns all DroneCharge in data base as DAL DroneCharge if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order for the DroneCharge to be returned</param>
        /// <returns>all DroneCharge in data base as DAL DroneCharge if they're up to the condition in the predicate</returns>
        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate);

        /// <summary>
        /// remove a specific drone from DroneCharge file in database
        /// </summary>
        /// <param name="DroneId">drone's Id</param>
        /// <returns>removed DroneCharge</returns>
        public DroneCharge RemoveDroneCharge(int DroneId);


        #endregion

        #region Base Stations

        /// <summary>
        /// add a new base station to database
        /// </summary>
        /// <param name="Id">Base Station's Id</param>
        /// <param name="Name">Base Station's name</param>
        /// <param name="Longitude">Longitude Base Station's located</param>
        /// <param name="Latitude">Latitude Base Station's located</param>
        /// <param name="ChargeSlots">number of Slots in the Base Station</param>
        public void AddBaseStation(int Id, String Name, double Longitude, double Latitude, int ChargeSlots);

        /// <summary>
        /// get all Base Stations from database
        /// </summary>
        /// <returns>all Base Stations </returns>
        public IEnumerable<BaseStation> GetAllBaseStations();

        /// <summary>
        ///  returns all Base Station in data base as DAL Base Station if they're up to the condition in the predicate
        /// </summary>
        /// <param name="predicate">the condition in order for the Base Station to be returned</param>
        /// <returns>all Base Station in data base as DAL Base Station if they're up to the condition in the predicate</returns>
        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate);

        /// <summary>
        /// get a specific Base Station during the Id
        /// </summary>
        /// <param name="Id">Base Station's Id</param>
        /// <returns>the wanted Base station</returns>
        public DO.BaseStation GetBaseStation(int Id);

        /// <summary>
        /// set Base station with new details
        /// </summary>
        /// <param name="newBS">Base station who contains the new details</param>
        public void SetBaseStation(BaseStation newBS);

        /// <summary>
        /// remove base station from database
        /// </summary>
        /// <param name="Id">Baase Station's Id</param>
        /// <returns>removed Base station</returns>
        public BaseStation RemoveBaseStation(int Id);

        #endregion
    }
}
