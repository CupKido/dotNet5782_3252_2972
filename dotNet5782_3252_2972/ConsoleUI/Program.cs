using DO;
using System;
using DAL;


namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime d;
           // DalApi.IDal DO = new DalObject.DalObject();
            DalApi.IDal DO = DalFactory.GetDal("List");
            bool flag = true;
            int MenuChoice;
            do
            {
                Console.WriteLine("Please Choose:");
                Console.WriteLine("1 to add an Object");
                Console.WriteLine("2 to update an Object");
                Console.WriteLine("3 to view an Object");
                Console.WriteLine("4 to view a list of Objects");
                Console.WriteLine("0 to Exit");
                while (!int.TryParse(Console.ReadLine(), out MenuChoice)) { }
                switch (MenuChoice)
                {

                    #region Object Addition

                    case 1:
                        {
                            Console.WriteLine("insert 1 to add a Base Station");
                            Console.WriteLine("insert 2 to add a Drone");
                            Console.WriteLine("insert 3 to add a Customer");
                            Console.WriteLine("insert 4 to add a Package");

                            MenuChoice = int.Parse(Console.ReadLine());
                            switch (MenuChoice)
                            {
                                case 1:
                                    //Add Base Station
                                    AddBaseStation(DO);
                                    break;

                                case 2: //Adding Drone
                                    AddDrone(DO);
                                    break;

                                case 3: //Adding Customer

                                    AddCustomer(DO);
                                    break;

                                case 4:
                                    AddParcel(DO);
                                    break;
                            }
                            break;
                        }
                    #endregion

                    #region Update Object

                    case 2:
                        {
                            Console.WriteLine("\ninsert 1 to decide a drone for a package");
                            Console.WriteLine("insert 2 to pick up a package by a drone");
                            Console.WriteLine("insert 3 to Deliver a package by a drone");
                            Console.WriteLine("insert 6 to update a Drone");
                            Console.WriteLine("insert 7 to update a Customer");
                            Console.WriteLine("insert 4 to charge a drone");
                            Console.WriteLine("insert 5 to decharge a drone");
                            MenuChoice = int.Parse(Console.ReadLine());
                            

                            switch (MenuChoice)
                            {
                                case 1:
                                    {
                                        DroneForPackage(DO);
                                    }
                                    break;

                                case 2:
                                    {
                                        PickPackage(DO);
                                    }
                                    break;

                                case 3:
                                    {
                                        DeliverPackage(DO);
                                        
                                    }
                                    break;

                                case 6:
                                    {
                                        PrintDrone(DO);
                                        

                                    }
                                    break;

                                case 7:
                                    {
                                        CustomerUpdate(DO);
                                    }
                                    break;

                                case 4:
                                    {
                                        ChargeDrone(DO);
                                    }
                                    break;

                                case 5:
                                    {
                                        DisChargeDrone(DO);
                                    }
                                    break;
                            }
                        }
                        break;

                    #endregion

                    #region Object View

                    case 3:
                        {

                            Console.WriteLine("\ninsert 1 to view a Base Station");
                            Console.WriteLine("insert 2 to view a Drone");
                            Console.WriteLine("insert 3 to view a Customer");
                            Console.WriteLine("insert 4 to view a Package");
                            MenuChoice = int.Parse(Console.ReadLine());
                            
                            switch (MenuChoice)
                            {
                                case 1:
                                    {
                                        PrintBaseStation(DO);
                                    }
                                    break;

                                case 2:
                                    {
                                        PrintDrone(DO);
                                        
                                    }
                                    break;

                                case 3:
                                    {
                                        PrintCustomer(DO);
                                        
                                    }
                                    break;

                                case 4:
                                    {
                                        PrintParcel(DO);
                                    }
                                    break;

                            }
                            Console.WriteLine("\n");
                        }
                        break;

                    #endregion

                    #region List View

                    case 4:
                        {
                            Console.WriteLine("\ninsert 1 to view Base Stations");
                            Console.WriteLine("insert 2 to view Drones");
                            Console.WriteLine("insert 3 to view Customers");
                            Console.WriteLine("insert 4 to view Packages");
                            Console.WriteLine("insert 6 to view available charging ports");

                            MenuChoice = int.Parse(Console.ReadLine());
                           
                            switch (MenuChoice)
                            {
                                case 1:
                                    {
                                        PrintAvailibleStations(DO);
                                    }
                                    break;
                                case 2:
                                    {
                                        foreach (DO.Drone drone in DO.GetAllDrones())
                                        {
                                            Console.WriteLine(drone + "\n");
                                        }
                                    }
                                    break;

                                case 3:
                                    {

                                        foreach (DO.Customer customer in DO.GetAllCustomers())
                                        {
                                            Console.WriteLine(customer + "\n");
                                        }
                                    }
                                    break;

                                case 4:
                                    {
                                        foreach (DO.Parcel parcel in DO.GetAllParcels())
                                        {
                                            Console.WriteLine(parcel + "\n");
                                        }
                                    }
                                    break;

                                case 5:
                                    {
                                        foreach (DO.Parcel parcel in DO.GetAllParcels())
                                        {
                                        
                                            if(parcel.DroneId == 0)
                                            {
                                            Console.WriteLine(parcel + "\n");
                                            }
                                        }


                                    }
                                    break;

                                case 6:
                                    {
                                        int ACS = 0; //Availible Charge Slots
                                        foreach (DO.BaseStation baseStation in DO.GetAllBaseStations())
                                        {
                                            ACS = baseStation.ChargeSlots;
                                            foreach (DO.DroneCharge droneCharge in DO.GetAllDroneCharges())
                                            {
                                                if (droneCharge.BaseStationId == baseStation.Id)
                                                    ACS -= 1;
                                            }

                                            if (ACS != 0)
                                            {
                                                Console.WriteLine(baseStation);
                                                Console.WriteLine("Availible Charge Slots: {0}", ACS);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    #endregion

                    #region Exit Menu
                    case 0:
                        flag = false;
                        break;
                    #endregion

                    default:

                        break;


                }

            } while (flag);

        }

        private static void PrintParcel(DalApi.IDal DO)
        {
            Console.WriteLine("Enter ID:");
            int Id;
            while (!int.TryParse(Console.ReadLine(), out Id))
            {
                Console.WriteLine("Error: wrong input type! insert digits only");
            }
            try
            {
                Console.WriteLine(DO.GetParcel(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void PrintCustomer(DalApi.IDal DO)
        {
            Console.WriteLine("Enter ID:");
            int Id;
            while (!int.TryParse(Console.ReadLine(), out Id))
            {
                Console.WriteLine("Error: wrong input type! insert digits only");
            }
            try
            {
                Console.WriteLine(DO.GetCustomer(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void PrintBaseStation(DalApi.IDal DO)
        {
            Console.WriteLine("Enter ID:");
            int Id;
            while (!int.TryParse(Console.ReadLine(), out Id))
            {
                Console.WriteLine("Error: wrong input type! insert digits only");
            }
            try
            {
                Console.WriteLine(DO.GetBaseStation(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DisChargeDrone(DalApi.IDal DO)
        {
            Console.WriteLine("\nPlease Enter the Drone's ID: ");
            DO.Drone drone;
            try
            {
                drone = DO.GetDrone(int.Parse(Console.ReadLine()));
            }
            catch
            {
                Console.WriteLine("Drone was not found!\n");
                return;
            }
            DO.RemoveDroneCharge(drone.Id);
            //drone.Status = IDAL.DO.DroneStatuses.Availible;
            DO.SetDrone(drone);
            Console.WriteLine(drone + "\n");
        }

        private static void ChargeDrone(DalApi.IDal DO)
        {
            Console.WriteLine("\nPlease Enter the Drone's ID: ");
            DO.Drone drone;
            try
            {
                drone = DO.GetDrone(int.Parse(Console.ReadLine()));
            }
            catch
            {
                Console.WriteLine("Drone was not found!");
                return;
            }
            Console.WriteLine(drone + "\n");
            Console.WriteLine("Please select a Base Station: ");
            PrintAvailibleStations(DO);

            Console.WriteLine("Please Enter the selected Base Station's ID: ");
            DO.BaseStation baseStation;
            try
            {
                baseStation = DO.GetBaseStation(int.Parse(Console.ReadLine()));
            }
            catch
            {
                Console.WriteLine("Base Station was not found!");
                return;
            }

            Console.WriteLine(baseStation);

            DO.AddDroneCharge(drone.Id, baseStation.Id);
            //drone.Status = IDAL.DO.DroneStatuses.Maintenance;
            //drone.Battery = 100;
            DO.SetDrone(drone);
            Console.WriteLine("Drone {0} is Being Charged at Base Station {1}!\n", drone.Id, baseStation.Id);
        }

        private static void CustomerUpdate(DalApi.IDal DO)
        {
            Console.WriteLine("\nEnter ID:");
            int Id = int.Parse(Console.ReadLine());

            DO.Customer customer;
            try
            {
                customer = DO.GetCustomer(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine(customer);

            Console.WriteLine("Please Enter Phone Number: ");
            String Phone = Console.ReadLine();
            if (Phone.Length != 10)
            {
                Console.WriteLine("illigel number!\n");
                return;
            }
            customer.Phone = Phone;

            Console.WriteLine("Please Enter Longitude: ");
            customer.Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Please Enter Latitude: ");
            customer.Latitude = double.Parse(Console.ReadLine());

            DO.SetCustomer(customer);

            Console.WriteLine("\nCustomer {0} {1} is now updated!\n", customer.Name, customer.Id);
        }

        private static void PrintDrone(DalApi.IDal DO)
        {
            Console.WriteLine("Enter ID:");
            int Id;
            while (!int.TryParse(Console.ReadLine(), out Id))
            {
                Console.WriteLine("Error: wrong input type! insert digits only");
            }
            try
            {
                Console.WriteLine(DO.GetDrone(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DeliverPackage(DalApi.IDal DO)
        {
            Console.WriteLine("\nEnter Package ID:");
            int Id = int.Parse(Console.ReadLine());

            DO.Parcel parcel;
            try
            {
                parcel = DO.GetParcel(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (parcel.DroneId == 0)
            {
                //ERROR
                return;
            }

            Console.WriteLine(parcel);

            DO.Drone drone;
            try
            {
                drone = DO.GetDrone(parcel.DroneId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //if (drone.Status != IDAL.DO.DroneStatuses.InDelivery)
            //{
            //    //ERROR
            //    return;
            //}
            //if (drone.Battery < 15)
            //{
            //    //ERROR
            //    return;
            //}

            //drone.Status = IDAL.DO.DroneStatuses.Availible;
            //drone.Battery -= 5;
            parcel.Delivered = DateTime.Now;

            DO.SetDrone(drone);
            DO.SetParcel(parcel);

            Console.WriteLine("Drone {0} has Delivered the Package {1}", drone.Id, parcel.Id);
        }

        private static void PickPackage(DalApi.IDal DO)
        {
            Console.WriteLine("\nEnter Package ID:");
            int Id = int.Parse(Console.ReadLine());

            DO.Parcel parcel;
            try
            {
                parcel = DO.GetParcel(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine(parcel);

            if (parcel.DroneId == 0)
            {
                //ERROR
                return;
            }

            DO.Drone drone;
            try
            {
                drone = DO.GetDrone(parcel.DroneId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //if (drone.Status != IDAL.DO.DroneStatuses.Availible)
            //{
            //    //ERROR
            //    return;
            //}
            //if (drone.Battery <= 20)
            //{
            //    //ERROR
            //    return;
            //}

            //drone.Status = IDAL.DO.DroneStatuses.InDelivery;
            //drone.Battery -= 5;
            parcel.PickedUp = DateTime.Now;

            DO.SetDrone(drone);
            DO.SetParcel(parcel);

            Console.WriteLine("Drone {0} has Picked Up the Package {1}", drone.Id, parcel.Id);
        }

        private static void DroneForPackage(DalApi.IDal DO)
        {
            {
                Console.WriteLine("\nEnter Package ID:");
                int Id;
                while (!int.TryParse(Console.ReadLine(), out Id))
                {
                    Console.WriteLine("Error: please insert a number");
                }
                DO.Parcel parcel;
                try
                {
                    parcel = DO.GetParcel(Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(parcel);

                Console.WriteLine("Enter Drone's ID:");
                Id = int.Parse(Console.ReadLine());
                DO.Drone drone;
                try
                {
                    drone = DO.GetDrone(Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                if (drone.MaxWeight < parcel.Weight)
                {
                    Console.WriteLine("The Drone Can't Carry the Package!\n");
                    return;
                }
                Console.WriteLine(drone);

                parcel.DroneId = Id;
                parcel.scheduled = DateTime.Now;
                DO.SetParcel(parcel);

                Console.WriteLine("Update Complete!\n");
            }
        }

        private static void AddParcel(DalApi.IDal DO)
        {
            Console.WriteLine("Please Enter New ID: ");
            int Id = int.Parse(Console.ReadLine());

            try
            {
                DO.GetBaseStation(Id);
                Console.WriteLine("ID Already exists!");
                return;
            }
            catch (Exception ex)
            { }

            Console.WriteLine("Please Enter SenderID: ");
            int SenderID = int.Parse(Console.ReadLine());


            Console.WriteLine("Please Enter TargetID: ");
            int TargetID = int.Parse(Console.ReadLine());

            Console.WriteLine("Please Enter a number for PackageWight:   Light = 0, Intermediate = 1, Heavy = 2 ");
            int num = int.Parse(Console.ReadLine());
            DO.WeightCategories PackageWight = (WeightCategories) num;

            if (num < 0 || num > 2)
            {
                Console.WriteLine("illigel PackageWight!");
                return;
            }


            Console.WriteLine("Please Enter a number for priority:   Regular = 0, Fast = 1, Emergency = 2 ");
            num = int.Parse(Console.ReadLine());
            DO.Priorities priority = (Priorities)num;

            if (num < 0 || num>2)
            {
                Console.WriteLine("illigel priority!");
                return;
            }

            DateTime d = DateTime.Now;
            try
            {
                DO.AddParcel(Id, SenderID, TargetID, PackageWight, priority,d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("Package Added!");
        }

        private static void AddBaseStation(DalApi.IDal DO) 
        {
            Console.WriteLine("Please Enter New ID: ");
            int Id = int.Parse(Console.ReadLine());

            try
            {
                DO.GetBaseStation(Id);
                Console.WriteLine("ID Already exists!");
                return;
            }
            catch (Exception ex)
            { }

            Console.WriteLine("Please Enter Name: ");
            String Name = Console.ReadLine();

           
            Console.WriteLine("Please Enter Longitude: ");
            double Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Please Enter Latitude: ");
            double Latitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Please Enter ChargeSlots: ");
            int ChargeSlots = int.Parse(Console.ReadLine());
            if (ChargeSlots<0)
            {
                Console.WriteLine("illigel number of chargeslots!");
                return;
            }


            try
            {
                DO.AddBaseStations(Id,  Name,  Longitude,  Latitude,  ChargeSlots);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("BaseStation Added!");
        }

        private static void AddCustomer(DalApi.IDal DO)
        {
            Console.WriteLine("Please Enter New ID: ");
            int Id = int.Parse(Console.ReadLine());

            try
            {
                DO.GetCustomer(Id);
                Console.WriteLine("ID Already Taken!");
                return;
            }
            catch (Exception ex)
            { }
            

            Console.WriteLine("Please Enter Name: ");
            String Name = Console.ReadLine();

            Console.WriteLine("Please Enter Phone Number: ");
            String Phone = Console.ReadLine();
            if (Phone.Length != 10)
            {
                Console.WriteLine("illigel number!");
                return;
            }

            Console.WriteLine("Please Enter Longitude: ");
            double Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Please Enter Latitude: ");
            double Latitude = double.Parse(Console.ReadLine());

            try
            {
                DO.AddCustomer(Id, Name, Phone, Longitude, Latitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("Customer Added!");
        }

        private static void AddDrone(DalApi.IDal DO)
        {
            Console.WriteLine("Please Enter New ID: ");
            int Id = int.Parse(Console.ReadLine());

            try
            {
                DO.GetDrone(Id);
                Console.WriteLine("ID Already exists!");
                return;
            }
            catch (Exception ex)
            { }

            Console.WriteLine("Please Enter Model: Mavic, SkyDrone or Parrot ");
            String Model = Console.ReadLine();


            Console.WriteLine("Please Enter number for MaxWeight: Light = 0, Intermediate = 1,  Heavy = 2 ");
            int max = int.Parse(Console.ReadLine());
            DO.WeightCategories MaxWeight = (WeightCategories)max;

            if(max<0 || max>2)
            {
                Console.WriteLine(" illegal number ");
                return;
            }


            try
            {
                DO.AddDrone(Id, Model, MaxWeight);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Drone Added!");
        }

        static void PrintAvailibleStations(DalApi.IDal DO)
        {
            int ACS = 0; //Availible Charge Slots
            foreach (DO.BaseStation baseStation in DO.GetAllBaseStations())
            {
                ACS = baseStation.ChargeSlots;
                foreach (DO.DroneCharge droneCharge in DO.GetAllDroneCharges())
                {
                    if (droneCharge.BaseStationId == baseStation.Id)
                        ACS -= 1;
                }

                if (ACS != 0)
                {
                    Console.WriteLine(baseStation);
                    Console.WriteLine("Availible Charge Slots: {0}", ACS);
                }
            }
        }

        static void PrintBaseStations(DalApi.IDal DO)
        {
            foreach (DO.BaseStation baseStation in DO.GetAllBaseStations())
            {
                Console.WriteLine(baseStation + "\n");
            }
        }
    }
}
