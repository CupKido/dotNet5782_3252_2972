using System;
using System.Collections.Generic;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            int Id { get; set; }
            String Name { get; set; }
            String Phone { get; set; }
            double Longitude { get; set; }
            double Latitude { get; set; }
        }
        public struct Parcel
        {
            int Id { get; set; }
            int SenderId { get; set; }
            int TargetId { get; set; }
            IDAL.DO.WeightCategories Weight { get; set; }
            IDAL.DO.Priorities Priority { get; set; }
            DateTime Requested { get; set; }
            int DroneId { get; set; }
            DateTime scheduled { get; set; }
            DateTime PickedUp { get; set; }
            DateTime Delivered { get; set; }

        }
        public struct Drone
        {
            int Id { get; set; }
            String Model { get; set; }
            IDAL.DO.WeightCategories MaxWeight { get; set; }
            IDAL.DO.DroneStatuses Status { get; set; }
            double Battery { get; set; }
        }
        public struct BaseStation
        {
            int Id { get; set; }
            String Name { get; set; }
            double Longitude { get; set; }
            double Latitude { get; set; }
            int ChargeSlots { get; set; }
        }
        public struct DroneCharge
        {
            int DroneId { get; set; }
            int BaseStationId { get; set; }
        }

    }

    namespace DalObject
    {
        internal class DataSource
        {
            public static List<DO.Drone> Drones;
            public static List<DO.Customer> Customers;
            public static List<DO.Parcel> Parcels;
            public static List<DO.BaseStation> BaseStations;
            public static List<DO.DroneCharge> DroneCharges;

        }
    }
    
}
