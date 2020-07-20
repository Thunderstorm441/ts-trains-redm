using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldWestRPClient.Utilities;

namespace TS_Trains
{
    static class Trains
    {
        enum TrainModels : int
        {
            Train1 = -1464742217,
            Train2 = -577630801,
            Train3 = -1901305252,
            Train4 = -1719006020,
            Train5 = 519580241,
            Train6 = 1495948496,
            Train7 = 1365127661,
            Train8 = -1083616881,
            Train9 = 1030903581,
            Train10 = -2006657222,
            Train11 = 1285344034,
            Train12 = -156591884,
            Train13 = 987516329,
            Train14 = -1740474560,
            Train15 = -651487570,
            Train16 = -593637311,
            Train17 = 1094934838,
            Train18 = 1054492269,
            Train19 = 1216031719,
        }

        static class TrainVehicle
        {


            static public async Task<int> Create(TrainModels trainModel, Vector3 loc, bool npcs)
            {
                int trainWagons = API.N_0x635423d55ca84fc8((int)trainModel);

                for (int i = 0; i < trainWagons; i++)
                {
                    int trainWagonModel = API.N_0x8df5f6a19f99f0d5((int)trainModel, i);
                    await Request.Model(trainWagonModel);
                }
                int handle = Function.Call<int>((Hash)0xC239DBD9A57D2A71, trainModel, loc.X, loc.Y, loc.Z, 0, 0, 1, 1);

                API.SetTrainSpeed(handle, 0f);

                //Driver
                int trainDriverHandle = API.GetPedInVehicleSeat(handle, -1);
                while (!API.DoesEntityExist(trainDriverHandle))
                {
                    trainDriverHandle = API.GetPedInVehicleSeat(handle, -1);
                    await BaseScript.Delay(100);
                }
                Function.Call((Hash)0xA5C38736C426FCB8, trainDriverHandle, true); // godmode
                Function.Call((Hash)0x9F8AA94D6D97DBF4, trainDriverHandle, true); //ignore events
                Function.Call((Hash)0x63F58F7C80513AAD, trainDriverHandle, false); //can be targeted
                Function.Call((Hash)0x7A6535691B477C48, trainDriverHandle, false); // knocked out of seat

                Function.Call((Hash)0x05254BA0B44ADC16, handle, false); // train cant be targeted 
                return handle;
            }
        }

        class TrackSwitch
        {
            public Vector4 switchLoc { get; set; }
            public int switchId { get; set; }
            public int switchState1 { get; set; }
            public int switchState2 { get; set; }

            public TrackSwitch(Vector4 s, int sid, int ss1, int ss2)
            {
                this.switchLoc = s;
                this.switchId = sid;
                this.switchState1 = ss1;
                this.switchState2 = ss2;
            }
        }

        class TrainRoute
        {

            public List<Vector4> Stops { get; set; }
            public List<TrackSwitch> Switches { get; set; }
            public double CruiseSpeed { get; set; }
            public Vector3 TrainSpawn { get; set; }
            public int Cooldown { get; set; }
            public TrainModels Model { get; set; }
            public bool Npcs { get; set; }


            public TrainRoute(List<Vector4> stops, List<TrackSwitch> switches, double speed, Vector3 spawn, TrainModels model, int cooldown, bool npcs)
            {
                this.Stops = stops;
                this.Switches = switches;
                this.CruiseSpeed = speed;
                this.TrainSpawn = spawn;
                this.Cooldown = cooldown;
                this.Model = model;
                this.Npcs = npcs;
            }

            public TrainRoute(List<Vector4> stops, List<TrackSwitch> switches, double speed, Vector3 spawn, TrainModels model, int cooldown)
            {
                this.Stops = stops;
                this.Switches = switches;
                this.CruiseSpeed = speed;
                this.TrainSpawn = spawn;
                this.Cooldown = cooldown;
                this.Model = model;
                this.Npcs = true;
            }
        }



