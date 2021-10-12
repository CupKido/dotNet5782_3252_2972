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

            public override string ToString()
            {

                #region Longitude & Latitude Calculations

                char lon = 'N';
                if (Longitude < 0)
                {
                    lon = 'S';
                    Longitude *= -1;
                }
                double lonDegreesWithFraction = Longitude;
                int londegrees = (int)lonDegreesWithFraction; // = 48

                double lonfractionalDegrees = lonDegreesWithFraction - londegrees; // = .858222
                double lonminutesWithFraction = 60 * lonfractionalDegrees; // = 51.49332
                int lonminutes = (int)lonminutesWithFraction; // = 51

                double lonfractionalMinutes = lonminutesWithFraction - lonminutes; // = .49332
                double lonsecondsWithFraction = 60 * lonfractionalMinutes; // = 29.6

                char lat = 'E';
                if (Latitude < 0)
                {
                    lat = 'W';
                    Latitude *= -1;
                }

                double latDegreesWithFraction = Latitude;
                int latdegrees = (int)latDegreesWithFraction; // = 48

                double latfractionalDegrees = latDegreesWithFraction - latdegrees; // = .858222
                double latminutesWithFraction = 60 * latfractionalDegrees; // = 51.49332
                int latminutes = (int)latminutesWithFraction; // = 51

                double latfractionalMinutes = latminutesWithFraction - latminutes; // = .49332
                double latsecondsWithFraction = 60 * latfractionalMinutes; // = 29.6

                #endregion

                return "ID: " + Id + "\nName: " + Name + "\nPhone: " + Phone +
                    "\nLongitude: " + londegrees + "°" + lonminutes + "'" + Math.Round(lonsecondsWithFraction, 3) + "\"" + lon +
                    "\nLatitude: " + latdegrees + "°" + latminutes + "'" + Math.Round(latsecondsWithFraction, 3) + "\"" + lat;
                    
            }
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

            public override string ToString()
            {
                return base.ToString();
            }
        }
        public struct Drone
        {
            public int Id { get; set; }
            public String Model { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }
            public IDAL.DO.DroneStatuses Status { get; set; }
            public double Battery { get; set; }

            public override string ToString()
            {
                return "ID: " + Id + "\nModel: " + Model + "\nMax Weight: " + MaxWeight + "\nStatus: " + Status + "\nBattery: " + Battery;
            }
        }
        public struct BaseStation
        {
            public int Id { get; set; }
            public String Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return base.ToString();
            }
        }
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int BaseStationId { get; set; }

            public override string ToString()
            {
                return base.ToString();
            }
        }

    }

   
    
}

namespace DalObject
{

    public class DataSource
    {

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
        private static Random r = new Random();
        internal static void Initialize()
        {

            //5 Drones initializer
            for (int i = 0; i < 5; i++)
            {
                Drones[i].Id = i + 1;
                Drones[i].Battery = r.Next(25, 100) + r.NextDouble();

                switch (r.Next(1, 4))
                {
                    case 1:
                        Drones[i].MaxWeight = IDAL.DO.WeightCategories.Heavy;
                        break;
                    case 2:
                        Drones[i].MaxWeight = IDAL.DO.WeightCategories.Intermediate;
                        break;
                    case 3:
                        Drones[i].MaxWeight = IDAL.DO.WeightCategories.Light;
                        break;
                }

                switch (r.Next(1, 4))
                {
                    case 1:
                        Drones[i].Model = "Mavic";
                        break;
                    case 2:
                        Drones[i].Model = "SkyDrone";
                        break;
                    case 3:
                        Drones[i].Model = "Parrot";
                        break;
                }

            }

            Config.FirstDrone = 5;


            //2 Base Stations initializer
            for (int i = 0; i < 2; i++)
            {
                BaseStations[i].Id = i + 1;
                BaseStations[i].Latitude = r.Next(-100, 100) + r.NextDouble();
                BaseStations[i].Longitude = r.Next(-100, 100) + r.NextDouble();
                BaseStations[i].ChargeSlots = r.Next(5, 11);
            }
            BaseStations[0].Name = "Jerusalem";
            BaseStations[1].Name = "Haifa";
            Config.FirstBaseStation = 2;


            //10 Customers
            for (int i = 0; i < 10; i++)
            {
                Customers[i].Id = i + 1;
                Customers[i].Latitude = r.Next(-100, 100) + r.NextDouble();
                Customers[i].Longitude = r.Next(-100, 100) + r.NextDouble();
                switch (r.Next(1, 5))
                {
                    case 1:
                        Customers[i].Phone = "052";
                        break;
                    case 2:
                        Customers[i].Phone = "054";
                        break;
                    case 3:
                        Customers[i].Phone = "058";
                        break;
                    case 4:
                        Customers[i].Phone = "055";
                        break;
                }
                for(int j = 0; j < 7; j++)
                {
                    Customers[i].Phone += r.Next(0, 9);
                }
                
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
