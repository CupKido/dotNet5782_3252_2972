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
}