        static TrainRoute ValTrainRoute = new TrainRoute(new List<Vector4>(){
            new Vector4(-150.728f,643.9002f,115.1231f,318.8281f),
            new Vector4(1528.799f,417.9227f,91.82778f,179.8871f),
            new Vector4(2748.035f,-1436.106f,47.473f,319.94f),
            new Vector4(2897.273f,648.3684f,58.52696f,344.9448f),
            new Vector4(2970.45f,1313.431f,45.47572f,344.4733f),
            new Vector4(-1327.832f,379.8979f,97.19212f,130.2857f),
            new Vector4(-1086.135f,-592.3543f,82.74096f,235.7766f),
            new Vector4(-318.5835f,-339.5699f,89.8374f,296.912f),
            new Vector4(-152.3961f,641.8745f,115.1232f,319.4841f),
        }, new List<TrackSwitch>(){
            new TrackSwitch(new Vector4(357.959f,596.374f,115.6759f,312f),1499637393, 4, 1),
            new TrackSwitch(new Vector4(1481.54f,648.331f,92.30682f,241.38f),1499637393, 2, 1),
            new TrackSwitch(new Vector4(2464.55f,-1475.74f,46.15192f,246.75f),-760570040, 5, 1),
            new TrackSwitch(new Vector4(2654.026f,-1477.149f,45.75834f,86.64f),-1242669618, 2, 1),
            new TrackSwitch(new Vector4(2659.79f,-435.7114f,43.38848f,5.359f),-705539859, 13, 0),
            new TrackSwitch(new Vector4(-281.1323f,-319.6579f,89.02458f,131.1303f),-705539859, 2, 1),
        }, 10.0, new Vector3(-504.0194f, -432.0699f, 82.54294f), TrainModels.Train1, 20);

        public static int valTrainHandle = 0;

        static TrainRoute BigTrainRoute = new TrainRoute(new List<Vector4>(){
            new Vector4(2897.273f,648.3684f,58.52696f,344.9448f),
            new Vector4(2970.45f,1313.431f,45.47572f,344.4733f),
            new Vector4(-1327.832f,379.8979f,97.19212f,130.2857f),
            new Vector4(-1086.135f,-592.3543f,82.74096f,235.7766f),
            new Vector4(-318.5835f,-339.5699f,89.8374f,296.912f),
            new Vector4(1246.924f,-1331.255f,78.03119f,228.0852f),
            new Vector4(2748.035f,-1436.106f,47.473f,319.94f),
        }, new List<TrackSwitch>(){
            new TrackSwitch(new Vector4(2659.79f,-435.7114f,43.38848f,5.359f),-705539859, 13, 0),
            new TrackSwitch(new Vector4(610.3571f,1661.904f,187.3867f,205.82f ), -705539859, 8, 1),
            new TrackSwitch(new Vector4(556.65f, 1725.99f, 187.7966f, 40.04874f), -705539859, 7, 1),
            new TrackSwitch(new Vector4(-281.1323f,-319.6579f,89.02458f,131.1303f), -705539859, 2, 0),
            new TrackSwitch(new Vector4(2588.54f,-1482.19f,46.04693f,270f), -705539859, 18, 1),
            new TrackSwitch(new Vector4(2654.026f,-1477.149f,45.75834f,86.64f),-1242669618, 2, 1),
        }, 10.0, new Vector3(1908.238f, -1638.898f, 43.140f), TrainModels.Train4, 20);

        public static int bigTrainHandle = 0;

