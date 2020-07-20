using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Trains_Server
{
    static class Trains
    {
        private static int valTrain = 0;
        private static int bigTrain = 0;
        private static int tramTrain = 0;
        public static void Init()
        {
            Server.GetInstance().RegisterEventHandler("Trains.Update", new Action<Player, int, int, int>(SetTrainHandles));
            Server.GetInstance().RegisterEventHandler("Trains.Request", new Action<Player>(ReqTrainHandles));
        }

        private static void SetTrainHandles([FromSource] Player p, int t1, int t2, int t3)
        {
            valTrain = t1;
            bigTrain = t2;
            tramTrain = t3;
        }

        private static void ReqTrainHandles([FromSource] Player p)
        {
            p.TriggerEvent("Trains.RequestCallback", valTrain, bigTrain, tramTrain);
        }
    }
}
