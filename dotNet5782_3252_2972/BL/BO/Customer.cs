using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {

            #region Longitude & Latitude Calculations

            char lon = 'N';
            if (Longitude < 0)
            {
                lon = 'S';
                Longitude *= -1;
            }
            double lonDegreesWithFraction = Longitude;
            int londegrees = (int)lonDegreesWithFraction; // = 48

            double lonfractionalDegrees = lonDegreesWithFraction - londegrees; // = .858222
            double lonminutesWithFraction = 60 * lonfractionalDegrees; // = 51.49332
            int lonminutes = (int)lonminutesWithFraction; // = 51

            double lonfractionalMinutes = lonminutesWithFraction - lonminutes; // = .49332
            double lonsecondsWithFraction = 60 * lonfractionalMinutes; // = 29.6

            char lat = 'E';
            if (Latitude < 0)
            {
                lat = 'W';
                Latitude *= -1;
            }

            double latDegreesWithFraction = Latitude;
            int latdegrees = (int)latDegreesWithFraction; // = 48

            double latfractionalDegrees = latDegreesWithFraction - latdegrees; // = .858222
            double latminutesWithFraction = 60 * latfractionalDegrees; // = 51.49332
            int latminutes = (int)latminutesWithFraction; // = 51

            double latfractionalMinutes = latminutesWithFraction - latminutes; // = .49332
            double latsecondsWithFraction = 60 * latfractionalMinutes; // = 29.6

            #endregion

            return "ID: " + Id + "\nName: " + Name + "\nPhone: " + Phone +
                "\nLongitude: " + londegrees + "°" + lonminutes + "'" + Math.Round(lonsecondsWithFraction, 3) + "\"" + lon +
                "\nLatitude: " + latdegrees + "°" + latminutes + "'" + Math.Round(latsecondsWithFraction, 3) + "\"" + lat;

        }
    }
}
