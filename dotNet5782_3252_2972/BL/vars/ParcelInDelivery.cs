using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInDelivery
    {
        public CustomerInDelivery Sender { get; set; }
        public CustomerInDelivery Target { get; set; }
        public int Id { get; set; }
        public Priorities priority { get; set; }
    }
}
