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
            List<IDAL.DO.Drone> DalDrones = dal.GetAllDrones();
            List<IDAL.DO.Drone> DalDronesList = dal.GetAllDrones();

            Random r = new Random();
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
                        Sender = new CustomerInDelivery()
                        {
                            Id = p.SenderId,
                            Name = dal.GetCustomer(p.SenderId).Name
                        },
                        Target = new CustomerInDelivery()
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
                        BaseStation closestToSender = ClosestBaseStation(sender.Longitude, sender.Latitude);
                        BaseStation closestToTarget = ClosestBaseStation(target.Longitude, target.Latitude);
                        newD.CurrentLocation.Latitude = closestToSender.Latitude;
                        newD.CurrentLocation.Longitude = closestToSender.Longitude;
                        newD.Battery =
                            getDistanceFromLatLonInKm(closestToSender.Latitude, closestToSender.Longitude, sender.Latitude, sender.Longitude) / AvailbleElec + //to sender
                            getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) / arr[(int)p.Weight + 1] + //to target
                            getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.Latitude, closestToTarget.Longitude) / AvailbleElec; //to station
                    }
                    else
                    {
                        BaseStation closestToTarget = ClosestBaseStation(target.Longitude, target.Latitude);
                        newD.CurrentLocation.Latitude = sender.Latitude;
                        newD.CurrentLocation.Longitude = sender.Longitude;
                        newD.Battery =
                            getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) / arr[(int)p.Weight + 1] + //to target
                            getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.Latitude, closestToTarget.Longitude) / AvailbleElec; //to station
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
                            BaseStation bs = ClosestBaseStation(SC.Longitude, SC.Latitude);
                            newD.Battery = r.Next((int)(getDistanceFromLatLonInKm(SC.Latitude, SC.Longitude, bs.Latitude, bs.Longitude) / (double)AvailbleElec), 100) + 1;
                        }
                        break;
                    case 1:
                        {
                            newD.Status = DroneStatuses.Maintenance;
                            newD.Battery = r.Next(20);
                        }
                        break;
                }
                BLDrones.Add(newD);

            }




        }

        private BaseStation ClosestBaseStation(double longitude, double latitude)
        {
            BaseStation newBS = new BaseStation();
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
            newBS.Id = closest.Id;
            newBS.Latitude = closest.Latitude;
            newBS.Longitude = closest.Longitude;
            newBS.Name = closest.Name;
            newBS.ChargeSlots = closest.ChargeSlots;
            return newBS;
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
    }
}
