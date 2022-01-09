using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using System.Threading;
using static BLobject.BL;

namespace BLobject
{
    internal class Simulator
    {
        const double DroneSpeed = 400;
        const int TimerCheck = 500;
        Drone drone;
        Parcel currentParcel;
        public Simulator(BL myBL, int DroneId, Action UpdatePL, Func<Boolean> ToCancel)
        {



            while (!ToCancel())
            {
                Thread.Sleep(TimerCheck);
                lock (myBL)
                {
                    drone = myBL.GetDrone(DroneId);
                }
                if (drone.Status == DroneStatuses.Availible)
                {
                    try
                    {
                        if (drone.Battery > 95)
                        {
                            myBL.AttributionParcelToDrone(DroneId);
                        }
                        else
                        {
                            myBL.ChargeDrone(DroneId);
                            continue;
                        }
                    }
                    catch (BO.NoParcelForThisDrone ex)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }
                }
                else if (drone.Status == DroneStatuses.Maintenance)
                {
                    if (drone.Battery < 95)
                    {
                        lock (myBL)
                        {
                            drone.Battery = myBL.chargeDrone(DroneId, TimerCheck);
                        }
                    }
                    else
                    {
                        lock (myBL)
                        {
                            myBL.disChargeDrone(DroneId);
                        }
                    }
                }
                else if(drone.Status == DroneStatuses.InDelivery && drone.CurrentParcel.Id != null)
                {
                    lock (myBL)
                    {
                        currentParcel = myBL.GetParcel((int)drone.CurrentParcel.Id);
                        if (currentParcel.PickedUp is not null)
                        {
                            DO.Customer target = myBL.dal.GetCustomer(currentParcel.Target.Id);
                            Location targetL = new Location() { Latitude = target.Latitude, Longitude = target.Longitude };
                            if (myBL.GoTowards(DroneId, targetL, DroneSpeed, myBL.getElecForWeight((BO.WeightCategories)(currentParcel.Weight))) == targetL)
                            {
                                myBL.SupplyParcel(DroneId);
                            }

                        }
                        else if (currentParcel.scheduled is not null)
                        {
                            DO.Customer sender = myBL.dal.GetCustomer(currentParcel.Sender.Id);
                            Location senderL = new Location() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                            if (myBL.GoTowards(DroneId, senderL, DroneSpeed, myBL.AvailbleElec) == senderL)
                            {
                                myBL.PickUpParcelByDrone(DroneId);
                            }
                        }
                    }
                }
                UpdatePL();

            }
        }

        
    }
}
