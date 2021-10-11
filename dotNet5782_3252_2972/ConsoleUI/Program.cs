using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DalObject DO = new DalObject.DalObject();
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
                MenuChoice = int.Parse(Console.ReadLine());
                switch (MenuChoice)
                {

                    #region Object Addition

                    case 1:
                        Console.WriteLine("insert 2 to add a Drone");
                        Console.WriteLine("insert 3 to add a Customer");
                        MenuChoice = int.Parse(Console.ReadLine());
                        switch (MenuChoice)
                        {
                            case 1:
                                //Add Base Station
                                break;

                            case 2: //Adding Drone
                                try
                                {
                                    DO.AddDrone(11, "blab", IDAL.DO.WeightCategories.Heavy);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;

                            case 3: //Adding Customer
                                Console.WriteLine("Please Enter New ID: ");
                                int Id = int.Parse(Console.ReadLine());
                                try
                                {
                                    DO.GetCustomer(Id);
                                    Console.WriteLine("ID Already Taken!");
                                    break;
                                }
                                catch (Exception ex)
                                {

                                }
                                Console.WriteLine("Please Enter Name: ");
                                String Name = Console.ReadLine();

                                Console.WriteLine("Please Enter Phone Number: ");
                                String Phone = Console.ReadLine();
                                if (Phone.Length != 10)
                                {
                                    Console.WriteLine("illigel number!");
                                    break;
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
                                    break;
                                }

                                Console.WriteLine("Customer Added!");

                                break;
                        }
                        break;

                    #endregion

                    #region Update Object

                    case 2:
                        break;

                    #endregion

                    #region Object View

                    case 3:
                    

                        Console.WriteLine("\ninsert 2 to view a Drone");
                        Console.WriteLine("insert 3 to view a Customer");
                        MenuChoice = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter ID:");
                        switch (MenuChoice)
                        {
                            case 2:
                                try
                                {
                                    Console.WriteLine(DO.GetDrone(int.Parse(Console.ReadLine())).ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;

                            case 3:
                                try
                                {
                                    Console.WriteLine(DO.GetCustomer(int.Parse(Console.ReadLine())).ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            
                        }
                        Console.WriteLine("");
                        break;

                    #endregion

                    #region List View

                    case 4:

                        Console.WriteLine("insert 2 to view Drones");
                        Console.WriteLine("insert 3 to view Customers");
                        MenuChoice = int.Parse(Console.ReadLine());
                        switch (MenuChoice)
                        {

                            case 2:
                                foreach (IDAL.DO.Drone drone in DO.GetAllDrones())
                                {
                                    Console.WriteLine(drone + "\n");
                                }
                                break;

                            case 3:

                                foreach(IDAL.DO.Customer customer in DO.GetAllCustomers())
                                {
                                    Console.WriteLine(customer + "\n");
                                }

                                break;

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
    }
}
