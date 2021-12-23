using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using DalApi;


namespace BLobject
{
    public partial class BL : BlApi.IBL // IBL.IBL
    {

        public DalApi.IDal dal; //with dal we have access to data source
        List<DroneToList> BLDrones = new List<DroneToList>();
        public Double AvailbleElec { get; set; }
        public Double LightElec { get; set; }
        public Double IntermediateElec { get; set; }
        public Double HeavyElec { get; set; }
        public Double ChargePerHours { get; set; }
        private static int runningNumForParcels { get; set; }
        internal static BL instance=null;
        private static object locker = new object();

        private BL()
        {
            dal = DalFactory.GetDal("List"); 

            runningNumForParcels = dal.GetAllParcels().Count() + 1;
            double[] arr = dal.AskForElectricity();
            AvailbleElec = arr[0];
            LightElec = arr[1];
            IntermediateElec = arr[2];
            HeavyElec = arr[3];
            ChargePerHours = arr[4];
            List<DO.Customer> SatisfiedCustomers = new List<DO.Customer>();
            List<DO.Drone> DalDronesList = dal.GetAllDrones().ToList();

            Random r = new Random();

            //if in delivery
            foreach (DO.Parcel p in dal.GetAllParcels())
            {
                if (p.DroneId != 0)
                {
                    DO.Drone PDrone = dal.GetDrone(p.DroneId);
                    if (p.Delivered == null)
                    {

                        DroneToList newD = new DroneToList();

                        newD.Status = DroneStatuses.InDelivery;
                        newD.Id = PDrone.Id;
                        newD.MaxWeight = (WeightCategories)PDrone.MaxWeight;
                        newD.Model = PDrone.Model;
                        newD.CarriedParcelId = p.Id;

                        DO.Customer sender = dal.GetCustomer(p.SenderId);
                        DO.Customer target = dal.GetCustomer(p.TargetId);

                        if (p.PickedUp == null)
                        {
                            BaseStation closestToSender = closestBaseStation(sender.Longitude, sender.Latitude);
                            BaseStation closestToTarget = closestBaseStation(target.Longitude, target.Latitude);
                            newD.CurrentLocation = new Location()
                            {
                                Latitude = closestToSender.StationLocation.Latitude,
                                Longitude = closestToSender.StationLocation.Longitude
                            };


                            newD.Battery =
                                getDistanceFromLatLonInKm(closestToSender.StationLocation.Latitude, closestToSender.StationLocation.Longitude, sender.Latitude, sender.Longitude) / AvailbleElec + //to sender
                                getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) / arr[(int)p.Weight + 1] + //to target
                                getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.StationLocation.Latitude, closestToTarget.StationLocation.Longitude) / AvailbleElec; //to station
                        }
                        else
                        {
                            BaseStation closestToTarget = closestBaseStation(target.Longitude, target.Latitude);
                            newD.CurrentLocation = new Location()
                            {
                                Latitude = sender.Latitude,
                                Longitude = sender.Longitude
                            };
                            newD.Battery =
                                getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) / arr[(int)p.Weight + 1] + //to target
                                getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.StationLocation.Latitude, closestToTarget.StationLocation.Longitude) / AvailbleElec; //to station
                        }

