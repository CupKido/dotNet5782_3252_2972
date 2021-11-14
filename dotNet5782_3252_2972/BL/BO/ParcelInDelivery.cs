using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInDelivery
    {
        public int Id { get; set; }

        public bool parcelStage { get; set; }

        public Priorities priority { get; set; }

        public CustomerInDelivery Sender { get; set; }

        public CustomerInDelivery Target { get; set; }

        public Location PickUp { get; set; }

        public Location Drop { get; set; }

        public double DeliveryDistance { get; set; }
    }
}
