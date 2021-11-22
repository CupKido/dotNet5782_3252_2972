using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ItemAlreadyExistsException : Exception
    {
        public int Id;
        public ItemAlreadyExistsException(int ItemId) : base()
        {
            Id = ItemId;
        }
        public ItemAlreadyExistsException(int ItemId, String message) : base(message)
        {
            Id = ItemId;
        }
        public ItemAlreadyExistsException(int ItemId, String message, Exception inner) : base(message, inner)
        {
            Id = ItemId;
        }
        public override string ToString()
        {
            return "Item with ID: " + Id + " already exists in data!\n" + Message + "\n";
        }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(String message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return InnerException.Message + "\n" + Message + "\n";
        }
    }

    public class IllegalActionException : Exception
    {
        public IllegalActionException() : base("") { }

        public IllegalActionException(String message) : base(message) { }

        public IllegalActionException(String message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return "Illegal Action has been attempted!\n" + Message + "\n";
        }
    }


    public class NegetiveNumberException : Exception
    {
        public int Number;
        public NegetiveNumberException(int number) : base()
        {
            Number = number;
        }
        public NegetiveNumberException(int number, String message) : base(message)
        {
            Number = number;
        }
        public NegetiveNumberException(int number, String message, Exception inner) : base(message, inner)
        {
            Number = number;
        }
        public override string ToString()
        {
            return "ERROR: Input cannot be negetive\nNegetive Number inserted: " + Number + "\n" + Message;
        }
    }

    public class InvalidNumberLengthException : Exception
    {
        public int Number;
        public InvalidNumberLengthException(int number) : base()
        {
            Number = number;
        }
        public InvalidNumberLengthException(int number, String message) : base(message)
        {
            Number = number;
        }
        public InvalidNumberLengthException(int number, String message, Exception inner) : base(message, inner)
        {
            Number = number;
        }
        public override string ToString()
        {
            return "ERROR: Invalid Number Length!\nInvalid number length inserted: " + Number + "\n" + Message;
        }
    }

    public class OutOfRangeException : Exception
    {
        public int Number;
        public OutOfRangeException(int number) : base()
        {
            Number = number;
        }
        public OutOfRangeException(int number, String message) : base(message)
        {
            Number = number;
        }
        public OutOfRangeException(int number, String message, Exception inner) : base(message, inner)
        {
            Number = number;
        }
        public override string ToString()
        {
            return "ERROR: Invalid Number has been chosen!\nInvalid number inserted: " + Number + "\n" + Message;
        }
    }

    public class NotEnoughDroneBatteryException : Exception
    {
        public int DroneId;

        public NotEnoughDroneBatteryException(int droneId)
        {
            DroneId = droneId;
        }

        public NotEnoughDroneBatteryException(int droneId, string message) : base(message)
        {
            DroneId = droneId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " does not have enough battery to get to charging" + "\n" + Message;
        }
    }

    public class DroneIsntAvailibleException : Exception
    {
        public int DroneId;

        public DroneIsntAvailibleException(int droneId)
        {
            DroneId = droneId;
        }

        public DroneIsntAvailibleException(int droneId, string message) : base(message)
        {
            DroneId = droneId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " is not availible, so it cant be charged." + "\n" + Message;
        }
    }

    public class DroneIsBusy : Exception
    {
        public int DroneId;

        public DroneIsBusy(int droneId)
        {
            DroneId = droneId;
        }

        public DroneIsBusy(int droneId, string message) : base(message)
        {
            DroneId = droneId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " is not availible, so it can not be connect to the parcel" + "\n" + Message;
        }
    }

    public class DroneIsAvailibleException : Exception
    {
        public int DroneId;

        public DroneIsAvailibleException(int droneId)
        {
            DroneId = droneId;
        }

        public DroneIsAvailibleException(int droneId, string message) : base(message)
        {
            DroneId = droneId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " is availible, so it cant be discharged." + "\n" + Message;
        }
    }

    public class BaseStationFullException : Exception
    {
        public int DroneId;
        public int BSId;
        
        public BaseStationFullException(int droneId, int bSId)
        {
            DroneId = droneId;
            BSId = bSId;
        }

        public BaseStationFullException(int droneId, int bSId, string message) : base(message)
        {
            DroneId = droneId;
            BSId = bSId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " cannot be charged at base station " + BSId + " Since the base station is full." + "\n" + Message;
        }
    }



    public class StatusIsntInDelivery : Exception
    {
        public int DroneId;

        public StatusIsntInDelivery(int droneId)
        {
            DroneId = droneId;
        }

        public StatusIsntInDelivery(int droneId, string message) : base(message)
        {
            DroneId = droneId;
        }

        public override string ToString()
        {
            return "ERROR: Drone " + DroneId + " is not in InDelivery Status " + "\n" + Message;
        }
    }

    public class ParcelAlreadyPickedUp: Exception
    {
        public int Id;

        public ParcelAlreadyPickedUp(int id)
        {
           Id = id;
        }

        public ParcelAlreadyPickedUp(int id, string message) : base(message)
        {
           Id = id;
        }

        public override string ToString()
        {
            return "ERROR: Parcel " + Id + " already pikced up " + "\n" + Message;
        }
    }

        public class ParcelAlreadySupply : Exception
    {
        public int Id;

        public ParcelAlreadySupply(int id)
        {
            Id = id;
        }

        public ParcelAlreadySupply(int id, string message) : base(message)
        {
            Id = id;
        }

        public override string ToString()
        {
            return "ERROR: Parcel " + Id + " already delivered " + "\n" + Message;
        }
    }
}
