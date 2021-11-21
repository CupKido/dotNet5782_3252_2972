﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLobject
{
    public partial class BL : IBL.IBL
    {

        public IDAL.IDal dal; //with dal we have access to data source
        List<DroneToList> BLDrones = new List<DroneToList>();
        public Double AvailbleElec { get; set; }
        public Double LightElec { get; set; }
        public Double IntermediateElec { get; set; }
        public Double HeavyElec { get; set; }
        public Double ChargePerHours { get; set; }
        private static int runningNumForParcels { get; set; }
        public BL()
        {
            dal = new DalObject.DalObject();

            runningNumForParcels = dal.GetAllParcels().Count() + 1;
            double[] arr = dal.AskForElectricity();
            AvailbleElec = arr[0];
            LightElec = arr[1];
            IntermediateElec = arr[2];
            HeavyElec = arr[3];
            ChargePerHours = arr[4];
            List<IDAL.DO.Customer> SatisfiedCustomers = new List<IDAL.DO.Customer>();
            List<IDAL.DO.Drone> DalDronesList = dal.GetAllDrones().ToList();

            Random r = new Random();

            //if in delivery
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                if (p.DroneId != 0)
                {
                    IDAL.DO.Drone PDrone = dal.GetDrone(p.DroneId);
                    if (p.Delivered == DateTime.MinValue)
                    {

                        DroneToList newD = new DroneToList();

                        newD.Status = DroneStatuses.InDelivery;
                        newD.Id = PDrone.Id;
                        newD.MaxWeight = (WeightCategories)PDrone.MaxWeight;
                        newD.Model = PDrone.Model;
                        newD.CarriedParcelId = p.Id;

                        IDAL.DO.Customer sender = dal.GetCustomer(p.SenderId);
                        IDAL.DO.Customer target = dal.GetCustomer(p.TargetId);

                        if (p.PickedUp == DateTime.MinValue)
                        {
                            BaseStation closestToSender = closestBaseStation(sender.Longitude, sender.Latitude);
                            BaseStation closestToTarget = closestBaseStation(target.Longitude, target.Latitude);
                            newD.CurrentLocation.Latitude = closestToSender.StationLocation.Latitude;
                            newD.CurrentLocation.Longitude = closestToSender.StationLocation.Longitude;
                            newD.Battery =
                                getDistanceFromLatLonInKm(closestToSender.StationLocation.Latitude, closestToSender.StationLocation.Longitude, sender.Latitude, sender.Longitude) / AvailbleElec + //to sender
                                getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) / arr[(int)p.Weight + 1] + //to target
                                getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.StationLocation.Latitude, closestToTarget.StationLocation.Longitude) / AvailbleElec; //to station
                        }
                        else
                        {
                            BaseStation closestToTarget = closestBaseStation(target.Longitude, target.Latitude);
                            newD.CurrentLocation.Latitude = sender.Latitude;
                            newD.CurrentLocation.Longitude = sender.Longitude;
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
                        DalDronesList.Remove(PDrone);
                    }
                    else if (p.Delivered != DateTime.MinValue)
                    {
                        SatisfiedCustomers.Add(dal.GetCustomer(p.TargetId));
                    }
                }
            }

            //if not in delivery
            foreach (IDAL.DO.Drone d in DalDronesList)
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
                                IDAL.DO.Customer SC = SatisfiedCustomers[r.Next(SatisfiedCustomers.Count)];
                                BaseStation bs = closestBaseStation(SC.Longitude, SC.Latitude);
                                newD.Battery = r.Next((int)(getDistanceFromLatLonInKm(SC.Latitude, SC.Longitude, bs.StationLocation.Latitude, bs.StationLocation.Longitude) / (double)AvailbleElec), 100) + 1;
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
                                    IDAL.DO.BaseStation randomBS = dal.GetAllBaseStations().ToList()[r.Next(dal.GetAllBaseStations().ToList().Count)];
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

        private void sendToMaitenance(DroneToList newD)
        {

            List<BaseStation> availibleStation = new List<BaseStation>();

            foreach (IDAL.DO.BaseStation baseStation in dal.GetAllBaseStations())
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
            foreach (IDAL.DO.DroneCharge droneCharge in dal.GetAllDroneCharges())
            {
                if (droneCharge.BaseStationId == baseStation.Id)
                    ACS -= 1;
            }
            return ACS;
        } //returns amount of availible slots in station

        private int getAvailibleSlotsForBaseStation(IDAL.DO.BaseStation baseStation)
        {
            int ACS = baseStation.ChargeSlots;
            foreach (IDAL.DO.DroneCharge droneCharge in dal.GetAllDroneCharges())
            {
                if (droneCharge.BaseStationId == baseStation.Id)
                    ACS -= 1;
            }
            return ACS;
        } //returns amount of availible slots in station

        private BaseStation closestBaseStation(double longitude, double latitude)
        {
            //need to throw exception if there are no BaseStation
            IDAL.DO.BaseStation closest = dal.GetAllBaseStations().ToList()[0];
            double Mindistance = getDistanceFromLatLonIncoords(latitude, longitude, closest.Latitude, closest.Longitude);
            double currentDis;
            foreach (IDAL.DO.BaseStation bs in dal.GetAllBaseStations())
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
            catch (IDAL.DO.ItemAlreadyExistsException ex)
            {
                throw;
            }

        }

        public IEnumerable<BaseStationToList> GetAllBaseStations()
        {
            List<BaseStationToList> AllBaseStations = (from IDAL.DO.BaseStation bs in dal.GetAllBaseStations()
                                                       let ACS = getAvailibleSlotsForBaseStation(bs)
                                                       select new BaseStationToList()
                                                       {
                                                           Id = bs.Id,
                                                           Name = bs.Name,
                                                           ChargeSlotsAvailible = ACS,
                                                           ChargeSlotsTaken = bs.ChargeSlots - ACS
                                                       }).ToList();
            AllBaseStations.Sort();
            return AllBaseStations;
        }

        public IEnumerable<BaseStationToList> GetAllBaseStationsBy(Predicate<BaseStation> predicate)
        {
            return from IDAL.DO.BaseStation bs in dal.GetAllBaseStations()
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
            IDAL.DO.BaseStation bs;
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
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw;
            }
        }

        private IEnumerable<DroneInCharge> GetDronesInBaseStation(IDAL.DO.BaseStation bs)
        {
            return from IDAL.DO.DroneCharge DC in dal.GetAllDroneCharges()
                   where DC.BaseStationId == bs.Id
                   select new DroneInCharge()
                   {
                       Battery = GetDrone(DC.DroneId).Battery,
                       Id = DC.DroneId
                   };
        }

        #endregion

        #region Drones 

        public void AddDrone(int Id, String Model, WeightCategories MaxWeight, int stationId)
        {
            if(Id<1 || stationId<1)
            {
                throw new IBL.BO.NegetiveNumberException(Id, "Id can not be negative number or zero");
            }
            if ((int)MaxWeight < 0|| (int)MaxWeight >2)
            {
                throw new IBL.BO.OutOfRangeException(Id, "Maxweight has to be 1 or 2 or 3 only");
            }
            try
            {
                Random r = new Random();
                IDAL.DO.BaseStation bs = dal.GetBaseStation(stationId);
                dal.AddDrone(Id, Model, (IDAL.DO.WeightCategories)MaxWeight);
                BLDrones.Add(new DroneToList()
                {
                    Id = Id,
                    Battery = r.Next(20,41),
                    MaxWeight = MaxWeight,
                    Model = Model,
                    Status = DroneStatuses.Maintenance,
                    CurrentLocation = new Location()
                    {
                        Latitude = bs.Latitude,
                        Longitude = bs.Longitude
                    },
                    CarriedParcelId = 0
                });
                dal.AddDroneCharge(Id, stationId);
            }
            catch (IDAL.DO.ItemNotFoundException ex) //base station not found
            {
                throw;
            }
            catch (IDAL.DO.ItemAlreadyExistsException ex)
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
            return from Drone d in BLDrones
                   select new DroneToList()
                   {
                       Id = d.Id,
                       Battery = d.Battery,
                       CurrentLocation = d.CurrentLocation,
                       Model = d.Model,
                       Status = d.Status,
                       MaxWeight = d.MaxWeight,
                       CarriedParcelId = dal.GetAllParcels().FirstOrDefault(p => p.DroneId == d.Id).Id
                   };
        }

        public IEnumerable<DroneToList> GetAllDronesBy(Predicate<Drone> predicate)
        {
            return from Drone d in GetAllDrones()
                   where predicate(d)
                   select new DroneToList()
                   {
                       Id = d.Id,
                       Battery = d.Battery,
                       CurrentLocation = d.CurrentLocation,
                       Model = d.Model,
                       Status = d.Status,
                       MaxWeight = d.MaxWeight,
                       CarriedParcelId = dal.GetAllParcels().FirstOrDefault(p => p.DroneId == d.Id).Id
                   };
        }

        public Drone GetDrone(int Id)
        {
            DroneToList  d = BLDrones.FirstOrDefault(p => p.Id == Id);
            if (d == null)
            {
                throw new IDAL.DO.ItemNotFoundException(Id, "Drone Not Found!");
            }
            if (d.CarriedParcelId == 0)
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

            Parcel p = GetParcel(d.CarriedParcelId);
            Customer sender = GetCustomer(p.Sender.Id);
            Customer target = GetCustomer(p.Target.Id);
            return new Drone()
            {
                Id = d.Id,
                Battery = d.Battery,
                CurrentParcel = new ParcelInDelivery()
                {
                    Id = p.Id,
                    ParcelStatus = ((p.PickedUp != DateTime.MinValue) ? true : false),
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

        public Parcel GetParcel(int Id)
        {
            try
            {
                IDAL.DO.Parcel p = dal.GetParcel(Id);
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
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw;
            }


        }

        public Customer GetCustomer(int Id)
        {
            try
            {
                IDAL.DO.Customer c = dal.GetCustomer(Id);

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
            catch (IDAL.DO.ItemNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw;
            }

        }

        private List<ParcelInCustomer> getToThisCustomer(IDAL.DO.Customer c)
        {
            return (from IDAL.DO.Parcel p in dal.GetAllParcels()
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

        private List<ParcelInCustomer> getFromeThisCustomer(IDAL.DO.Customer c)
        {
            return (from IDAL.DO.Parcel p in dal.GetAllParcels()
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

        private ParcelStatuses getParcelStatus(IDAL.DO.Parcel p)
        {
            if (p.Delivered != DateTime.MinValue)
            {
                return ParcelStatuses.Delivered;
            }
            else if (p.PickedUp != DateTime.MinValue)
            {
                return ParcelStatuses.PickedUp;
            }
            else if (p.scheduled != DateTime.MinValue)
            {
                return ParcelStatuses.Associated;
            }
            else
            {
                return ParcelStatuses.Created;
            }
        }

        #endregion

        #region Customers

        public void AddCustomer(int Id, String Name, String Phone, double Longitude, double Latitude)
        {
            if (Id < 1 )
            {
                throw new IBL.BO.NegetiveNumberException(Id, "Id can not be negative number or zero");
            }
            if(Phone.Length != 10)
            {
                throw new InvalidNumberLengthException(Phone.Length);
            }
            try
            {
                dal.AddCustomer(Id, Name, Phone, Longitude, Latitude);
            }
            catch (IDAL.DO.ItemAlreadyExistsException ex)
            {
                throw new ItemAlreadyExistsException(Id, ex.Message, ex);
            }
        } //need to fix

        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            List<CustomerToList> CTL = (from IDAL.DO.Customer c in dal.GetAllCustomers()
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
                                        }).ToList();
            CTL.Sort();
            return CTL;
        }

        private int getRecieved(IDAL.DO.Customer c)
        {
            int sum = 0;
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                if (p.TargetId == c.Id && p.Delivered != DateTime.MinValue && p.PickedUp != DateTime.MinValue)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getOnTheWay(IDAL.DO.Customer c)
        {
            int sum = 0;
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                if (p.TargetId == c.Id && p.Delivered == DateTime.MinValue && p.PickedUp != DateTime.MinValue)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getSentAndNotDelivered(IDAL.DO.Customer c)
        {
            int sum = 0;
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                if (p.SenderId == c.Id && p.Delivered == DateTime.MinValue && p.PickedUp != DateTime.MinValue)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int getSentAndDelivered(IDAL.DO.Customer c)
        {
            int sum = 0;
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                if (p.SenderId == c.Id && p.Delivered != DateTime.MinValue && p.PickedUp != DateTime.MinValue)
                {
                    sum++;
                }
            }
            return sum;
        }
        #endregion

        #region Parcel

        public IEnumerable<ParcelToList> GetAllParcels()
        {
            List<ParcelToList> PTL = (from IDAL.DO.Parcel p in dal.GetAllParcels()
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

        public void AddParcel(int SenderId, int TargetId, IDAL.DO.WeightCategories PackageWight, IDAL.DO.Priorities priority)
        {
           
            if (SenderId < 0)
            {
                throw new IBL.BO.NegetiveNumberException(SenderId, " Sender Id can not be negative number or zero");
            }
            if (TargetId < 0)
            {
                throw new IBL.BO.NegetiveNumberException(TargetId, " Target Id can not be negative number or zero");
            }
            //if ((int)PackageWight < 0 || (int)PackageWight > 2  )
            //{
            //    throw new IBL.BO.OutOfRangeException(Id, "PackageWight has to be 1 or 2 or 3 only");
            //}
            //if ( (int)priority < 0 || (int)priority > 2)
            //{
            //    throw new IBL.BO.OutOfRangeException(Id, "priority has to be 1 or 2 or 3 only");
            //}
            try
            {
                dal.AddParcel (runningNumForParcels, SenderId,  TargetId, PackageWight, priority);
                runningNumForParcels++;
            }
            catch (IDAL.DO.ItemAlreadyExistsException ex)
            {
                throw;
            }

        }

        #endregion
    }
}