        static TrainRoute tramTrainRoute = new TrainRoute(new List<Vector4>(){
            new Vector4(2608.522f, -1204.119f, 53.96195f, 180f),
            new Vector4(2613.163f, -1276.121f, 53.27362f, 201.8492f),
            new Vector4(2678.167f, -1375.313f, 47.801f, 220.0522f),
            new Vector4(2751.001f, -1408.953f, 46.649f, 295.5167f),
            new Vector4(2806.234f, -1312.762f, 47.27655f, 317.6901f),
            new Vector4(2807.307f, -119.984f, 48.09161f, 4.916435f),
        }, new List<TrackSwitch>(){
            new TrackSwitch(new Vector4(2615.05f, -1281.2f, 52.34358f, 179.609f), -1739625337, 6, 0),
            new TrackSwitch(new Vector4(2608.49f, -1254.66f, 52.66566f, 179.609f), -1739625337, 7, 0),
            new TrackSwitch(new Vector4(2686.55f, -1385.46f, 46.36679f, 179.609f), -1739625337, 3, 1),
            new TrackSwitch(new Vector4(2624.4f, -1139.85f, 51.51707f, 179.609f), -1739625337, 11, 0),
        }, 5.0, new Vector3(2608.539f, -1171.967f, 53.77959f), TrainModels.Train8, 10);

        public static int tramTrainHandle = 0;




