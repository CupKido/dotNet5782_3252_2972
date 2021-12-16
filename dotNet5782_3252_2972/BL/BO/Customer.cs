using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public Location Address { get; set; }

        public List<ParcelInCustomer> FromThisCustomer = new List<ParcelInCustomer>();

        public List<ParcelInCustomer> ToThisCustomer = new List<ParcelInCustomer>();

        public override string ToString()
        {

            #region Longitude & Latitude Calculations

            char lon = 'N';
            if (Address.Longitude < 0)
            {
                lon = 'S';
                Address.Longitude *= -1;
            }
            double lonDegreesWithFraction = Address.Longitude;
            int londegrees = (int)lonDegreesWithFraction; // = 48

            double lonfractionalDegrees = lonDegreesWithFraction - londegrees; // = .858222
            double lonminutesWithFraction = 60 * lonfractionalDegrees; // = 51.49332
            int lonminutes = (int)lonminutesWithFraction; // = 51

            double lonfractionalMinutes = lonminutesWithFraction - lonminutes; // = .49332
            double lonsecondsWithFraction = 60 * lonfractionalMinutes; // = 29.6

            char lat = 'E';
            if (Address.Latitude < 0)
            {
                lat = 'W';
                Address.Latitude *= -1;
            }

            double latDegreesWithFraction = Address.Latitude;
            int latdegrees = (int)latDegreesWithFraction; // = 48

            double latfractionalDegrees = latDegreesWithFraction - latdegrees; // = .858222
            double latminutesWithFraction = 60 * latfractionalDegrees; // = 51.49332
            int latminutes = (int)latminutesWithFraction; // = 51

            double latfractionalMinutes = latminutesWithFraction - latminutes; // = .49332
            double latsecondsWithFraction = 60 * latfractionalMinutes; // = 29.6

            #endregion

            string FromThisCustomerString = "";
            string ToThisCustomerString = "";

            foreach(ParcelInCustomer pic in FromThisCustomer)
            {
                FromThisCustomerString += pic.ToString() + "\n";
            }

            foreach (ParcelInCustomer pic in ToThisCustomer)
            {
                ToThisCustomerString += pic.ToString() + "\n";
            }

            return "ID: " + Id + "\nName: " + Name + "\nPhone: " + Phone +
                "\nLongitude: " + londegrees + "°" + lonminutes + "'" + Math.Round(lonsecondsWithFraction, 3) + "\"" + lon +
                "\nLatitude: " + latdegrees + "°" + latminutes + "'" + Math.Round(latsecondsWithFraction, 3) + "\"" + lat + "\n" +
                "Arriving Parcels:\n" + ToThisCustomerString + "\n" + "Sent Parcels:\n" + FromThisCustomerString;

        }
    }
}
