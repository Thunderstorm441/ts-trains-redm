# ts-trains-redm
Adds the singleplayer trains &amp; tram to RedM.
Originally for [OldWesternRP](https://oldwesternrp.com) & made standalone for any [RedM](https://redm.gg) server to use.

# Install

1. Download the latest version from [Releases](https://github.com/Thunderstorm441/ts-trains-redm/releases)
2. Drop `ts-trains` into the resources folder
3. Add `ensure ts-trains` to your config file

# Planned features

+ Config file for train routes & stops
+ NPC Passengers
+ Robbable trains, NPC + Armoured

# Getting Switch info

https://vespura.com/doc/natives/#_0x785639D89F8451AB
Using the above native you can locate the track switches, the first parameter is the track hash and the second parameter is the index of the switch.

To find track switches I reccommend looking through decompiled R* scripts to find switch ids https://github.com/stianhje/rdr3-decompiled-scripts.1232/

# Adding a new train
1. Find the train stops and get the coordinates + heading for each.
2. Find the track switches and get the coordinates, heading, track id, switch id & switch state.
3. Copy the below code and rename the subroutine.
  ```C#
  static TrainRoute exampleTrainRoute = new TrainRoute(new List<Vector4>(){
              new Vector4(0f, 0f, 0f, 0f), //Train stop coordinates + heading
              new Vector4(2970.45f,1313.431f,45.47572f,344.4733f), // Example of above
          }, new List<TrackSwitch>(){
              new TrackSwitch(new Vector4(0f, 0f, 0f, 0f), 0, 0, 0), //Track switch info
              new TrackSwitch(new Vector4(2659.79f,-435.7114f,43.38848f,5.359f),-705539859, 13, 0), // Example of above
          }, 5.0, // Train Speed
          new Vector3(2608.539f, -1171.967f, 53.77959f), //Train start coordinates
          TrainModels.Train8, 10);

          public static int exampleTrainHandle = 0;




          static async Task ExampleTrainTick()
          {
              try
              {
                  if (API.NetworkIsHost())
                  {
                      TrainRoute tr = exampleTrainRoute; // Set to config
                      if (!API.DoesEntityExist(exampleTrainHandle))
                      {
                          Log.Info("Creating Example Train");
                          valTrainHandle = await TrainVehicle.Create(tr.Model, tr.TrainSpawn, tr.Npcs);
                          BaseScript.TriggerServerEvent("Trains.Update", API.VehToNet(valTrainHandle), API.VehToNet(bigTrainHandle), API.VehToNet(tramTrainHandle),API.VehToNet(exampleTrainHandle)); //Update with new train
                      }
                      else
                      {
                          TrackSwitch nextSwitch = tr.Switches.OrderBy(o => Distance.EntityDistanceToSquared(exampleTrainHandle, new Vector3(o.switchLoc.X, o.switchLoc.Y, o.switchLoc.Z))).First();
                          if (Distance.EntityDistanceToSquared(valTrainHandle, new Vector3(nextSwitch.switchLoc.X, nextSwitch.switchLoc.Y, nextSwitch.switchLoc.Z)) < 15f)
                          {
                              Function.Call((Hash)0xE6C5E2125EB210C1, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                              Function.Call((Hash)0x3ABFA128F5BF5A70, nextSwitch.switchId, nextSwitch.switchState1, nextSwitch.switchState2);
                          }
                          Vector4 nextStop = tr.Stops.OrderBy(o => Distance.EntityDistanceToSquared(exampleTrainHandle, new Vector3(o.X, o.Y, o.Z))).First();
                          if (Distance.EntityDistanceToSquared(exampleTrainHandle, new Vector3(nextStop.X, nextStop.Y, nextStop.Z)) < 10f)
                          {
                              API.SetTrainCruiseSpeed(exampleTrainHandle, 0f);
                              await BaseScript.Delay(tr.Cooldown * 1000);
                              API.SetTrainCruiseSpeed(exampleTrainHandle, (float)tr.CruiseSpeed);
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
```
          
          
4. Register the tick handler in the [TrainRequestHandler](https://github.com/Thunderstorm441/ts-trains-redm/blob/master/TS-Trains/Trains.cs#L323) subroutine.
5. Add the new train to the [server side code](https://github.com/Thunderstorm441/ts-trains-redm/blob/master/TS-Trains-Server/Trains.cs) by simply copying the same format used for the other trains.
6. Compile
