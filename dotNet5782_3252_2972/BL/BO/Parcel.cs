using System;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public IDAL.DO.WeightCategories Weight { get; set; }
        public IDAL.DO.Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime Requested { get; set; }
        public DateTime scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }

        public override string ToString()
        {
            String res = "ID: " + Id + "\nSender ID: " + Sender.Id + "   Target ID: " + Target.Id +
                "\nWeight: " + Weight + "   Priority: " + Priority + "\nRequested: " + Requested;
            if (DroneId != 0)
            {
                res += "\nScheduled: " + scheduled + "  Drone's ID: " + DroneId;
            }
            return res;
        }
    }
}