                        if (newD.Battery > 100)
                        {
                            newD.Battery = 100;
                        }
                        else
                        {
                            newD.Battery = r.Next((int)newD.Battery, 99) + 1;
                        }
                        BLDrones.Add(newD);
                        DalDronesList.RemoveAll(d => d.Id == PDrone.Id);
                    }
                    else if (p.Delivered != null)
                    {
                        SatisfiedCustomers.Add(dal.GetCustomer(p.TargetId));
                    }
                }
            }

            //if not in delivery
            foreach (DO.Drone d in DalDronesList)
            {
                DroneToList newD = new DroneToList();
                newD.Id = d.Id;
                newD.MaxWeight = (WeightCategories)d.MaxWeight;
                newD.Model = d.Model;
                switch (r.Next(2))
                {
                    case 0:
                        {
                            newD.Status = DroneStatuses.Availible;
                            if (SatisfiedCustomers.Count != 0)
                            {
                                DO.Customer SC = SatisfiedCustomers[r.Next(SatisfiedCustomers.Count)];
                                BaseStation bs = closestBaseStation(SC.Longitude, SC.Latitude);
                                double percentToStation = getDistanceFromLatLonInKm(SC.Latitude, SC.Longitude, bs.StationLocation.Latitude, bs.StationLocation.Longitude) / (double)AvailbleElec;
                                if (percentToStation >= 100)
                                {
                                    newD.Battery = 100;
                                }
                                else newD.Battery = r.Next((int)percentToStation, 101);
                                newD.CurrentLocation = new Location()
                                {
                                    Latitude = SC.Longitude,
                                    Longitude = SC.Latitude
                                };
                            }
                            else
                            {
                                if (dal.GetAllBaseStations().ToList().Count != 0)
                                {
                                    DO.BaseStation randomBS = dal.GetAllBaseStations().ToList()[r.Next(dal.GetAllBaseStations().ToList().Count)];
                                    newD.CurrentLocation = new Location()
                                    {
                                        Latitude = randomBS.Latitude,
                                        Longitude = randomBS.Longitude
                                    };

                                    newD.Battery = 100;
                                }
                                else
                                {
                                    newD.Battery = 100;
                                    newD.CurrentLocation = new Location()
                                    {
                                        Latitude = r.Next(50),
                                        Longitude = r.Next(50)
                                    };
                                }
                            }

                        }
                        break;
                    case 1:
                        {
                            newD.Status = DroneStatuses.Maintenance;
                            sendToMaitenance(newD);
                            newD.Battery = r.Next(20);
                        }
                        break;
                }

                BLDrones.Add(newD);


            }




        }

         public static BL GetInstance()
        {
         
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new BL();
                    }
                }
            }
            return instance;
        }

        private void sendToMaitenance(DroneToList newD)
        {

            List<BaseStation> availibleStation = new List<BaseStation>();

            foreach (DO.BaseStation baseStation in dal.GetAllBaseStations())
            {
                if (getAvailibleSlotsForBaseStation(baseStation) != 0)
                {
                    availibleStation.Add(new BaseStation()
                    {
                        Id = baseStation.Id
                    });
                }
            }

            Random r = new Random();
            BaseStation bs = GetBaseStation(availibleStation[r.Next(availibleStation.Count)].Id);
            newD.CurrentLocation = new Location()
            {
                Latitude = bs.StationLocation.Latitude,
                Longitude = bs.StationLocation.Longitude
            };
            dal.AddDroneCharge(newD.Id, bs.Id);

        }
       
        private int getAvailibleSlotsForBaseStation(BaseStation baseStation)
        {
            int ACS = baseStation.ChargeSlots;
            foreach (DO.DroneCharge droneCharge in dal.GetAllDroneCharges())
            {
                if (droneCharge.BaseStationId == baseStation.Id)
                    ACS -= 1;
            }
            return ACS;
        } //returns amount of availible slots in station

        private int getAvailibleSlotsForBaseStation(DO.BaseStation baseStation)
        {
            int ACS = baseStation.ChargeSlots;
            foreach (DO.DroneCharge droneCharge in dal.GetAllDroneCharges())
            {
                if (droneCharge.BaseStationId == baseStation.Id)
                    ACS -= 1;
            }
            return ACS;
        } //returns amount of availible slots in station

        private BaseStation closestBaseStation(double longitude, double latitude)
        {
            //need to throw exception if there are no BaseStation
            DO.BaseStation closest = dal.GetAllBaseStations().ToList()[0];
            double Mindistance = getDistanceFromLatLonIncoords(latitude, longitude, closest.Latitude, closest.Longitude);
            double currentDis;
            foreach (DO.BaseStation bs in dal.GetAllBaseStations())
            {
                currentDis = getDistanceFromLatLonIncoords(latitude, longitude, bs.Latitude, bs.Longitude);
                if (currentDis < Mindistance)
                {
                    Mindistance = currentDis;
                    closest = bs;
                }
            }

            return new BaseStation()
            {
                Id = closest.Id,
                StationLocation = new Location()
                {
                    Latitude = closest.Latitude,
                    Longitude = closest.Longitude
                },
                Name = closest.Name,
                ChargeSlots = closest.ChargeSlots
            };
        }

        private double getDistanceFromLatLonIncoords(double lat1, double lon1, double lat2, double lon2)
        {
            return Math.Sqrt(Math.Pow(lat1 - lat2, 2) + Math.Pow(lon1 - lon2, 2));

        }

        private double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = deg2rad(lon2 - lon1);
            double a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

        private double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        #region Base Stations

        public void AddBaseStations(int Id, string Name, Location StationLocation, int ChargeSlots)
        {
            if (Id < 0)
            {
                throw new NegetiveNumberException(Id, "Base Station number cannot be negetive!");
            }
            try
            {
                dal.AddBaseStations(Id, Name, StationLocation.Longitude, StationLocation.Latitude, ChargeSlots);
            }
            catch (DO.ItemAlreadyExistsException ex)
            {
                throw;
            }

        }

        public IEnumerable<BaseStationToList> GetAllBaseStations()
        {
            return (from DO.BaseStation bs in dal.GetAllBaseStations()
                    let ACS = getAvailibleSlotsForBaseStation(bs)
                    select new BaseStationToList()
                    {
                        Id = bs.Id,
                        Name = bs.Name,
                        ChargeSlotsAvailible = ACS,
                        ChargeSlotsTaken = bs.ChargeSlots - ACS
                    }).OrderBy(BSTL => BSTL.Id);
           
        }

        public IEnumerable<BaseStationToList> GetAllBaseStationsBy(Predicate<BaseStation> predicate)
        {
            return from DO.BaseStation bs in dal.GetAllBaseStations()
                   where predicate(GetBaseStation(bs.Id))
                   let ACS = getAvailibleSlotsForBaseStation(bs)
                   select new BaseStationToList()
                   {
                       Id = bs.Id,
                       Name = bs.Name,
                       ChargeSlotsAvailible = ACS,
                       ChargeSlotsTaken = bs.ChargeSlots - ACS
                   };

        }

        public BaseStation GetBaseStation(int Id)
        {
            DO.BaseStation bs;
            try
            {
                bs = dal.GetBaseStation(Id);
                return new BaseStation()
                {
                    Id = bs.Id,
                    StationLocation = new Location()
                    {
                        Latitude = bs.Latitude,
                        Longitude = bs.Longitude
                    },
                    ChargeSlots = bs.ChargeSlots,
                    Name = bs.Name,
                    DroneInChargesList = GetDronesInBaseStation(bs).ToList()

                };
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw;
            }
        }

        public void UpdateBaseStation(int Id, string Name, int? ChargeSlots)
        {
            DO.BaseStation lastBS;
            try
            {
                lastBS = dal.GetBaseStation(Id);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Base Station with specified ID was not found", ex);
            }

            if (ChargeSlots < 1)
            {
                throw new NegetiveNumberException((int)ChargeSlots, "Charge Slots number cannot be negetive!");
            }

            if (GetDronesInBaseStation(lastBS).Count() > ChargeSlots)
            {
                throw new OutOfRangeException((int)ChargeSlots, "Too many drones in base station num: " + lastBS.Id);
            }

            if (Name != "")
            {
                lastBS.Name = Name;
            }
            if (ChargeSlots != null)
            {
                lastBS.ChargeSlots = (int)ChargeSlots;
            }

            dal.SetBaseStation(lastBS);
        }

        private IEnumerable<DroneInCharge> GetDronesInBaseStation(DO.BaseStation bs)
        {
            return from DO.DroneCharge DC in dal.GetAllDroneCharges()
                   where DC.BaseStationId == bs.Id
                   select new DroneInCharge()
                   {
                       Battery = GetDrone(DC.DroneId).Battery,
                       Id = DC.DroneId
                   };
        }

        public IEnumerable<BaseStationToList> GetAvailibleBaseStations()
        {
            return from DO.BaseStation bs in dal.GetAllBaseStations()
                   let ACS = getAvailibleSlotsForBaseStation(bs)
                   where ACS > 0
                   select new BaseStationToList()
                   {
                       Id = bs.Id,
                       Name = bs.Name,
                       ChargeSlotsAvailible = ACS,
                       ChargeSlotsTaken = bs.ChargeSlots - ACS
                   };

        }

        #endregion

        #region Drones 

        public void AddDrone(int Id, String Model, WeightCategories MaxWeight, int stationId)
        {
            if (Id < 1)
            {
                throw new NegetiveNumberException(Id, "Drone Id can not be negative number or zero");
            }
            if (stationId < 1)
            {
                throw new NegetiveNumberException(Id, "Base Station Id can not be negative number or zero");
            }
            if ((int)MaxWeight < 0 || (int)MaxWeight > 2)
            {
                throw new OutOfRangeException(Id, "Maxweight has to be 1 or 2 or 3 only");
            }
            try
            {
                Random r = new Random();
                DO.BaseStation bs = dal.GetBaseStation(stationId);
                dal.AddDrone(Id, Model, (DO.WeightCategories)MaxWeight);
                BLDrones.Add(new DroneToList()
                {
                    Id = Id,
                    Battery = r.Next(20, 41),
                    MaxWeight = MaxWeight,
                    Model = Model,
                    Status = DroneStatuses.Maintenance,
                    CurrentLocation = new Location()
                    {
                        Latitude = bs.Latitude,
                        Longitude = bs.Longitude
                    },
                    CarriedParcelId = null
                });
                dal.AddDroneCharge(Id, stationId);
            }
            catch (DO.ItemNotFoundException ex) //base station not found
            {
                throw;
            }
            catch (DO.ItemAlreadyExistsException ex)
            {
                throw;
            }
            catch
            {
                throw;
            }
        } //need to fix

        public IEnumerable<DroneToList> GetAllDrones()
        {
            BLDrones.Sort();
            //return BLDrones;
            return BLDrones;
        }

        public IEnumerable<DroneToList> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return from DroneToList d in GetAllDrones()
                   let drone = GetDrone(d.Id)
                   where predicate(drone)
                   select d;
        }

        public void UpdateDrone(int Id, string Model)
        {
            DO.Drone lastDrone;

            try
            {
                lastDrone = dal.GetDrone(Id);

            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Drone with specified ID was not found", ex);
            }
            lastDrone.Model = Model;
            dal.SetDrone(lastDrone);
            DroneToList BLdrone = BLDrones.FirstOrDefault(d => d.Id == Id);
            BLDrones.Remove(BLdrone);
            BLdrone.Model = Model;
            BLDrones.Add(BLdrone);
        }

        public Drone GetDrone(int Id)
        {
            DroneToList d = BLDrones.FirstOrDefault(p => p.Id == Id);
            if (d == null)
            {
                throw new DO.ItemNotFoundException(Id, "Drone Not Found!");
            }
            if (d.CarriedParcelId == null)
            {
                return new Drone()
                {
                    Id = d.Id,
                    Battery = d.Battery,
                    MaxWeight = d.MaxWeight,
                    Model = d.Model,
                    Status = d.Status,
                    CurrentLocation = d.CurrentLocation
                };
            }

            Parcel p = GetParcel((int)d.CarriedParcelId);
            Customer sender = GetCustomer(p.Sender.Id);
            Customer target = GetCustomer(p.Target.Id);
            return new Drone()
            {
                Id = d.Id,
                Battery = d.Battery,
                CurrentParcel = new ParcelInDelivery()
                {
                    Id = p.Id,
                    ParcelStatus = ((p.PickedUp != null) ? true : false),
                    PickUp = sender.Address,
                    Drop = target.Address,
                    DeliveryDistance = getDistanceFromLatLonInKm(sender.Address.Latitude, sender.Address.Longitude, target.Address.Latitude, target.Address.Longitude),
                    priority = (Priorities)p.Priority,
                    Sender = new CustomerInParcel()
                    {
                        Id = sender.Id,
                        Name = sender.Name
                    },
                    Target = new CustomerInParcel() { Id = target.Id, Name = target.Name }
                },
                MaxWeight = d.MaxWeight,
                Model = d.Model,
                Status = d.Status,
                CurrentLocation = d.CurrentLocation
            };
        }

        public void DeleteDrone(int Id)
        {
            try
            {
                dal.RemoveDrone(Id);
                BLDrones.RemoveAll(d => d.Id == Id);
            }
            catch
            {
                throw;
            }

        }

        public void ChargeDrone(int Id)
        {
            Drone droneToCharge;
            try
            {
                droneToCharge = GetDrone(Id);
            }
            catch (ItemNotFoundException ex)
            {
                throw;
            }

            if (droneToCharge.Status != DroneStatuses.Availible)
            {
                throw new DroneIsntAvailibleException(Id);
            }

            BaseStation BS = closestBaseStation(droneToCharge.CurrentLocation.Longitude, droneToCharge.CurrentLocation.Latitude);

            if (BS.DroneInChargesList.Count == BS.ChargeSlots)
            {
                throw new BaseStationFullException(Id, BS.Id);
            }

            double distanceToBS = getDistanceFromLatLonInKm(droneToCharge.CurrentLocation.Latitude, droneToCharge.CurrentLocation.Longitude, BS.StationLocation.Latitude, BS.StationLocation.Longitude);

            if (droneToCharge.Battery < distanceToBS / AvailbleElec)
            {
                throw new NotEnoughDroneBatteryException(Id);
            }

            DroneToList dToUpdate = BLDrones.FirstOrDefault(d => d.Id == Id);
            BLDrones.Remove(dToUpdate);
            dToUpdate.Status = DroneStatuses.Maintenance;
            BLDrones.Add(dToUpdate);
            dal.AddDroneCharge(Id, BS.Id);

        }

        public void DisChargeDrone(int Id, float time)
        {
            Drone droneDisCharge;
            try
            {
                droneDisCharge = GetDrone(Id);
            }
            catch (ItemNotFoundException ex)
            {
                throw;
            }

            if (droneDisCharge.Status != DroneStatuses.Maintenance)
            {
                throw new StatusIsntMaintance(Id);
            }
            double[] a = dal.AskForElectricity();


            DroneToList dToUpdate = BLDrones.FirstOrDefault(d => d.Id == Id);
            BLDrones.Remove(dToUpdate);
            dToUpdate.Status = DroneStatuses.Availible;
            dToUpdate.Battery += a[4] * (time / 60);   // change to time base 60 minutes
            if (dToUpdate.Battery > 100)
            {
                dToUpdate.Battery = 100;
            }
            BLDrones.Add(dToUpdate);
            dal.RemoveDroneCharge(Id);



        }

        #endregion

        #region Customers

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude)
        {
            if (Id < 1)
            {
                throw new BO.NegetiveNumberException(Id, "Id can not be negative number or zero");
            }
            if (Phone.Length != 10)
            {
                throw new InvalidNumberLengthException(Phone.Length);
            }
            try
            {
                dal.AddCustomer(Id, Name, Phone, Longitude, Latitude);
            }
            catch (DO.ItemAlreadyExistsException ex)
            {
                throw new ItemAlreadyExistsException(Id, ex.Message, ex);
            }
        } //need to fix

        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            return (from DO.Customer c in dal.GetAllCustomers()
                                        select new CustomerToList()
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            Phone = c.Phone,
                                            SentAndDelivered = getSentAndDelivered(c),
                                            SentAndNotDelivered = getSentAndNotDelivered(c),
                                            OnTheWay = getOnTheWay(c),
                                            Recieved = getRecieved(c)
                                            //need to create private funcs for others params
                                        }).OrderBy(CTL => CTL.Id);
            
        }

        public Customer GetCustomer(int Id)
        {
            try
            {
                DO.Customer c = dal.GetCustomer(Id);

                return new Customer()
                {
                    Id = c.Id,
                    Address = new Location()
                    {
                        Latitude = c.Latitude,
                        Longitude = c.Longitude
                    },
                    Name = c.Name,
                    Phone = c.Phone,
                    FromThisCustomer = getFromeThisCustomer(c),
                    ToThisCustomer = getToThisCustomer(c),
                };
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw;
            }

        }

        private int getRecieved(DO.Customer c)
        {
            int sum = 0;
            foreach (DO.Parcel p in dal.GetAllParcels())
            {
                if (p.TargetId == c.Id && p.Delivered != null && p.PickedUp != null)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getOnTheWay(DO.Customer c)
        {
            int sum = 0;
            foreach (DO.Parcel p in dal.GetAllParcels())
            {
                if (p.TargetId == c.Id && p.Delivered == null && p.PickedUp != null)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getSentAndNotDelivered(DO.Customer c)
        {
            int sum = 0;
            foreach (DO.Parcel p in dal.GetAllParcels())
            {
                if (p.SenderId == c.Id && p.Delivered == null && p.PickedUp != null)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getSentAndDelivered(DO.Customer c)
        {
            int sum = 0;
            foreach (DO.Parcel p in dal.GetAllParcels())
            {
                if (p.SenderId == c.Id && p.Delivered != null && p.PickedUp != null)
                {
                    sum++;
                }
            }
            return sum;
        }

        public void UpdateCustomer(int Id, string name, string phone)
        {
            DO.Customer lastCustomer;

            try
            {
                lastCustomer = dal.GetCustomer(Id);

            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Customer with specified ID was not found", ex);
            }

            if (name != "")
            {
                lastCustomer.Name = name;
            }

            if (phone != "")
            {
                lastCustomer.Phone = phone;
            }

            dal.SetCustomer(lastCustomer);

        }

        public void DeleteCustomer(int Id)
        {
            try
            {
                dal.RemoveCustomer(Id);
            }
            catch
            {
                throw;
            }
        }

        private List<ParcelInCustomer> getToThisCustomer(DO.Customer c)
        {
            return (from DO.Parcel p in dal.GetAllParcels()
                    where p.TargetId == c.Id
                    let sender = dal.GetCustomer(p.SenderId)
                    select new ParcelInCustomer()
                    {
                        Id = p.Id,

                        Priority = (Priorities)p.Priority,
                        Weight = (WeightCategories)p.Weight,
                        Status = getParcelStatus(p),
                        OtherSide = new CustomerInParcel()
                        {
                            Id = sender.Id,
                            Name = sender.Name
                        }
                    }).ToList();
        }

        private List<ParcelInCustomer> getFromeThisCustomer(DO.Customer c)
        {
            return (from DO.Parcel p in dal.GetAllParcels()
                    where p.SenderId == c.Id
                    let target = dal.GetCustomer(p.TargetId)
                    select new ParcelInCustomer()
                    {
                        Id = p.Id,

                        Priority = (Priorities)p.Priority,
                        Weight = (WeightCategories)p.Weight,
                        Status = getParcelStatus(p),
                        OtherSide = new CustomerInParcel()
                        {
                            Id = target.Id,
                            Name = target.Name
                        }
                    }).ToList();
        }

        #endregion

        #region Parcel

        public IEnumerable<ParcelToList> GetAllParcels()
        {
            return (from DO.Parcel p in dal.GetAllParcels()
                                      select new ParcelToList()
                                      {
                                          Id = p.Id,
                                          Priority = (Priorities)p.Priority,
                                          Weight = (WeightCategories)p.Weight,
                                          Status = getParcelStatus(p),
                                          SenderName = dal.GetCustomer(p.SenderId).Name,
                                          TargetName = dal.GetCustomer(p.TargetId).Name,
                                      }).OrderBy(PTL => PTL.Id);
           
        }

        public void AddParcel(int SenderId, int TargetId, DO.WeightCategories PackageWight, DO.Priorities priority)
        {
            try
            {
                dal.GetCustomer(SenderId);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Sender could not be found!", ex);
            }
            try
            {
                dal.GetCustomer(SenderId);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Sender could not be found!", ex);
            }
            if ((int)PackageWight < 0 || (int)PackageWight > 2)
            {
                throw new OutOfRangeException((int)PackageWight, "Package Weight has to be between 1 to 3 only");
            }
            if ((int)priority < 0 || (int)priority > 2)
            {
                throw new OutOfRangeException((int)priority, "Priority has to be between 1 to 3 only");
            }
            DateTime created = DateTime.Now;
            try
            {
                dal.AddParcel(runningNumForParcels, SenderId, TargetId, PackageWight, priority, created);
                runningNumForParcels++;
            }
            catch (DO.ItemAlreadyExistsException ex)
            {
                throw;
            }

        }

        public Parcel GetParcel(int Id)
        {
            try
            {
                DO.Parcel p = dal.GetParcel(Id);
                Customer sender = GetCustomer(p.SenderId);
                Customer target = GetCustomer(p.TargetId);
                return new Parcel()
                {
                    Id = p.Id,
                    DroneId = p.DroneId,
                    Priority = p.Priority,
                    Sender = new CustomerInParcel() { Id = sender.Id, Name = sender.Name },
                    Target = new CustomerInParcel() { Id = target.Id, Name = target.Name },
                    Weight = p.Weight,
                    Requested = p.Requested,
                    scheduled = p.scheduled,
                    PickedUp = p.PickedUp,
                    Delivered = p.Delivered
                };
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw;
            }


        }

        private ParcelStatuses getParcelStatus(DO.Parcel p)
        {
            if (p.Delivered != null)
            {
                return ParcelStatuses.Delivered;
            }
            else if (p.PickedUp != null)
            {
                return ParcelStatuses.PickedUp;
            }
            else if (p.scheduled != null)
            {
                return ParcelStatuses.Associated;
            }
            else
            {
                return ParcelStatuses.Created;
            }
        }

        public IEnumerable<ParcelToList> GetParcelsWithNoDrone()
        {
            List<ParcelToList> PTL = (from DO.Parcel p in dal.GetAllParcels()
                                      where p.DroneId == 0
                                      select new ParcelToList()
                                      {
                                          Id = p.Id,
                                          Priority = (Priorities)p.Priority,
                                          Weight = (WeightCategories)p.Weight,
                                          Status = getParcelStatus(p),
                                          SenderName = dal.GetCustomer(p.SenderId).Name,
                                          TargetName = dal.GetCustomer(p.TargetId).Name,
                                      }).ToList();
            PTL.Sort();
            return PTL;
        }

        public void DeleteParcel(int Id)
        {
            try
            {
                Parcel p = GetParcel(Id);
                dal.RemoveParcel(Id);
                if(p.DroneId != 0)
                {
                    DroneToList tempDrone = BLDrones.First(d => d.Id == p.DroneId);
                    if(tempDrone.Status == DroneStatuses.InDelivery)
                    {
                        BLDrones.Remove(tempDrone);
                        tempDrone.Status = DroneStatuses.Availible;
                        tempDrone.CarriedParcelId = null;
                        BLDrones.Add(tempDrone);
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        public void AttributionParcelToDrone(int id)
        {

            DO.Drone drone = dal.GetDrone(id);
            Drone d = GetDrone(id);
            if (d.Status != DroneStatuses.Availible)
            {
                throw new DroneIsBusy(id);
            }

            bool flag = false;
            DO.Parcel p1 = dal.GetAllParcels().First(); //p1 is variable (parcel) we gonna check to connect


            foreach (DO.Parcel p2 in dal.GetAllParcels())
            {
                if (((int)p2.Weight > (int)drone.MaxWeight) || p2.scheduled != null)
                {
                    continue;
                }
                Customer c1 = GetCustomer(p1.SenderId);
                Customer c2 = GetCustomer(p2.SenderId);
                Customer Target = GetCustomer(p1.TargetId);

                //distance drone to parcel p1
                double distanceP1 = getDistanceFromLatLonInKm(d.CurrentLocation.Latitude, d.CurrentLocation.Longitude, c1.Address.Latitude, c1.Address.Longitude);
                //distance drone to parcel p2
                double distanceP2 = getDistanceFromLatLonInKm(d.CurrentLocation.Latitude, d.CurrentLocation.Longitude, c2.Address.Latitude, c2.Address.Longitude);
                //distance delivery
                double distanceDelivery = getDistanceFromLatLonInKm(Target.Address.Latitude, Target.Address.Longitude, c2.Address.Latitude, c1.Address.Longitude);
                //distance to base station
                BaseStation BS = closestBaseStation(Target.Address.Longitude, Target.Address.Latitude);
                double distanceToBs = getDistanceFromLatLonInKm(Target.Address.Latitude, Target.Address.Longitude, BS.StationLocation.Latitude, BS.StationLocation.Longitude);



                double[] arr = dal.AskForElectricity();
                double ELecInDelivery = arr[(int)p1.Weight + 1];

                if (d.Battery < distanceP2 / AvailbleElec + distanceDelivery / ELecInDelivery + distanceToBs / AvailbleElec)
                {
                    continue;
                }
                if(p1.DroneId != 0)
                {
                    continue;
                }
                if (d.Battery < distanceP1 / AvailbleElec + distanceDelivery / ELecInDelivery + distanceToBs / AvailbleElec)
                {
                    flag = true;
                    p1 = p2;
                    continue;
                }
                if ((int)p2.Priority > (int)p1.Priority)
                {
                    flag = true;
                    p1 = p2;
                    continue;
                }
                if ((int)p2.Priority < (int)p1.Priority)
                {
                    continue;
                }
                if ((int)p2.Weight > (int)p1.Priority)
                {
                    flag = true;
                    p1 = p2;
                    continue;
                }
                if ((int)p2.Weight < (int)p1.Priority)
                {
                    continue;
                }

                if (distanceP2 < distanceP1)
                {
                    flag = true;
                    p1 = p2;
                    continue;
                }
                flag = true;



            }
            if (flag) // to meke condition if didnt finnd parcels at all
            {


                DroneToList BLdrone = BLDrones.FirstOrDefault(d => d.Id == id);
                BLDrones.Remove(BLdrone);
                BLdrone.Status = (DroneStatuses)2;
                BLdrone.CarriedParcelId = p1.Id;
                BLDrones.Add(BLdrone);

                p1.DroneId = id;  // NEED TO CHANGE IN BO INT DRONEID-->> DroneInParcel drone
                p1.scheduled = DateTime.Now;
                try { dal.SetParcel(p1); }
                catch { throw; }

                Console.WriteLine("drone id :" + id + " takes parcel number :" + p1.Id);
                return;

            }
            throw new NoParcelForThisDrone(id);


        }

        public void PickUpParcelByDrone(int Id)
        {
            try
            {
                Drone drone = GetDrone(Id);
                if (drone.Status != DroneStatuses.InDelivery)
                {
                    throw new StatusIsntInDelivery(Id);
                }
                int parcelId = drone.CurrentParcel.Id;
                DO.Parcel parcel = dal.GetParcel(parcelId);
                if (parcel.PickedUp != null)
                {
                    throw new ParcelAlreadyPickedUp(parcelId);
                }
                double[] arr = dal.AskForElectricity();
                double ELecInDelivery = arr[(int)parcel.Weight + 1];
                Customer c = GetCustomer(parcel.SenderId);
                double distance = getDistanceFromLatLonInKm(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, c.Address.Latitude, c.Address.Longitude);


                DroneToList BLdrone = BLDrones.FirstOrDefault(d => d.Id == Id);
                BLDrones.Remove(BLdrone);
                BLdrone.Battery -= distance / ELecInDelivery;
                BLdrone.CurrentLocation = c.Address;
                BLDrones.Add(BLdrone);

                parcel.PickedUp = DateTime.Now;
                dal.SetParcel(parcel);
            }
            catch { throw; }


        }

        public void SupplyParcel(int Id)
        {
            try
            {
                Drone drone = GetDrone(Id);
                if (drone.Status != DroneStatuses.InDelivery)
                {
                    throw new StatusIsntInDelivery(Id);
                }
                int parcelId = drone.CurrentParcel.Id;
                DO.Parcel parcel = dal.GetParcel(parcelId);
                if (parcel.Delivered != null)
                {
                    throw new ParcelAlreadySupply(parcelId);
                }




                double[] arr = dal.AskForElectricity();
                double ELecInDelivery = arr[(int)parcel.Weight + 1];
                Customer c = GetCustomer(parcel.TargetId);
                double distance = getDistanceFromLatLonInKm(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, c.Address.Latitude, c.Address.Longitude);


                DroneToList BLdrone = BLDrones.FirstOrDefault(d => d.Id == Id);
                BLDrones.Remove(BLdrone);
                BLdrone.Battery -= distance / ELecInDelivery;
                BLdrone.CurrentLocation = c.Address;
                BLdrone.Status = DroneStatuses.Availible;
                BLdrone.CarriedParcelId = null;
                BLDrones.Add(BLdrone);

                parcel.Delivered = DateTime.Now;
                dal.SetParcel(parcel);
            }
            catch { throw; }

        }




        #endregion
    }
}
