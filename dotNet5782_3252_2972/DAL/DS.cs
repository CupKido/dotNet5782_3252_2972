using System;
using System.Collections.Generic;
using System.Linq;

namespace DalObject
{
  
    public class DataSource
    {
        internal static List<DO.Drone> Drones = new List<DO.Drone>();
        internal static List<DO.Customer> Customers = new List<DO.Customer>();
        internal static List<DO.Parcel> Parcels = new List<DO.Parcel>();
        internal static List<DO.BaseStation> BaseStations = new List<DO.BaseStation>();
        internal static List<DO.DroneCharge> DroneCharges = new List<DO.DroneCharge>();

        internal class Config
        {
            internal static Double AvailbleElec = 20;
            internal static Double LightElec = 35;
            internal static Double IntermediateElec = 50;
            internal static Double HeavyElec = 80;
            internal static Double ChargePerHours = 40;
            
        }

        private static Random r = new Random();
        internal static void Initialize()
        {

            //5 Drones initializer
            for (int i = 0; i < 10; i++)
            {
                DO.Drone drone = new DO.Drone();
                drone.Id = i + 1;
                //drone.Battery = r.Next(25, 100) + r.NextDouble();


                drone.MaxWeight = (DO.WeightCategories)r.Next(0, 3); //IDAL.DO.WeightCategories.Heavy;


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
                DO.BaseStation BS = new DO.BaseStation();
                BS.Id = i + 1;
                BS.Latitude = r.Next(0, 10) + r.NextDouble();
                BS.Longitude = r.Next(0, 10) + r.NextDouble();
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
            string[] Names = { "Itzhak", "Shlomo", "Moshe", "Yosef", "John", "Ahmed", "Sayuri", "Jason", "Yaakov", "Avi" };

            for (int i = 0; i < 10; i++)
            {
                DO.Customer customer = new DO.Customer();
                customer.Id = i + 1;
                customer.Latitude = r.Next(5, 10) + r.NextDouble();
                customer.Longitude = r.Next(5, 10) + r.NextDouble();
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
                customer.Name = Names[i];
                Customers.Add(customer);
            }
            //customers[0].Name = "Itzhak";
            //Customers[1].Name = "Shlomo";
            //Customers[2].Name = "Moshe";
            //Customers[3].Name = "Yosef";
            //Customers[4].Name = "John";
            //Customers[5].Name = "Ahmed";
            //Customers[6].Name = "Sayuri";
            //Customers[7].Name = "Jason";
            //Customers[8].Name = "Yaakov";
            //Customers[9].Name = "Avi";
            ///string[] Names= { "Itzhak", "Shlomo", "Moshe", "Yosef", "John", "Ahmed", "Sayuri", "Jason", "Yaakov", "Avi" };
            
            

            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            List<int> DronesId = (from DO.Drone DId in Drones
                                  select DId.Id).ToList();

            for (int i = 0; i < 10; i++)
            {
                DO.Parcel parcel = new DO.Parcel();
                parcel.Id = Parcels.Count + 1;
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
                parcel.Priority = (DO.Priorities)r.Next(0, 3);
                parcel.Weight = (DO.WeightCategories)r.Next(0, 3);
                parcel.Requested = start.AddDays(r.Next(range));

                
                switch (r.Next(4))
                {
                    case 0:
                        parcel.DroneId = 0;
                        break;

                    case 1:
                    case 2:
                    case 3:
                        DO.Parcel? takenDroneP;
                        int times = 0;
                        do
                        {
                            times++;
                            parcel.DroneId = DronesId[r.Next(DronesId.Count)];
                            DronesId.Remove(parcel.DroneId);
                            parcel.Scheduled = DateTime.Now;
                            if(r.Next(2) == 1)
                            {
                                parcel.PickedUp = DateTime.Now;
                                if(r.Next(2) == 1)
                                {
                                    parcel.Delivered = DateTime.Now;
                                }
                            }
                            takenDroneP = Parcels.FirstOrDefault(p => p.DroneId == parcel.DroneId && p.Delivered == null);
                        } while (takenDroneP.Value.Id != 0 && parcel.Weight > Drones.FirstOrDefault(d => d.Id == parcel.DroneId).MaxWeight && times <= 3);
                        if(times == 4)
                        {
                            parcel.DroneId = 0;
                            parcel.Scheduled = null;
                            parcel.PickedUp = null;
                            parcel.Delivered = null;
                        }
                        break;
                }
                

                Parcels.Add(parcel);
            }



        }
    }
}