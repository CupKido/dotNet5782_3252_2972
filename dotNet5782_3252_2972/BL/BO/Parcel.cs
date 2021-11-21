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
            return "ID: " + Id +
                "\nSender ID: " + Sender.Id + " Name: " + Sender.Name +
                "\nTarget ID: " + Target.Id + " Name: " + Target.Name +
                "\nWeight: " + Weight + "   Priority: " + Priority + 
                "\nRequested: " + Requested +
                ((DroneId != 0) ? ("\nDrone's ID: " + DroneId + "\nScheduled: " + scheduled) : "") +
                ((PickedUp != DateTime.MinValue) ? ("\nPicked Up Date: " + PickedUp) : "") +
                ((Delivered != DateTime.MinValue) ? ("\nDelivered Date: " + Delivered) : "");
        }
    }
}
