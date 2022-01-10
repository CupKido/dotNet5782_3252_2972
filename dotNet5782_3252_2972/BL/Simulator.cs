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
        const double DroneSpeed = 50;
        const int TimerCheck = 500;
        Drone drone;
        Parcel currentParcel;
        BaseStation toChargeIn;
        public Simulator(BL myBL, int DroneId, Action UpdatePL, Func<Boolean> ToCancel)
        {



            while (!ToCancel())
            {
                Thread.Sleep(TimerCheck);
                lock (myBL)
                {
                    try
                    {
                        drone = myBL.GetDrone(DroneId);
                    }
                    catch
                    {
                        return;
                    }
                }
                if (drone.Status == DroneStatuses.Availible)
                {

                    if (myBL.canSupplySomthing(drone) || drone.Battery >= 95)
                    {
                        try
                        {
                            myBL.AttributionParcelToDrone(DroneId);

                        }
                        catch (BO.NoParcelForThisDrone ex)
                        {
                            myBL.subtructStandingBattery(DroneId);
                            UpdatePL();
                            Thread.Sleep(3000);
                            continue;
                        }
                    }
                    else
                    {
                        if(drone.Battery <= 0)
                        {
                            myBL.DeleteDrone(DroneId);
                            UpdatePL();
                            return;
                        }
                        try
                        {
                            toChargeIn = myBL.closestAvailibleBaseStation(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude);
                            if (myBL.GoTowards(DroneId, toChargeIn.StationLocation, DroneSpeed, myBL.AvailbleElec) == toChargeIn.StationLocation)
                            {
                                myBL.ChargeDrone(DroneId);
                            }
                        }
                        catch (BO.NotEnoughDroneBatteryException ex)
                        {
                            UpdatePL();
                            return;
                        }
                    }
                }
                else if (drone.Status == DroneStatuses.Maintenance)
                {
                    if (drone.Battery < 99)
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
                else if (drone.Status == DroneStatuses.InDelivery && drone.CurrentParcel.Id != null)
                {
                    lock (myBL)
                    {
                        currentParcel = myBL.GetParcel((int)drone.CurrentParcel.Id);
                        if (currentParcel.PickedUp is not null)
                        {
                            lock (myBL.dal)
                            {
                                DO.Customer target = myBL.dal.GetCustomer(currentParcel.Target.Id);
                                Location targetL = new Location() { Latitude = target.Latitude, Longitude = target.Longitude };
                                if (myBL.GoTowards(DroneId, targetL, DroneSpeed, myBL.getElecForWeight((BO.WeightCategories)(currentParcel.Weight))) == targetL)
                                {
                                    myBL.SupplyParcel(DroneId);
                                }
                            }

                        }
                        else if (currentParcel.scheduled is not null)
                        {
                            lock (myBL.dal)
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
                }
                UpdatePL();

            }
        }


    }
}
