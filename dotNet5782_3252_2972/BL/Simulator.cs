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
        const double DroneSpeed = 1000;
        const int TimerCheck = 500;
        Drone drone;
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
                        drone.Battery = myBL.chargeDrone(DroneId, TimerCheck);
                    }
                    else
                    {

                        myBL.disChargeDrone(DroneId);
                    }
                }
                else if(drone.Status == DroneStatuses.InDelivery)
                {

                }
                UpdatePL();

            }
        }
    }
}
