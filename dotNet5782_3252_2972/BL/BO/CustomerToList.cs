using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerToList : IComparable
    {

        public int Id { get; set; }

        public String Name { get; set; }

        public String Phone { get; set; }

        public int SentAndDelivered { get; set; }

        public int SentAndNotDelivered { get; set; }

        public int Recieved { get; set; }

        public int OnTheWay { get; set; }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((CustomerToList)obj).Id);
        }

        public override string ToString()
        {
            return "ID: " + Id + " Name: " + Name + " Phone: " + Phone + " Sent And Delivered: " + SentAndDelivered + " Sent but not Delivered: " + SentAndNotDelivered + " Recieved: " + Recieved + " On The Way: " + OnTheWay;
        }

    }
}
