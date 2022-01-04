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
            XMLTools.CreateFile(baseStationsPath, "BaseStations");
            XMLTools.CreateFile(dronesPath, "Drones");
            XMLTools.CreateFile(parcelsPath, "Parcels");
            XMLTools.CreateFile(customersPath, "Customers");
            XMLTools.CreateFile(droneChargesPath, "DroneCharges");
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

        #region BaseStations

        public void AddBaseStations(int Id, string Name, double Longitude, double Latitude, int ChargeSlots)
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);
            try
            {
                GetBaseStation(Id);
                throw new ItemAlreadyExistsException(Id, "BaseStation Id already taken");
            }
            catch(ItemNotFoundException ex)
            {

            }
            catch
            {
                throw;
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
            return GetAllBaseStations().Where(bs => predicate(bs));
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

        #endregion

        #region Customers

        public void AddCustomer(int Id, string Name, string Phone, double Longitude, double Latitude)
        {
            XElement CustomersRootElem = XMLTools.LoadListFromXMLElement(customersPath);
            try
            {
                GetCustomer(Id);
                throw new ItemAlreadyExistsException(Id, "Customer Id already taken");
            }
            catch (ItemNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }


            XElement newCustomer = new XElement("Customer",
                new XElement("Id", Id),
                new XElement("Name", Name),
                new XElement("Phone", Phone),
                new XElement("Longitude", Longitude),
                new XElement("Latitude", Latitude)
                );
            CustomersRootElem.Add(newCustomer);
            XMLTools.SaveListToXMLElement(CustomersRootElem, customersPath);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            XElement CustomersRootElem = XMLTools.LoadListFromXMLElement(customersPath);

            return from CustomerElem in CustomersRootElem.Elements()
                   select new Customer()
                   {
                       Id = Convert.ToInt32(CustomerElem.Element("Id").Value),
                       Name = CustomerElem.Element("Name").Value,
                       Phone = CustomerElem.Element("Phone").Value,
                       Longitude = Convert.ToDouble(CustomerElem.Element("Longitude").Value),
                       Latitude = Convert.ToDouble(CustomerElem.Element("Latitude").Value)
                   };
        }

        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate)
        {
            return GetAllCustomers().Where(c => predicate(c));
        }

        public Customer GetCustomer(int Id)
        {
            try
            {
                return GetAllCustomersBy(d => d.Id == Id).First();
            }
            catch
            {
                throw new ItemNotFoundException(Id, "Customer Not Found!");
            }
        }

        public Customer RemoveCustomer(int Id)
        {
            XElement CustomersRootElem = XMLTools.LoadListFromXMLElement(customersPath);
            Customer res;
            foreach (XElement CustomerElem in CustomersRootElem.Elements())
            {
                if (Convert.ToInt32(CustomerElem.Element("Id").Value) == Id)
                {
                    res = new Customer()
                    {
                        Id = Convert.ToInt32(CustomerElem.Element("Id").Value),
                        Name = CustomerElem.Element("Name").Value,
                        Phone = CustomerElem.Element("Phone").Value,
                        Longitude = Convert.ToDouble(CustomerElem.Element("Longitude").Value),
                        Latitude = Convert.ToDouble(CustomerElem.Element("Latitude").Value)
                    };
                    CustomerElem.Remove();
                    XMLTools.SaveListToXMLElement(CustomersRootElem, customersPath);
                    return res;
                }
            }
            throw new ItemNotFoundException(Id, "Customer Not Found!");
        }

        #endregion

        #region Drones

        public void AddDrone(int Id, string Model, WeightCategories MaxWeight)
        {
            XElement DronesRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            try
            {
                GetDrone(Id);
                throw new ItemAlreadyExistsException(Id, "Drone ID already taken");
            }
            catch (ItemNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }


            XElement newDrone = new XElement("Drone",
                new XElement("Id", Id),
                new XElement("Model", Model),
                new XElement("MaxWeight", (int)MaxWeight)
                );
            DronesRootElem.Add(newDrone);
            XMLTools.SaveListToXMLElement(DronesRootElem, dronesPath);
        }

        public IEnumerable<Drone> GetAllDrones()
        {
            XElement DronesRootElem = XMLTools.LoadListFromXMLElement(dronesPath);

            return from DroneElem in DronesRootElem.Elements()
                   select new Drone()
                   {
                       Id = Convert.ToInt32(DroneElem.Element("Id").Value),
                       Model = DroneElem.Element("Model").Value,
                       MaxWeight = (WeightCategories)Convert.ToInt32(DroneElem.Element("MaxWeight").Value)
                   };
        }

        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return GetAllDrones().Where(d => predicate(d));
        }

        public Drone GetDrone(int Id)
        {
            try
            {
                return GetAllDronesBy(d => d.Id == Id).First();
            }
            catch
            {
                throw new ItemNotFoundException(Id, "Drone Not Found!");
            }
        }

        public Drone RemoveDrone(int Id)
        {
            XElement DronesRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            Drone res;
            foreach (XElement DroneElem in DronesRootElem.Elements())
            {
                if (Convert.ToInt32(DroneElem.Element("Id").Value) == Id)
                {
                    res = new Drone()
                    {
                        Id = Convert.ToInt32(DroneElem.Element("Id").Value),
                        Model = DroneElem.Element("Model").Value,
                        MaxWeight = (WeightCategories)Convert.ToInt32(DroneElem.Element("MaxWeight").Value)
                    };
                    DroneElem.Remove();
                    XMLTools.SaveListToXMLElement(DronesRootElem, dronesPath);
                    return res;
                }
            }
            throw new ItemNotFoundException(Id, "Drone Not Found!");
        }

        #endregion

        #region DroneCharges

        public void AddDroneCharge(int DroneId, int BaseStationId, DateTime started)
        {
            XElement DCRootElem = XMLTools.LoadListFromXMLElement(droneChargesPath);
            try
            {
                GetDroneCharge(DroneId);
                throw new ItemAlreadyExistsException(DroneId, "Drone is already being charged");
            }
            catch (ItemNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }


            XElement newDC = new XElement("Drone",
                new XElement("DroneId", DroneId),
                new XElement("BaseStationId", BaseStationId),
                new XElement("Started", started)
                );
            DCRootElem.Add(newDC);
            XMLTools.SaveListToXMLElement(DCRootElem, droneChargesPath);
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges()
        {
            XElement DroneChargesRootElem = XMLTools.LoadListFromXMLElement(droneChargesPath);

            return from DCElem in DroneChargesRootElem.Elements()
                   select new DroneCharge()
                   {
                       DroneId = Convert.ToInt32(DCElem.Element("DroneId").Value),
                       BaseStationId = Convert.ToInt32(DCElem.Element("BaseStationId").Value),
                       Started = Convert.ToDateTime(DCElem.Element("Started").Value)
                   };
        }

        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate)
        {
            return GetAllDroneCharges().Where(dc => predicate(dc));
        }

        public DroneCharge GetDroneCharge(int DroneId)
        {
            try
            {
                return GetAllDroneChargesBy(d => d.DroneId == DroneId).First();
            }
            catch
            {
                throw new ItemNotFoundException(DroneId, "Drone Charging Not Found!");
            }
        }

        public DroneCharge RemoveDroneCharge(int DroneId)
        {
            XElement DCRootElem = XMLTools.LoadListFromXMLElement(droneChargesPath);
            DroneCharge res;
            foreach (XElement DCElem in DCRootElem.Elements())
            {
                if (Convert.ToInt32(DCElem.Element("DroneId").Value) == DroneId)
                {
                    res = new DroneCharge()
                    {
                        DroneId = Convert.ToInt32(DCElem.Element("DroneId").Value),
                        BaseStationId = Convert.ToInt32(DCElem.Element("BaseStationId").Value),
                        Started = Convert.ToDateTime(DCElem.Element("Started").Value)
                    };
                    DCElem.Remove();
                    XMLTools.SaveListToXMLElement(DCRootElem, droneChargesPath);
                    return res;
                }
            }
            throw new ItemNotFoundException(DroneId, "Drone is not being charged!");
        }

        #endregion

        public void AddParcel(int Id, int SenderId, int TargetId, WeightCategories PackageWight, Priorities priority, DateTime created)
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
            return GetAllParcels().Where(p => predicate(p));
        }

        


        

        

        public Parcel GetParcel(int Id)
        {
            try
            {
                return GetAllParcelsBy(d => d.Id == Id).First();
            }
            catch
            {
                throw new ItemNotFoundException(Id, "Parcel Not Found!");
            }
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
