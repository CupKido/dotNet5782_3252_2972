using System;
using System.Collections.Generic;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }
            public String Name { get; set; }
            public String Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public IDAL.DO.WeightCategories Weight { get; set; }
            public IDAL.DO.Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

        }
        public struct Drone
        {
            public int Id { get; set; }
            public String Model { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }
            public IDAL.DO.DroneStatuses Status { get; set; }
            public double Battery { get; set; }
        }
        public struct BaseStation
        {
            public int Id { get; set; }
            public String Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
        }
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int BaseStationId { get; set; }
        }

    }

   
    
}

namespace DalObject
{

    public class DataSource
    {
        public DataSource()
        {
            Initialize();
        }

        internal static IDAL.DO.Drone[] Drones = new IDAL.DO.Drone[10];
        internal static IDAL.DO.Customer[] Customers = new IDAL.DO.Customer[100];
        internal static IDAL.DO.Parcel[] Parcels = new IDAL.DO.Parcel[1000];
        internal static IDAL.DO.BaseStation[] BaseStations = new IDAL.DO.BaseStation[5];

        internal class Config
        {
            internal static int FirstDrone = 0;
            internal static int FirstCustomer = 0;
            internal static int FirstBaseStation = 0;
            internal static int FirstParcel = 0;
        }
        static Random r = new Random();
        static void Initialize()
        {


            for (int i = 0; i < 5; i++)
            {
                Drones[i].Id = i;
                Drones[i].Battery = r.Next(25, 99) + r.NextDouble();
            }
            Config.FirstDrone = 5;



            for (int i = 0; i < 2; i++)
            {
                BaseStations[i].Id = i;
                BaseStations[i].Latitude = r.Next() + r.NextDouble();
                BaseStations[i].Longitude = r.Next() + r.NextDouble();
            }
            BaseStations[0].Name = "Jerusalem";
            BaseStations[1].Name = "Haifa";
            Config.FirstBaseStation = 2;



            for (int i = 0; i < 10; i++)
            {
                Customers[i].Id = i;
                Customers[i].Latitude = r.Next() + r.NextDouble();
                Customers[i].Longitude = r.Next() + r.NextDouble();
            }
            Customers[0].Name = "Itzhak";
            Customers[1].Name = "Shlomo";
            Customers[2].Name = "Moshe";
            Customers[3].Name = "Yosef";
            Customers[4].Name = "John";
            Customers[5].Name = "Ahmed";
            Customers[6].Name = "Sayuri";
            Customers[7].Name = "Jason";
            Customers[8].Name = "Yaakov";
            Customers[9].Name = "Avi";
            Config.FirstCustomer = 10;

        }

        
    }
}
