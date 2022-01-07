using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
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
            XMLTools.CreateConfig(configPath, GetAllParcels().Count());
            CreateAllFiles();
        }

        

        private void CreateAllFiles()
        {
            XMLTools.CreateFile(baseStationsPath, "BaseStations");
            XMLTools.CreateFile(dronesPath, "Drones");
            XMLTools.CreateFile(customersPath, "Customers");
            XMLTools.CreateFile(droneChargesPath, "DroneCharges");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            XElement BSRootElem = XMLTools.LoadListFromXMLElement(baseStationsPath);

            try
            {
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
            catch
            {
                return new List<BaseStation>();
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStationsBy(Predicate<BaseStation> predicate)
        {
            return GetAllBaseStations().Where(bs => predicate(bs));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomersBy(Predicate<Customer> predicate)
        {
            return GetAllCustomers().Where(c => predicate(c));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCustomer(Customer newCustomer)
        {
            XElement CustomersRootElem = XMLTools.LoadListFromXMLElement(customersPath);
            foreach (XElement CustomerElem in CustomersRootElem.Elements())
            {
                if (Convert.ToInt32(CustomerElem.Element("Id").Value) == newCustomer.Id)
                {
                    CustomerElem.Element("Name").Value = newCustomer.Name;
                    CustomerElem.Element("Phone").Value = newCustomer.Phone;
                    CustomerElem.Element("Longitude").Value = newCustomer.Longitude.ToString();
                    CustomerElem.Element("Latitude").Value = newCustomer.Latitude.ToString();
                    
                    XMLTools.SaveListToXMLElement(CustomersRootElem, customersPath);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return GetAllDrones().Where(d => predicate(d));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetDrone(Drone newDrone)
        {
            XElement DronesRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            foreach (XElement DroneElem in DronesRootElem.Elements())
            {
                if (Convert.ToInt32(DroneElem.Element("Id").Value) == newDrone.Id)
                {
                    DroneElem.Element("Model").Value = newDrone.Model;
                    DroneElem.Element("MaxWeight").Value = ((int)newDrone.MaxWeight).ToString();
                    XMLTools.SaveListToXMLElement(DronesRootElem, dronesPath);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDroneChargesBy(Predicate<DroneCharge> predicate)
        {
            return GetAllDroneCharges().Where(dc => predicate(dc));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetDroneCharge(DroneCharge newDC)
        {
            XElement DCRootElem = XMLTools.LoadListFromXMLElement(droneChargesPath);
            foreach (XElement DCElem in DCRootElem.Elements())
            {
                if (Convert.ToInt32(DCElem.Element("DroneId").Value) == newDC.DroneId)
                {
                    DCElem.Element("BaseStationId").Value = newDC.BaseStationId.ToString();
                    DCElem.Element("Started").Value = newDC.Started.ToString();
                    XMLTools.SaveListToXMLElement(DCRootElem, droneChargesPath);
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        #region Parcels

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel( int SenderId, int TargetId, WeightCategories PackageWight, Priorities priority, DateTime created)
        {
           
            List<Parcel> Parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            XElement config = XMLTools.LoadListFromXMLElement(configPath);
            int num = Convert.ToInt32(config.Element("ParcelsRunningNum").Value);
            Parcels.Add(new Parcel()
            {
                Id = num,
                SenderId = SenderId,
                TargetId = TargetId,
                Weight = PackageWight,
                Requested = created
            });
            config.Element("ParcelsRunningNum").Value = (num + 1).ToString();
            XMLTools.SaveListToXMLSerializer<Parcel>(Parcels, parcelsPath);
            XMLTools.SaveListToXMLElement(config, configPath);
            return num;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels()
        {
            try
            {
                return XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            }
            catch
            {
                return new List<Parcel>();
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcelsBy(Predicate<Parcel> predicate)
        {
            return GetAllParcels().Where(p => predicate(p));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetParcel(Parcel newParcel)
        {
            List<Parcel> Parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            foreach (Parcel exParcel in Parcels)
            {
                if (exParcel.Id == newParcel.Id)
                {
                    Parcels.Remove(exParcel);
                    Parcels.Add(newParcel);
                    XMLTools.SaveListToXMLSerializer<Parcel>(Parcels, parcelsPath);
                    return;
                }
            }
            throw new ItemNotFoundException(newParcel.Id, "Parcel Not Found!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel RemoveParcel(int Id)
        {
            
            List<Parcel> Parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            try
            {
                Parcel res = Parcels.First(p => p.Id == Id);
                Parcels.Remove(res);

                XMLTools.SaveListToXMLSerializer<Parcel>(Parcels, parcelsPath);

                return res;
            }
            catch
            {
                throw new ItemNotFoundException(Id, "Parcel Not Found!");
            }

            
        }

        #endregion
    }
}
