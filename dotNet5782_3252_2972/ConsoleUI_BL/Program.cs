using System;
using BLobject;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.IBL myBL = new BLobject.BL();
            bool Exit = false;
            int MenuChoice;

            while (!Exit)
            {

                Console.WriteLine("Please Choose:");
                Console.WriteLine("1 to add an Object");
                Console.WriteLine("2 to update an Object");
                Console.WriteLine("3 to view an Object");
                Console.WriteLine("4 to view a list of Objects");
                Console.WriteLine("0 to Exit");
                while(!int.TryParse(Console.ReadLine(), out MenuChoice)) { }
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
                                    AddBaseStation(myBL);
                                    break;

                                case 2: //Adding Drone
                                    //AddDrone(DO);
                                    break;

                                case 3: //Adding Customer

                                    //AddCustomer(DO);
                                    break;

                                case 4:
                                    //AddParcel(DO);
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
                                        //DroneForPackage(DO);
                                    }
                                    break;

                                case 2:
                                    {
                                        //PickPackage(DO);
                                    }
                                    break;

                                case 3:
                                    {
                                        //DeliverPackage(DO);

                                    }
                                    break;

                                case 6:
                                    {
                                        //PrintDrone(DO);


                                    }
                                    break;

                                case 7:
                                    {
                                        //CustomerUpdate(DO);
                                    }
                                    break;

                                case 4:
                                    {
                                        //ChargeDrone(DO);
                                    }
                                    break;

                                case 5:
                                    {
                                        //DisChargeDrone(DO);
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
                                        //PrintBaseStation(DO);
                                    }
                                    break;

                                case 2:
                                    {
                                        //PrintDrone(DO);

                                    }
                                    break;

                                case 3:
                                    {
                                       // PrintCustomer(DO);

                                    }
                                    break;

                                case 4:
                                    {
                                        //PrintParcel(DO);
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
                                        PrintAvailibleStations(myBL);
                                    }
                                    break;
                                case 2:
                                    {
                                        //foreach (IDAL.DO.Drone drone in DO.GetAllDrones())
                                        //{
                                        //    Console.WriteLine(drone + "\n");
                                        //}
                                    }
                                    break;

                                case 3:
                                    {

                                        //foreach (IDAL.DO.Customer customer in DO.GetAllCustomers())
                                        //{
                                        //    Console.WriteLine(customer + "\n");
                                        //}
                                    }
                                    break;

                                case 4:
                                    {
                                        //foreach (IDAL.DO.Parcel parcel in DO.GetAllParcels())
                                        //{
                                        //    Console.WriteLine(parcel + "\n");
                                        //}
                                    }
                                    break;

                                case 5:
                                    {
                                        //foreach (IDAL.DO.Parcel parcel in DO.GetAllParcels())
                                        //{

                                        //    if (parcel.DroneId == 0)
                                        //    {
                                        //        Console.WriteLine(parcel + "\n");
                                        //    }
                                        //}


                                    }
                                    break;

                                case 6:
                                    {
                                        //int ACS = 0; //Availible Charge Slots
                                        //foreach (IDAL.DO.BaseStation baseStation in DO.GetAllBaseStations())
                                        //{
                                        //    ACS = baseStation.ChargeSlots;
                                        //    foreach (IDAL.DO.DroneCharge droneCharge in DO.GetAllDroneCharges())
                                        //    {
                                        //        if (droneCharge.BaseStationId == baseStation.Id)
                                        //            ACS -= 1;
                                        //    }

                                        //    if (ACS != 0)
                                        //    {
                                        //        Console.WriteLine(baseStation);
                                        //        Console.WriteLine("Availible Charge Slots: {0}", ACS);
                                        //    }
                                        //}
                                    }
                                    break;
                            }
                        }
                        break;

                    #endregion

                    #region Exit Menu
                    case 0:
                        Exit = true;
                        break;
                    #endregion

                    default:

                        break;


                }



            }
        }

        private static void PrintAvailibleStations(IBL.IBL myBL)
        {
            foreach(BaseStationToList bs in myBL.GetAllBaseStations())
            {
                Console.WriteLine(bs);
            }
        }

        private static void AddBaseStation(IBL.IBL myBL)
        {
            int BSId;
            String BSName;
            double BSLongitude;
            double BSLatitude;
            int BSChargeSlots;

            do
            {
                Console.WriteLine("Insert BaseStation ID:");
            } while (!int.TryParse(Console.ReadLine(), out BSId));

            Console.WriteLine("Enter Station Name: ");
            BSName = Console.ReadLine();
            do
            {
                Console.WriteLine("Insert BaseStation Longitude:");
            } while (!double.TryParse(Console.ReadLine(), out BSLongitude));
            do
            {
                Console.WriteLine("Insert BaseStation Latitude:");
            } while (!double.TryParse(Console.ReadLine(), out BSLatitude));
            do
            {
                Console.WriteLine("Insert BaseStation Charge slots amount:");
            } while (!int.TryParse(Console.ReadLine(), out BSChargeSlots));

            try
            {
                myBL.AddBaseStations(BSId, BSName, new Location() { Longitude = BSLongitude, Latitude = BSLatitude }, BSChargeSlots);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
