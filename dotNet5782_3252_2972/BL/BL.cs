using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL : IBL.IBL
    {
        public  List<IBL.BO.Drone> Drones = new List<IBL.BO.Drone>();
        public IDAL.IDal dal ; //with dal we have access to data sorce
        public Double AvailbleElec { get; set; }
        public Double LightElec { get; set; }
        public Double IntermediateElec { get; set; }
        public Double HeavyElec { get; set; }
        public Double ChargePerHours { get; set; }
        public BL()
        {
            dal = new DalObject.DalObject();
            double[] arr= dal.AskForElectricity();
            AvailbleElec = arr[0];
            LightElec = arr[1];
            IntermediateElec = arr[2];
            HeavyElec = arr[3];
            ChargePerHours = arr[4];
            List<IDAL.DO.Drone> DalDrones = dal.GetAllDrones();
            List<Drone> BLDrones = new List<Drone>();
            //לבדוק על רחפן אם ישנה חבילה ששויכה לו ועדיין לא סופקה ללקוח
            foreach (IDAL.DO.Drone d in DalDrones)
            {
                Drone newD = new Drone();
                newD.Id = d.Id;
                newD.MaxWeight = d.MaxWeight;
                newD.Model = d.Model;

                foreach (IDAL.DO.Parcel p in dal.GetAllParcels())
                {
                    if (d.Id == p.DroneId)
                    {
                        if (p.Delivered == new DateTime()) //if now delivered
                        {
                            newD.Status = DroneStatuses.InDelivery;
                            IDAL.DO.Customer sender = dal.GetCustomer(p.SenderId);
                            IDAL.DO.Customer target = dal.GetCustomer(p.TargetId);
                            double needToTravel;
                            if(p.PickedUp == new DateTime()) //if not picked up
                            {
                                BaseStation closestToSender = ClosestBaseStation(sender.Longitude, sender.Latitude);
                                BaseStation closestToTarget = ClosestBaseStation(target.Longitude, target.Latitude);
                                newD.Latitude = closestToSender.Latitude;
                                newD.Longitude = closestToSender.Longitude;
                                newD.Battery =
                                    getDistanceFromLatLonInKm(closestToSender.Latitude, closestToSender.Longitude, sender.Latitude, sender.Longitude) * AvailbleElec + //to sender
                                    getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) * arr[(int)p.Weight + 1] + //to target
                                    getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.Latitude, closestToTarget.Longitude) * AvailbleElec; //to station
                            }
                            else //if picked up
                            {
                                BaseStation closestToTarget = ClosestBaseStation(target.Longitude, target.Latitude);
                                newD.Latitude = sender.Latitude;
                                newD.Longitude = sender.Longitude;
                                newD.Battery =
                                    getDistanceFromLatLonInKm(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude) * arr[(int)p.Weight + 1] + //to target
                                    getDistanceFromLatLonInKm(target.Latitude, target.Longitude, closestToTarget.Latitude, closestToTarget.Longitude) * AvailbleElec; //to station
                            }
                            
                            
                        }
                        
                    }
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
