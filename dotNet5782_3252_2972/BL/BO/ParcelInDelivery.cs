using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInDelivery
    {
        public int Id { get; set; }

        public bool ParcelStatus { get; set; } // waiting for pickup or on the way to target

        public Priorities priority { get; set; }

        public CustomerInParcel Sender { get; set; }

        public CustomerInParcel Target { get; set; }

        public Location PickUp { get; set; }

        public Location Drop { get; set; }

        public double DeliveryDistance { get; set; }
    }
}
