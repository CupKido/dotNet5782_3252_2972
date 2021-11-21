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

                            while (!int.TryParse(Console.ReadLine(), out MenuChoice)) ;
                            switch (MenuChoice)
                            {
                                case 1:
                                    //Add Base Station
                                    AddBaseStation(myBL);
                                    break;

                                case 2: //Adding Drone
                                    AddDrone(myBL);
                                    break;

                                case 3: //Adding Customer

                                    AddCustomer(myBL);
                                    break;

                                case 4:
                                    AddParcel(myBL);
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

                            while (!int.TryParse(Console.ReadLine(), out MenuChoice)) ;

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

                            while (!int.TryParse(Console.ReadLine(), out MenuChoice)) ;
                            switch (MenuChoice)
                            {
                                case 1:
                                    {
                                        PrintBaseStation(myBL);
                                    }
                                    break;

                                case 2:
                                    {
                                        PrintDrone(myBL);

                                    }
                                    break;

                                case 3:
                                    {
                                       PrintCustomer(myBL);

                                    }
                                    break;

                                case 4:
                                    {
                                        PrintParcel(myBL);
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

                            while (!int.TryParse(Console.ReadLine(), out MenuChoice)) ;

                            switch (MenuChoice)
                            {
                                case 1:
                                    {
                                        PrintBaseStations(myBL);
                                    }
                                    break;
                                case 2:
                                    {
                                        PrintDrones(myBL);
                                    }
                                    break;

                                case 3:
                                    {
                                        printCustomers(myBL);
                                    }
                                    break;

                                case 4:
                                    {
                                        printParcels(myBL);
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
                                        PrintAvailibleBaseStations(myBL);
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

        private static void AddCustomer(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter new customer's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id) );

            Console.WriteLine("Enter Name: ");
            String Name = Console.ReadLine();

            String Phone;
            int Phoneint;
            do
            {
                Console.WriteLine("Enter Phone: ");
                Phone = Console.ReadLine();
            } while (!int.TryParse(Phone, out Phoneint) );

            double CLongitude;
            do
            {
                Console.WriteLine("Insert BaseStation Longitude:");
            } while (!double.TryParse(Console.ReadLine(), out CLongitude));

            double CLatitude;
            do
            {
                Console.WriteLine("Insert BaseStation Latitude:");
            } while (!double.TryParse(Console.ReadLine(), out CLatitude));

            try
            {
                myBL.AddCustomer(Id, Name, Phone, CLongitude, CLatitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void printParcels(IBL.IBL myBL)
        {
            foreach (ParcelToList p in myBL.GetAllParcels())
            {
                Console.WriteLine(p);
            }
            Console.WriteLine("\n");
        }

        private static void PrintParcel(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter parcel's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id));
            try
            {
                Console.WriteLine(myBL.GetParcel(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void PrintCustomer(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter customers's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id));
            try
            {
                Console.WriteLine(myBL.GetCustomer(Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void printCustomers(IBL.IBL myBL)
        {
            foreach(CustomerToList c in myBL.GetAllCustomers())
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\n");
        }

        private static void PrintDrone(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter drone's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id));
            try
            {
                Console.WriteLine(myBL.GetDrone(Id));
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void PrintDrones(IBL.IBL myBL)
        {
            foreach(DroneToList d in myBL.GetAllDrones())
            {
                Console.WriteLine(d);
            }
            Console.WriteLine("\n");
        }

        private static void AddDrone(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter new drone's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id));
            Console.WriteLine("Enter Model: ");
            String Model = Console.ReadLine();

            int MaxWeight;
            do
            {
                Console.WriteLine("Enter drone's max wieght capability:\n 1 - Light     2 - Intermediate     3 - Heavy");
            } while (!int.TryParse(Console.ReadLine(), out MaxWeight) );

            int StationId;
            do
            {
                Console.WriteLine("Enter starting base station's ID: ");
            } while (!int.TryParse(Console.ReadLine(), out StationId) );

            try
            {
            myBL.AddDrone(Id, Model, (WeightCategories)(MaxWeight - 1), StationId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private static void PrintAvailibleBaseStations(IBL.IBL myBL)
        {
            foreach(BaseStationToList bs in myBL.GetAllBaseStationsBy(p => p.DroneInChargesList.Count < p.ChargeSlots))
            {
                Console.WriteLine(bs);
            }
            Console.WriteLine("\n");
        }

        private static void PrintBaseStation(IBL.IBL myBL)
        {
            int Id;
            do
            {
                Console.WriteLine("Enter Base Station ID: ");
            } while (!int.TryParse(Console.ReadLine(), out Id));
            try
            {
                Console.WriteLine(myBL.GetBaseStation(Id));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void PrintBaseStations(IBL.IBL myBL)
        {
            foreach(BaseStationToList bs in myBL.GetAllBaseStations())
            {
                Console.WriteLine(bs);
            }
            Console.WriteLine("\n");
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
                Console.WriteLine("Insert BaseStation number:");
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
        private static void AddParcel(IBL.IBL myBL)
        {
            
            int BSSenderId;
            int BSTargetId;
            int BSPackageWight;
            int BSpriority;

            do
            {
                Console.WriteLine("Enter Sender ID: ");
            } while (!int.TryParse(Console.ReadLine(), out BSSenderId));

            do
            {
                Console.WriteLine("Enter Target ID: ");
            } while (!int.TryParse(Console.ReadLine(), out BSTargetId));

          
            do
            {
                Console.WriteLine("Enter package wieght:\n 1 - Light  2 - Intermediate  3 - Heavy");
            } while (!int.TryParse(Console.ReadLine(), out BSPackageWight));
            
           
            do
            {
                Console.WriteLine("Enter package wieght:\n 1 - Regular  2 - Fast  3 - Emergency");
            } while (!int.TryParse(Console.ReadLine(), out BSpriority));


            try
            {
                myBL.AddParcel( BSSenderId,  BSTargetId, (IDAL.DO.WeightCategories)BSPackageWight - 1, (IDAL.DO.Priorities) BSpriority-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
