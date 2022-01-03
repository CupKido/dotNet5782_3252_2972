using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace DalXml
{
    internal class DalXml : DalApi.IDal
    {
        internal static DalXml instance = null;
        private static object locker = new object();
        private DalXml()
        {
            CreateConfig();
            CreateAllFiles();
        }

        private void CreateConfig()
        {
            if (!File.Exists(configPath))
            {
                Double AvailbleElec = 20;
                Double LightElec = 35;
                Double IntermediateElec = 50;
                Double HeavyElec = 80;
                Double ChargePerHours = 40;
                double[] a = { AvailbleElec, LightElec, IntermediateElec, HeavyElec, ChargePerHours};
                XElement Elec = new XElement("Elec", 
                    new XElement("AvailbleElec", AvailbleElec),
                    new XElement("LightElec", LightElec),
                    new XElement("IntermediateElec", IntermediateElec),
                    new XElement("HeavyElec", HeavyElec),
                    new XElement("ChargePerHours", ChargePerHours)
                    );
                XElement config = new XElement("Config", Elec);
                XMLTools.SaveListToXMLElement(config, configPath);
            }
        }

        private void CreateAllFiles()
        {
            if (!File.Exists(baseStationsPath))
            {
                XElement baseStations = new XElement("BaseStations");
                XMLTools.SaveListToXMLElement(baseStations, baseStationsPath);
            }
            if (!File.Exists(dronesPath))
            {
                XElement drones = new XElement("Drones");
                XMLTools.SaveListToXMLElement(drones, dronesPath);
            }
            if (!File.Exists(parcelsPath))
            {
                XElement parcels = new XElement("Parcels");
                XMLTools.SaveListToXMLElement(parcels, parcelsPath);
            }
            if (!File.Exists(customersPath))
            {
                XElement customers = new XElement("Customers");
                XMLTools.SaveListToXMLElement(customers, customersPath);
            }
            if (!File.Exists(droneChargesPath))
            {
                XElement droneCharges = new XElement("DroneCharges");
                XMLTools.SaveListToXMLElement(droneCharges, droneChargesPath);
            }

        }

        public static DalXml GetInstance()
        {

            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new DalXml();
                    }
                }
            }
            return instance;
        }

        string customersPath = @"CustomersXml.xml";
        string dronesPath = @"DronesXml.xml";
        string baseStationsPath = @"BaseStationsXml.xml";
        string parcelsPath = @"ParcelsXml.xml";
        string droneChargesPath = @"DroneChargesXml.xml";
        string configPath = @"ConfigXml.xml";

        public void AddBaseStations(int Id, string Name, double Longitude, double Latitude, int ChargeSlots)
        {
            XElement BSRootElem;
            if (!File.Exists(baseStationsPath))
            {
                BSRootElem = new XElement("BaseStations");
            }
            else 
            { 
                BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);
                try
            {
                GetBaseStation(Id);
            }
            catch(ItemNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }
            }

            XElement newBS = new XElement("BaseStation", 
                new XElement("Id", Id),
                new XElement("Name", Name),
                new XElement("Longitude", Longitude),
                new XElement("Latitude", Latitude),
                new XElement("ChargeSlots", ChargeSlots)
                );
            BSRootElem.Add(newBS);
            XMLTools.SaveListToXMLElement(BSRootElem, baseStationsPath);
        }

        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);

            return from BSElem in BSRootElem.Elements()
                   select new BaseStation()
                   {
                       Id = Convert.ToInt32(BSElem.Element("Id").Value),
                       Name = BSElem.Element("Name").Value,
                       ChargeSlots = Convert.ToInt32(BSElem.Element("ChargeSlots").Value),
                       Longitude = Convert.ToDouble(BSElem.Element("Longitude").Value),
                       Latitude = Convert.ToDouble(BSElem.Element("Latitude").Value)
                   };
        }

        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate)
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);

            return from BSElem in BSRootElem.Elements()
                   let BS = new BaseStation()
                   {
                       Id = Convert.ToInt32(BSElem.Element("Id").Value),
                       Name = BSElem.Element("Name").Value,
                       ChargeSlots = Convert.ToInt32(BSElem.Element("ChargeSlots").Value),
                       Longitude = Convert.ToDouble(BSElem.Element("Longitude").Value),
                       Latitude = Convert.ToDouble(BSElem.Element("Latitude").Value)
                   }
                   where predicate(BS)
                   select BS;
        }

        public BaseStation GetBaseStation(int Id)
        {
            try
            {
                return GetAllBaseStationsBy(d => d.Id == Id).First();
            }
            catch
            {
                throw new ItemNotFoundException(Id, "Base Station Not Found!");
            }
        }

        public void SetBaseStation(BaseStation newBS)
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);
            foreach(XElement BSElem in BSRootElem.Elements())
            {
                if(Convert.ToInt32(BSElem.Element("Id").Value) == newBS.Id)
                {
                    BSElem.Element("Latitude").Value = newBS.Latitude.ToString();
                    BSElem.Element("Longitude").Value = newBS.Longitude.ToString();
                    BSElem.Element("Name").Value = newBS.Name;
                    BSElem.Element("ChargeSlots").Value = newBS.ChargeSlots.ToString();
                    XMLTools.SaveListToXMLElement(BSRootElem, baseStationsPath);
                    return;
                }
            }
        }

        public BaseStation RemoveBaseStation(int Id)
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);
            BaseStation res = new BaseStation();
            foreach (XElement BSElem in BSRootElem.Elements())
            {
                if (Convert.ToInt32(BSElem.Element("Id").Value) == Id)
                {
                    res.Id = Id;
                    res.Name = BSElem.Element("Name").Value;
                    res.ChargeSlots = Convert.ToInt32(BSElem.Element("ChargeSlots").Value);
                    res.Longitude = Convert.ToDouble(BSElem.Element("Longitude").Value);
                    res.Latitude = Convert.ToDouble(BSElem.Element("Latitude").Value);
                    BSElem.Remove();
                    XMLTools.SaveListToXMLElement(BSRootElem, baseStationsPath);
                    return res;
                }
            }
            throw new ItemNotFoundException(Id, "Base Station Not Found!");
        }

        public void AddCustomer(int Id, string Name, string Phone, double Longitude, double Latitude)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(int Id, string Model, WeightCategories MaxWeight)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(int DroneId, int BaseStationId, DateTime started)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(int Id, int SenderId, int TargetId, WeightCategories PackageWight, Priorities priority, DateTime created)
        {
            throw new NotImplementedException();
        }

        public double[] AskForElectricity()
        {
            XElement config = XMLTools.LoadListFromXMLElement(configPath);
            XElement Elec = config.Element("Elec");
            double[] arr = new double[5];
            arr[0] = Convert.ToDouble(Elec.Element("AvailbleElec").Value);
            arr[1] = Convert.ToDouble(Elec.Element("LightElec").Value);
            arr[2] = Convert.ToDouble(Elec.Element("IntermediateElec").Value);
            arr[3] = Convert.ToDouble(Elec.Element("HeavyElec").Value);
            arr[4] = Convert.ToDouble(Elec.Element("ChargePerHours").Value);
            return arr;
        }

        

        

        public IEnumerable<Customer> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> a = new List<Drone>();
            return a;
        }

        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            XElement ParcelsRootElem = XMLTools.LoadListFromXMLElement(parcelsPath);

            return from ParcelElem in ParcelsRootElem.Elements()
                   select new Parcel()
                   {
                       Id = Convert.ToInt32(ParcelElem.Element("Id").Value),
                       DroneId = Convert.ToInt32(ParcelElem.Element("DroneId").Value),
                       SenderId = Convert.ToInt32(ParcelElem.Element("SenderId").Value),
                       TargetId = Convert.ToInt32(ParcelElem.Element("TargetId").Value),
                       Weight = (WeightCategories)Convert.ToInt32(ParcelElem.Element("Weight").Value),
                       Priority = (Priorities)Convert.ToInt32(ParcelElem.Element("Priority").Value),
                       Requested = Convert.ToDateTime(ParcelElem.Element("Requested").Value),
                       Scheduled = Convert.ToDateTime(ParcelElem.Element("Scheduled").Value),
                       PickedUp = Convert.ToDateTime(ParcelElem.Element("PickedUp").Value),
                       Delivered = Convert.ToDateTime(ParcelElem.Element("Delivered").Value)
                   };
        }

        public IEnumerable<Parcel> GetAllParcelsBy(Predicate<Parcel> predicate)
        {
            throw new NotImplementedException();
        }

        

        public Customer GetCustomer(int Id)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int Id)
        {
            throw new NotImplementedException();
        }

        public DroneCharge GetDroneCharge(int DroneId)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int Id)
        {
            throw new NotImplementedException();
        }

        

        public Customer RemoveCustomer(int Id)
        {
            throw new NotImplementedException();
        }

        public Drone RemoveDrone(int Id)
        {
            throw new NotImplementedException();
        }

        public DroneCharge RemoveDroneCharge(int DroneId)
        {
            throw new NotImplementedException();
        }

        public Parcel RemoveParcel(int Id)
        {
            throw new NotImplementedException();
        }

        

        public void SetCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void SetDrone(Drone newDrone)
        {
            throw new NotImplementedException();
        }

        public void SetDroneCharge(DroneCharge newDC)
        {
            throw new NotImplementedException();
        }

        public void SetParcel(Parcel newParcel)
        {
            throw new NotImplementedException();
        }
    }
}
