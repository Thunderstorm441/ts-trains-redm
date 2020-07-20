using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace OldWestRPClient.Utilities
{
    static class Distance
    {
        public static bool IsPlayerWithinDistance(Vector3 v, float dist) => Math.Abs((API.GetEntityCoords(API.PlayerPedId(), true, true) - v).Length()) < dist;

        public static float DistanceToSquared(Vector3 v) => Math.Abs((API.GetEntityCoords(API.PlayerPedId(), true, true) - v).Length());

        public static float EntityDistanceToSquared(int a, Vector3 v) => Math.Abs((API.GetEntityCoords(a, true, true) - v).Length());
    }
}
