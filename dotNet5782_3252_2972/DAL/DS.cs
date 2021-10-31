using System;
using System.Collections.Generic;


namespace DalObject
{

    public class DataSource
    {
        internal static List<IDAL.DO.Drone> Drones = new List<IDAL.DO.Drone>();
        internal static List<IDAL.DO.Customer> Customers = new List<IDAL.DO.Customer>();
        internal static List<IDAL.DO.Parcel> Parcels = new List<IDAL.DO.Parcel>();
        internal static List<IDAL.DO.BaseStation> BaseStations = new List<IDAL.DO.BaseStation>();
        internal static List<IDAL.DO.DroneCharge> DroneCharges = new List<IDAL.DO.DroneCharge>();

        internal class Config
        {
            internal static Double AvailbleElec = 1;
            internal static Double LightElec = 15;
            internal static Double IntermediateElec = 25;
            internal static Double HeavyElec = 40;
            internal static Double ChargePerHours = 40;
            
        }

        private static Random r = new Random();
        internal static void Initialize()
        {

            //5 Drones initializer
            for (int i = 0; i < 5; i++)
            {
                IDAL.DO.Drone drone = new IDAL.DO.Drone();
                drone.Id = i + 1;
                //drone.Battery = r.Next(25, 100) + r.NextDouble();


                drone.MaxWeight = (IDAL.DO.WeightCategories)r.Next(0, 3); //IDAL.DO.WeightCategories.Heavy;


                switch (r.Next(1, 4))
                {
                    case 1:
                        drone.Model = "Mavic";
                        break;
                    case 2:
                        drone.Model = "SkyDrone";
                        break;
                    case 3:
                        drone.Model = "Parrot";
                        break;
                }
                Drones.Add(drone);
            }


            //2 Base Stations initializer
            for (int i = 0; i < 2; i++)
            {
                IDAL.DO.BaseStation BS = new IDAL.DO.BaseStation();
                BS.Id = i + 1;
                BS.Latitude = r.Next(-100, 100) + r.NextDouble();
                BS.Longitude = r.Next(-100, 100) + r.NextDouble();
                BS.ChargeSlots = r.Next(3, 6);
                if (i == 0)
                {
                    BS.Name = "Jerusalem";
                }
                else
                {
                    BS.Name = "Haifa";
                }
                BaseStations.Add(BS);
            }
            
            //10 Customers
            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Customer customer = new IDAL.DO.Customer();
                customer.Id = i + 1;
                customer.Latitude = r.Next(-100, 100) + r.NextDouble();
                customer.Longitude = r.Next(-100, 100) + r.NextDouble();
                switch (r.Next(1, 5))
                {
                    case 1:
                        customer.Phone = "052";
                        break;
                    case 2:
                        customer.Phone = "054";
                        break;
                    case 3:
                        customer.Phone = "058";
                        break;
                    case 4:
                        customer.Phone = "055";
                        break;
                }
                for (int j = 0; j < 7; j++)
                {
                    customer.Phone += r.Next(0,10);
                }
                Customers.Add(customer);
            }
            //Customers[0].Name = "Itzhak";
            //Customers[1].Name = "Shlomo";
            //Customers[2].Name = "Moshe";
            //Customers[3].Name = "Yosef";
            //Customers[4].Name = "John";
            //Customers[5].Name = "Ahmed";
            //Customers[6].Name = "Sayuri";
            //Customers[7].Name = "Jason";
            //Customers[8].Name = "Yaakov";
            //Customers[9].Name = "Avi";
            

            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
                parcel.Id = i + 1;
                bool flag = true;
                while (flag)
                {
                    parcel.SenderId = r.Next(1, 11);
                    parcel.TargetId = r.Next(1, 11);
                    if (parcel.SenderId != parcel.TargetId)
                    {
                        flag = false;
                    }
                }
                parcel.Priority = (IDAL.DO.Priorities)r.Next(0, 3);
                parcel.Weight = (IDAL.DO.WeightCategories)r.Next(0, 3);
                parcel.DroneId = 0;
                parcel.Requested = start.AddDays(r.Next(range));

            }



        }
    }
}