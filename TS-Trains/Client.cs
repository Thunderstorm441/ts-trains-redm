using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using OldWestRPClient.Utilities;
using static CitizenFX.Core.Native.API;

namespace TS_Trains
{
    public class Client : BaseScript
    {
        private static Client _instance;
        public static Client GetInstance()
        {
            return _instance;
        }

        public Client()
        {
            _instance = this;
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
            Log.Info("Starting Trains script by Thunderstorm441");
            Trains.Init();
        }

        public void RegisterEventHandler(string name, Delegate action)
        {
            try
            {
                EventHandlers[name] += action;
                Log.Success($"Registered New Event Handler: {name}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public void RegisterTickHandler(Func<Task> action)
        {
            try
            {
                Tick += action;
                Log.Success($"Registered New Tick Handler: {action.GetHashCode()}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }

}
