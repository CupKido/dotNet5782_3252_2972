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

        public void AddDrone(int Id, String Model, DO.WeightCategories MaxWeight);

        public IEnumerable<Drone> GetAllDrones();

        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate);

        public DO.Drone GetDrone(int Id);

        public void SetDrone(DO.Drone newDrone);

        public DO.Drone RemoveDrone(int Id);

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

        public void AddBaseStation(int Id, String Name, double Longitude, double Latitude, int ChargeSlots);

        public IEnumerable<BaseStation> GetAllBaseStations();
        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate);
        public DO.BaseStation GetBaseStation(int Id);

        public void SetBaseStation(BaseStation newBS);

        public BaseStation RemoveBaseStation(int Id);
        #endregion
    }
}
