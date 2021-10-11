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
            int Choice;
            do
            {
                Console.WriteLine("please insert 1 to add a Drone");
                Choice = int.Parse(Console.ReadLine());
                switch (Choice)
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
                    default:
                        flag = false;
                        break;
                }

            } while (flag);
            
        }
    }
}
