using IBL.BO;
using System;
using System.Collections.Generic;
namespace BL
{
    public partial class BL : IBL.IBL
    {
        public List<IBL.BO.Drone> Drones = new List<IBL.BO.Drone>();
        public IDAL.IDal dal; //with dal we have access to data source
        List<Drone> BLDrones = new List<Drone>();
        public Double AvailbleElec { get; set; }
        public Double LightElec { get; set; }
        public Double IntermediateElec { get; set; }
        public Double HeavyElec { get; set; }
        public Double ChargePerHours { get; set; }
        public BL()
        {
            dal = new DalObject.DalObject();


            double[] arr = dal.AskForElectricity();
            AvailbleElec = arr[0];
            LightElec = arr[1];
            IntermediateElec = arr[2];
            HeavyElec = arr[3];
            ChargePerHours = arr[4];
            List<IDAL.DO.Customer> SatisfiedCustomers = new List<IDAL.DO.Customer>();
            List<IDAL.DO.Drone> DalDronesList = dal.GetAllDrones();

            Random r = new Random();

            //if in delivery
            foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
            {
                IDAL.DO.Drone PDrone = dal.GetDrone(p.DroneId);
                if (p.DroneId != 0 && p.Delivered == DateTime.MinValue)
                {

                    Drone newD = new Drone();

                    newD.Status = DroneStatuses.InDelivery;
                    newD.Id = PDrone.Id;
                    newD.MaxWeight = (WeightCategories)PDrone.MaxWeight;
                    newD.Model = PDrone.Model;
                    newD.CurrentParcel = new ParcelInDelivery()
                    {
                        Id = p.Id,
                        Sender = new CustomerInParcel()
                        {
                            Id = p.SenderId,
                            Name = dal.GetCustomer(p.SenderId).Name
                        },
                        Target = new CustomerInParcel()
                        {
                            Id = p.TargetId,
                            Name = dal.GetCustomer(p.TargetId).Name
                        },
                        priority = (Priorities)p.Priority
                    };

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

            //if not in delivery
            foreach (IDAL.DO.Drone d in DalDronesList)
            {
                Drone newD = new Drone();
                newD.Id = d.Id;
                newD.MaxWeight = (WeightCategories)d.MaxWeight;
                newD.Model = d.Model;
                switch (r.Next(2))
                {
                    case 0:
                        {
                            newD.Status = DroneStatuses.Availible;
                            IDAL.DO.Customer SC = SatisfiedCustomers[r.Next(SatisfiedCustomers.Count)];
                            BaseStation bs = closestBaseStation(SC.Longitude, SC.Latitude);
                            newD.Battery = r.Next((int)(getDistanceFromLatLonInKm(SC.Latitude, SC.Longitude, bs.StationLocation.Latitude, bs.StationLocation.Longitude) / (double)AvailbleElec), 100) + 1;
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

        private void sendToMaitenance(Drone newD)
        {
            List<BaseStation> availibleStation = new List<BaseStation>();
            int ACS = 0; //Availible Charge Slots
            foreach (IDAL.DO.BaseStation baseStation in dal.GetAllBaseStations())
            {
                ACS = baseStation.ChargeSlots;
                foreach (IDAL.DO.DroneCharge droneCharge in dal.GetAllDroneCharges())
                {
                    if (droneCharge.BaseStationId == baseStation.Id)
                        ACS -= 1;
                }

                if (ACS != 0)
                {
                    availibleStation.Add(new BaseStation()
                    {
                        Id = baseStation.Id
                    });
                }
            }

            Random r = new Random();
            dal.AddDroneCharge( newD.Id, availibleStation[ r.Next(availibleStation.Count) ].Id);

        }

        private BaseStation closestBaseStation(double longitude, double latitude)
        {
            //need to throw exception if there are no BaseStation
            IDAL.DO.BaseStation closest = dal.GetAllBaseStations()[0];
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

        public void AddBaseStations(int Id, string Name, Location StationLocation, int ChargeSlots)
        {
            dal.AddBaseStations(Id, Name, StationLocation.Longitude, StationLocation.Latitude, ChargeSlots);
        }
    }
}
