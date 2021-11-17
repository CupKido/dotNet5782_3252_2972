using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInCustomer
    {

        public int Id { get; set; }

        public WeightCategories Weight { get; set; }

        public Priorities Priority { get; set; }

        public ParcelStatuses Status { get; set; }

        public CustomerInParcel OtherSide { get; set; } //if customer is target, then sender, and if sender, target

        public override string ToString()
        {
            return "ID: " + Id + " | " + " Weight: " + Weight + " | " + " Priority: " + Priority + " | " + " Status: " + Status + " | " + " Other Side's ID: " + OtherSide.Id + " | " + " Name: " + OtherSide.Name;
        }
    }
}
