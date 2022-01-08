using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using DalApi;
using System.Runtime.CompilerServices;

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

        #region single tone

        internal static BL instance = null;
        private static object locker = new object();
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

        #endregion

        private BL(int a)
        {
            dal = DalFactory.GetDal("XML");
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
                    DroneToList newD = new DroneToList();
                    try
                    {
                        DO.DroneCharge droneCharge = dal.GetDroneCharge(PDrone.Id);
                        DO.BaseStation baseStation = dal.GetBaseStation(droneCharge.BaseStationId);
                        newD.CurrentLocation = new Location()
                        {
                            Latitude = baseStation.Latitude,
                            Longitude = baseStation.Longitude
                        };
                        newD.Status = DroneStatuses.Maintenance;
                        newD.Battery = r.Next(20);
                        newD.CarriedParcelId = p.Id;
                        BLDrones.Add(newD);
                        continue;
                    }
                    catch { }

                    if (p.Delivered == null)
                    {


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
                try
                {
                    DO.DroneCharge droneCharge = dal.GetDroneCharge(d.Id);
                    DO.BaseStation baseStation = dal.GetBaseStation(droneCharge.BaseStationId);
                    newD.CurrentLocation = new Location()
                    {
                        Latitude = baseStation.Latitude,
                        Longitude = baseStation.Longitude
                    };
                    newD.Status = DroneStatuses.Maintenance;
                    newD.Battery = r.Next(20);

                    BLDrones.Add(newD);
                    continue;
                }
                catch
                {

                }


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

        private BL()
        {
            dal = DalFactory.GetDal("XML");
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
            foreach (DO.Drone d in dal.GetAllDrones())
            {
                DroneToList newD = new DroneToList()
                {
                    Id = d.Id,
                    MaxWeight = (WeightCategories)d.MaxWeight,
                    Model = d.Model
                };
                try
                {

                    DO.BaseStation chargedIn = dal.GetBaseStation(dal.GetDroneCharge(d.Id).BaseStationId);
                    newD.Status = DroneStatuses.Maintenance;
                    newD.CurrentLocation = new Location()
                    {

                        Longitude = chargedIn.Longitude,
                        Latitude = chargedIn.Latitude

                    };
                    newD.Battery = r.Next(20);
                    BLDrones.Add(newD);
                    continue;
                }
                catch
                {

                }
                DO.Parcel p;
                try
                {
                    p = dal.GetAllParcelsBy(p => (p.DroneId == d.Id && p.Delivered is null)).First();
                    newD.Status = DroneStatuses.InDelivery;
                    DO.Customer sender = dal.GetCustomer(p.SenderId);
                    Location senderL = new Location() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                    DO.Customer target = dal.GetCustomer(p.SenderId);
                    Location targetL = new Location() { Latitude = target.Latitude, Longitude = target.Longitude };
                    BaseStation senderBS = closestBaseStation(sender.Longitude, sender.Latitude);
                    BaseStation targetBS = closestBaseStation(target.Longitude, target.Latitude);
                    newD.Battery = getDistanceInBattery(senderL, targetL, p.Weight);
                    newD.Battery += getDistanceInBattery(targetL, targetBS.StationLocation);
                    if (p.PickedUp is not null)
                    {
                        newD.CurrentLocation = new Location()
                        {
                            Longitude = sender.Longitude,
                            Latitude = sender.Latitude
                        };
                    }
                    else
                    {
                        newD.CurrentLocation = senderBS.StationLocation;
                        newD.Battery += getDistanceInBattery(senderBS.StationLocation, senderL);
                    }
                    if (newD.Battery > 100) newD.Battery = 100;
                    else newD.Battery = r.Next((int)newD.Battery + 1, 101);
                    BLDrones.Add(newD);
                    continue;
                    //case of attributed parcel
                }
                catch
                {
                    
                }
                try
                {
                    p = dal.GetAllParcelsBy(p => (p.DroneId == d.Id)).OrderBy(d => d.Delivered).First();
                    newD.Status = DroneStatuses.Availible;
                    DO.Customer target = dal.GetCustomer(p.TargetId);
                    BaseStation targetBS = closestBaseStation(target.Longitude, target.Latitude);
                    newD.CurrentLocation = new Location() { Latitude = target.Latitude, Longitude = target.Longitude };
                    newD.Battery = r.Next((int)getDistanceInBattery(newD.CurrentLocation, targetBS.StationLocation) + 1, 101);
                    BLDrones.Add(newD);
                    continue;
                    //case of satisfied customers
                }
                catch
                {
                    //case of no attributed Parcel
                    newD.Status = DroneStatuses.Maintenance;
                    DO.BaseStation baseStation = dal.GetBaseStation(sendToMaitenance(newD));
                    newD.CurrentLocation = new Location() { Longitude = baseStation.Longitude, Latitude = baseStation.Latitude };
                    newD.Battery = r.Next(20);
                    BLDrones.Add(newD);
                    continue;
                }

            }

        }






        private int sendToMaitenance(DroneToList newD)
        {

            IEnumerable<BaseStation> availibleStation = from DO.BaseStation baseStation in dal.GetAllBaseStations()
                                                 where getAvailibleSlotsForBaseStation(baseStation) > 0
                                                 select new BaseStation()
                                                 {
                                                     Id = baseStation.Id
                                                 };

            Random r = new Random();
            BaseStation bs = GetBaseStation(availibleStation.ToArray()[r.Next(availibleStation.Count())].Id);
            dal.AddDroneCharge(newD.Id, bs.Id, DateTime.Now);
            return bs.Id;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                    DroneInChargesList = GetDronesInBaseStation(bs.Id).ToList()

                };
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

            if (GetDronesInBaseStation(lastBS.Id).Count() > ChargeSlots)
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

        private IEnumerable<DroneInCharge> GetDronesInBaseStation(int BSId)
        {
            return from DO.DroneCharge DC in dal.GetAllDroneCharges()
                   where DC.BaseStationId == BSId
                   select new DroneInCharge()
                   {
                       Battery = GetDrone(DC.DroneId).Battery,
                       Id = DC.DroneId
                   };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStationToList TurnBaseStationToList(BaseStation Bs)
        {
            int available = getAvailibleSlotsForBaseStation(Bs);
            return new BaseStationToList()
            {
                Id = Bs.Id,
                Name = Bs.Name,
                ChargeSlotsAvailible = available,
                ChargeSlotsTaken = Bs.ChargeSlots - available
            };
        }

        #endregion

        #region Drones 

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                if (getAvailibleSlotsForBaseStation(GetBaseStation(bs.Id)) <= 0)
                {
                    throw new Exception("No Availible slot in Base Station num: " + bs.Id);
                }
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
                dal.AddDroneCharge(Id, stationId, DateTime.Now);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDrones()
        {
            BLDrones.Sort();
            //return BLDrones;
            return BLDrones;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return from DroneToList d in GetAllDrones()
                   let drone = GetDrone(d.Id)
                   where predicate(drone)
                   select d;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            try
            {
                dal.RemoveDroneCharge(Id);
            }
            catch
            {

            }


        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneToList TurnDroneToList(Drone drone)
        {
            return new DroneToList()
            {
                Id = drone.Id,
                Battery = drone.Battery,
                Status = drone.Status,
                CurrentLocation = drone.CurrentLocation,
                CarriedParcelId = drone.CurrentParcel.Id,
                MaxWeight = drone.MaxWeight,
                Model = drone.Model
            };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            dal.AddDroneCharge(Id, BS.Id, DateTime.Now);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            List<CustomerToList> res = new List<CustomerToList>();

            foreach (DO.Customer c in dal.GetAllCustomers())
            {
                Customer BOc;
                try
                {
                    BOc = GetCustomer(c.Id);
                }
                catch (Exception ex)
                {
                    throw;
                }
                CustomerToList newC = new CustomerToList();
                foreach (ParcelInCustomer PIC in BOc.ToThisCustomer)
                {
                    if (PIC.Status == ParcelStatuses.Delivered)
                    {
                        newC.Recieved += 1;
                    }
                    else
                    {
                        newC.OnTheWay += 1;
                    }
                }
                foreach (ParcelInCustomer PIC in BOc.FromThisCustomer)
                {
                    if (PIC.Status == ParcelStatuses.Delivered)
                    {
                        newC.SentAndDelivered += 1;
                    }
                    else
                    {
                        newC.SentAndNotDelivered += 1;
                    }
                }
                newC.Id = c.Id;
                newC.Name = c.Name;
                newC.Phone = c.Phone;
                res.Add(newC);
            }
            return res.OrderBy(CTL => CTL.Id);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerToList TurnCustomerToList(Customer customer)
        {
            DO.Customer c = dal.GetCustomer(customer.Id);
            return new CustomerToList()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                SentAndDelivered = getSentAndDelivered(c),
                SentAndNotDelivered = getSentAndNotDelivered(c),
                Recieved = getRecieved(c),
                OnTheWay = getOnTheWay(c),

            };
        }

        #endregion

        #region Parcel

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                        TargetName = dal.GetCustomer(p.TargetId).Name
                    }).OrderBy(PTL => PTL.Id);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(int SenderId, int TargetId, BO.WeightCategories PackageWight, BO.Priorities priority)
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
                dal.GetCustomer(TargetId);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Target could not be found!", ex);
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
            int res = 0;
            try
            {
                return dal.AddParcel(SenderId, TargetId, (DO.WeightCategories)PackageWight, (DO.Priorities)priority, created);
            }
            catch (DO.ItemAlreadyExistsException ex)
            {
                throw;
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                    scheduled = p.Scheduled,
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            else if (p.Scheduled != null)
            {
                return ParcelStatuses.Associated;
            }
            else
            {
                return ParcelStatuses.Created;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private ParcelStatuses getParcelStatus(Parcel p)
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int Id)
        {
            try
            {
                Parcel p = GetParcel(Id);
                dal.RemoveParcel(Id);
                if (p.DroneId != 0)
                {
                    DroneToList tempDrone = BLDrones.First(d => d.Id == p.DroneId);
                    if (tempDrone.Status == DroneStatuses.InDelivery)
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelToList TurnParcelToList(Parcel parcel)
        {
            return new ParcelToList()
            {
                Id = parcel.Id,
                Priority = (Priorities)parcel.Priority,
                Weight = (WeightCategories)parcel.Weight,
                Status = getParcelStatus(parcel),
                SenderName = dal.GetCustomer(parcel.Sender.Id).Name,
                TargetName = dal.GetCustomer(parcel.Target.Id).Name
            };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AttributionParcelToDroneTemp(int id)
        {
            DO.Drone drone = dal.GetDrone(id);
            Drone d = GetDrone(id);
            if (d.Status != DroneStatuses.Availible)
            {
                throw new DroneIsBusy(id);
            }

            bool flag = false;
            DO.Parcel p1;
            try
            {
                p1 = dal.GetAllParcels().First(); //p1 is variable (parcel) we gonna check to connect
            }
            catch
            {
                throw new NoParcelForThisDrone(id);
            }


            foreach (DO.Parcel p2 in dal.GetAllParcels())
            {
                if (((int)p2.Weight > (int)drone.MaxWeight) || p2.Scheduled != null)
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
                if (p1.DroneId != 0)
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
            if (flag) // to meke condition if didnt find parcels at all
            {


                DroneToList BLdrone = BLDrones.FirstOrDefault(d => d.Id == id);
                BLDrones.Remove(BLdrone);
                BLdrone.Status = (DroneStatuses)2;
                BLdrone.CarriedParcelId = p1.Id;
                BLDrones.Add(BLdrone);

                p1.DroneId = id;  // NEED TO CHANGE IN BO INT DRONEID-->> DroneInParcel drone
                p1.Scheduled = DateTime.Now;
                try { dal.SetParcel(p1); }
                catch { throw; }

                return;

            }
            throw new NoParcelForThisDrone(id);


        }

        public void AttributionParcelToDrone(int Id)
        {
            Drone d = GetDrone(Id);
            if (d.Status != DroneStatuses.Availible)
            {
                throw new NoParcelForThisDrone(Id);
            }

            try
            {
                DO.Parcel parcel = (from DO.Parcel p in dal.GetAllParcelsBy(p => (canSupply(d, p) && p.DroneId == 0))
                                    orderby p.Priority
                                    select p).Last();
                updateAttribution(Id, parcel.Id);
            }
            catch
            {
                throw new NoParcelForThisDrone(Id);
            }

        }
        private void updateAttribution(int DroneId, int ParcelId)
        {
            try
            {
                DO.Parcel p = dal.GetParcel(ParcelId);
                p.DroneId = DroneId;
                p.Scheduled = DateTime.Now;
                dal.SetParcel(p);
                DroneToList d = BLDrones.First(d => d.Id == DroneId);
                BLDrones.Remove(d);
                d.CarriedParcelId = ParcelId;
                d.Status = DroneStatuses.InDelivery;
                BLDrones.Add(d);
            }
            catch
            {
                throw;
            }

        }
        private bool canSupply(Drone drone, DO.Parcel parcel)
        {
            if ((int)drone.MaxWeight < (int)parcel.Weight)
            {
                return false;
            }
            DO.Customer sender = dal.GetCustomer(parcel.SenderId);
            Location senderL = new Location() { Latitude = sender.Latitude, Longitude = sender.Longitude };
            DO.Customer target = dal.GetCustomer(parcel.TargetId);
            Location targetL = new Location() { Latitude = target.Latitude, Longitude = target.Longitude };

            double batteryNeeded = getDistanceInBattery(drone.CurrentLocation, senderL) + getDistanceInBattery(senderL, targetL, parcel.Weight);

            if (drone.Battery >= batteryNeeded)
            {
                return true;
            }
            return false;
        }

        private double getDistanceInBattery(Location from, Location to)
        {
            double disinkm = getDistanceFromLatLonInKm(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
            return disinkm / AvailbleElec;

        }

        private double getDistanceInBattery(double fLatitude, double fLongitude, double tLatitude, double tLongitude, DO.WeightCategories weight)
        {
            double disinkm = getDistanceFromLatLonInKm(fLatitude, fLongitude, tLatitude, tLongitude);
            return disinkm / dal.AskForElectricity()[(int)weight + 1];

        }

        private double getDistanceInBattery(Location from, Location to, DO.WeightCategories weight)
        {
            double disinkm = getDistanceFromLatLonInKm(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
            return disinkm / dal.AskForElectricity()[(int)weight + 1];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickUpParcelByDrone(int Id)
        {
            try
            {
                Drone drone = GetDrone(Id);
                if (drone.Status != DroneStatuses.InDelivery)
                {
                    throw new StatusIsntInDelivery(Id);
                }
                int parcelId = (int)drone.CurrentParcel.Id;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SupplyParcel(int Id)
        {
            try
            {
                Drone drone = GetDrone(Id);
                if (drone.Status != DroneStatuses.InDelivery)
                {
                    throw new StatusIsntInDelivery(Id);
                }
                int parcelId = (int)drone.CurrentParcel.Id;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(int Id, BO.Priorities prior)
        {
            DO.Parcel lastParcel;

            try
            {
                lastParcel = dal.GetParcel(Id);

            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException("Parcel with specified ID was not found", ex);
            }
            lastParcel.Priority = (DO.Priorities)prior;



            dal.SetParcel(lastParcel);

        }

        #endregion
    }
}