        static async Task ValTrainTick()
        {
            try
            {
                if (API.NetworkIsHost())
                {
                    TrainRoute tr = ValTrainRoute;
                    if (!API.DoesEntityExist(valTrainHandle))
                    {
                        Log.Info("Creating Val Train");
                        valTrainHandle = await TrainVehicle.Create(tr.Model, tr.TrainSpawn, tr.Npcs);
                        BaseScript.TriggerServerEvent("Trains.Update", API.VehToNet(valTrainHandle), API.VehToNet(bigTrainHandle), API.VehToNet(tramTrainHandle));
                    }
                    else
                    {
                        TrackSwitch nextSwitch = tr.Switches.OrderBy(o => Distance.EntityDistanceToSquared(valTrainHandle, new Vector3(o.switchLoc.X, o.switchLoc.Y, o.switchLoc.Z))).First();
                        if (Distance.EntityDistanceToSquared(valTrainHandle, new Vector3(nextSwitch.switchLoc.X, nextSwitch.switchLoc.Y, nextSwitch.switchLoc.Z)) < 15f)
                        {
                            Function.Call((Hash)0xE6C5E2125EB210C1, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                            Function.Call((Hash)0x3ABFA128F5BF5A70, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                        }
                        Vector4 nextStop = tr.Stops.OrderBy(o => Distance.EntityDistanceToSquared(valTrainHandle, new Vector3(o.X, o.Y, o.Z))).First();
                        if (Distance.EntityDistanceToSquared(valTrainHandle, new Vector3(nextStop.X, nextStop.Y, nextStop.Z)) < 10f)
                        {
                            API.SetTrainCruiseSpeed(valTrainHandle, 0f);
                            await BaseScript.Delay(tr.Cooldown * 1000);
                            API.SetTrainCruiseSpeed(valTrainHandle, (float)tr.CruiseSpeed);
                        }
                    }
                }
                else
                {
                    await BaseScript.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            await BaseScript.Delay(1000);
            await Task.FromResult(0);

        }

        static async Task BigTrainTick()
        {
            try
            {
                if (API.NetworkIsHost())
                {
                    TrainRoute tr = BigTrainRoute;
                    if (!API.DoesEntityExist(bigTrainHandle))
                    {
                        Log.Info("Creating Big Train");
                        bigTrainHandle = await TrainVehicle.Create(tr.Model, tr.TrainSpawn, tr.Npcs);

                        BaseScript.TriggerServerEvent("Trains.Update", API.VehToNet(valTrainHandle), API.VehToNet(bigTrainHandle), API.VehToNet(tramTrainHandle));
                    }
                    else
                    {
                        TrackSwitch nextSwitch = tr.Switches.OrderBy(o => Distance.EntityDistanceToSquared(bigTrainHandle, new Vector3(o.switchLoc.X, o.switchLoc.Y, o.switchLoc.Z))).First();
                        if (Distance.EntityDistanceToSquared(bigTrainHandle, new Vector3(nextSwitch.switchLoc.X, nextSwitch.switchLoc.Y, nextSwitch.switchLoc.Z)) < 50f)
                        {
                            Function.Call((Hash)0xE6C5E2125EB210C1, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                            Function.Call((Hash)0x3ABFA128F5BF5A70, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                        }
                        Vector4 nextStop = tr.Stops.OrderBy(o => Distance.EntityDistanceToSquared(bigTrainHandle, new Vector3(o.X, o.Y, o.Z))).First();
                        if (Distance.EntityDistanceToSquared(bigTrainHandle, new Vector3(nextStop.X, nextStop.Y, nextStop.Z)) < 10f)
                        {
                            API.SetTrainCruiseSpeed(bigTrainHandle, 0f);
                            await BaseScript.Delay(tr.Cooldown * 1000);
                            API.SetTrainCruiseSpeed(bigTrainHandle, (float)tr.CruiseSpeed);
                        }
                    }
                }
                else
                {
                    await BaseScript.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            await BaseScript.Delay(1000);
            await Task.FromResult(0);

        }

        static async Task TramTrainTick()
        {
            try
            {
                if (API.NetworkIsHost())
                {
                    TrainRoute tr = tramTrainRoute;
                    if (!API.DoesEntityExist(tramTrainHandle))
                    {
                        Log.Info("Creating Tram");
                        tramTrainHandle = await TrainVehicle.Create(tr.Model, tr.TrainSpawn, tr.Npcs);
                        BaseScript.TriggerServerEvent("Trains.Update", API.VehToNet(valTrainHandle), API.VehToNet(bigTrainHandle), API.VehToNet(tramTrainHandle));
                    }
                    else
                    {
                        TrackSwitch nextSwitch = tr.Switches.OrderBy(o => Distance.EntityDistanceToSquared(tramTrainHandle, new Vector3(o.switchLoc.X, o.switchLoc.Y, o.switchLoc.Z))).First();
                        if (Distance.EntityDistanceToSquared(tramTrainHandle, new Vector3(nextSwitch.switchLoc.X, nextSwitch.switchLoc.Y, nextSwitch.switchLoc.Z)) < 25f)
                        {
                            Function.Call((Hash)0xE6C5E2125EB210C1, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                            Function.Call((Hash)0x3ABFA128F5BF5A70, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                        }
                        Vector4 nextStop = tr.Stops.OrderBy(o => Distance.EntityDistanceToSquared(tramTrainHandle, new Vector3(o.X, o.Y, o.Z))).First();
                        if (Distance.EntityDistanceToSquared(tramTrainHandle, new Vector3(nextStop.X, nextStop.Y, nextStop.Z)) < 5f)
                        {
                            API.SetTrainCruiseSpeed(tramTrainHandle, 0f);
                            await BaseScript.Delay(tr.Cooldown * 1000);
                            API.SetTrainCruiseSpeed(tramTrainHandle, (float)tr.CruiseSpeed);
                        }
                    }
                }
                else
                {
                    await BaseScript.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            await BaseScript.Delay(1000);
            await Task.FromResult(0);

        }


        static public void Init()
        {
            Client.GetInstance().RegisterEventHandler("Trains.RequestCallback", new Action<int, int, int>(TrainRequestHandler));
            BaseScript.TriggerServerEvent("Trains.Request");
        }

        private static void TrainRequestHandler(int tVal, int tBig, int tTram)
        {
            try
            {
                valTrainHandle = API.NetToVeh(tVal);
                bigTrainHandle = API.NetToVeh(tBig);
                tramTrainHandle = API.NetToVeh(tTram);

                Client.GetInstance().RegisterTickHandler(ValTrainTick);
                Client.GetInstance().RegisterTickHandler(BigTrainTick);
                Client.GetInstance().RegisterTickHandler(TramTrainTick);
            }
            catch (Exception e)
            {
                Log.Error("Error in Train setup");
                Log.Error(e.Message);
            }

        }
    }
}

