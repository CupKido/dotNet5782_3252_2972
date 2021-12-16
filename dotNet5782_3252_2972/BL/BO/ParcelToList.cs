using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList : IComparable
    {

        public int Id { get; set; }

        public String SenderName { get; set; }

        public String TargetName { get; set; }

        public WeightCategories Weight { get; set; }

        public Priorities Priority { get; set; }

        public ParcelStatuses Status { get; set; }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((ParcelToList)obj).Id);
        }

        public override string ToString()
        {
            return "ID: " + Id + " | " + "Sender's Name: " + SenderName + " | " + "Target's Name: " + TargetName + " | " + "Weight: " + Weight + " | " + "Priority: " + Priority + " | " + "Status: " + Status;
        }

    }
}
