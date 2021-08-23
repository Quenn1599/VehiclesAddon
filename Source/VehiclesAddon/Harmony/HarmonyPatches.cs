using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using HarmonyLib;

namespace Quenn1599.VehiclesAddon
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches {
        static HarmonyPatches()
        {
            var harmony = new Harmony("quenn1599.vehiclesaddon");
            harmony.PatchAll();
        }
    }
}
