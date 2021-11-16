using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {

        public int Id { get; set; }

        public String Name { get; set; }

        public String Phone { get; set; }

        public int SentAndDelivered { get; set; }

        public int SentAndNotDelivered { get; set; }

        public int Recieved { get; set; }

        public int OnTheWay { get; set; }

    }
}
