using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DataSource DS = new DalObject.DataSource();
            DalObject.DalObject DO = new DalObject.DalObject();
            bool flag = true;
            int MenuChoice;
            do
            {
                Console.WriteLine("please insert 1 to add an Object");
                MenuChoice = int.Parse(Console.ReadLine());
                switch (MenuChoice)
                {
                    case 1:
                        Console.WriteLine("please insert 1 to add a Drone");
                        MenuChoice = int.Parse(Console.ReadLine());
                        switch (MenuChoice)
                        {
                            case 1:
                                try
                                {
                                    DO.AddDrone(11, "blab", IDAL.DO.WeightCategories.Heavy);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                        }
                        
                        break;
                    case 2:
                        try
                        {
                            Console.WriteLine(DO.GetDrone(int.Parse(Console.ReadLine())).ToString());
                        }catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default:
                        flag = false;
                        break;
                }

            } while (flag);
            
        }    }
}
