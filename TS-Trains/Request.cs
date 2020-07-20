using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
namespace OldWestRPClient.Utilities
{
    static class Request
    {
        static public async Task Model(int model)
        {
            while (!Function.Call<bool>((Hash)0x1283B8B89DD5D1B6, model))
            {
                Function.Call((Hash)0xFA28FE3A6246FC30, model);
                await BaseScript.Delay(1);
            }
            await Task.FromResult(0);
        }
    }

    static class NoLongerNeeded
    {
        static public async Task Model(int model)
        {
            Function.Call((Hash)0x4AD96EF928BD4F9A, model);
            await BaseScript.Delay(1);
            await Task.FromResult(0);
        }
    }
}

