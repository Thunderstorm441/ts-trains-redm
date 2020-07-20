using CitizenFX.Core;
using System;

namespace TS_Trains_Server
{
	public class Server : BaseScript
	{
		private static Server _server;

		public static Server GetInstance()
		{
			return _server;
		}

		public Server()
		{
			_server = this;
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
	}
}
