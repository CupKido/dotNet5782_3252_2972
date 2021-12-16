using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
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
        public int Id;
        public ItemNotFoundException(int ItemId) : base()
        {
            Id = ItemId;
        }
        public ItemNotFoundException(int ItemId, String message) : base(message)
        {
            Id = ItemId;
        }
        public ItemNotFoundException(int ItemId, String message, Exception inner) : base(message, inner)
        {
            Id = ItemId;
        }
        public override string ToString()
        {
            return "Item with ID: " + Id + " was not found in data!\n" + Message + "\n";
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

}