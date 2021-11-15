using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationToList
    {

        public int Id { get; set; }

        public String Name { get; set; }

        public int ChargeSlotsAvailible { get; set; }

        public int ChargeSlotsTaken { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "  Name: " + Name + "   Availible slots: " + ChargeSlotsAvailible + "  Taken slots: " + ChargeSlotsTaken;
        }
    }
}
