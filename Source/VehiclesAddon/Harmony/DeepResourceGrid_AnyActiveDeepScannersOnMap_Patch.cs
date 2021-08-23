using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using Vehicles;

namespace Quenn1599.VehiclesAddon.Harmony
{
    [HarmonyPatch(typeof(DeepResourceGrid), "AnyActiveDeepScannersOnMap")]
    public static class DeepResourceGrid_AnyActiveDeepScannersOnMap_Patch
    {
        static void Postfix(ref bool __result, Map ___map)
        {
            if (!__result)
            {
                foreach (Pawn p in ___map.mapPawns.AllPawnsSpawned)
                {
                    if (p is VehiclePawn vehicle)
                    {
                        CompDeepScanner compDeepScanner = vehicle.TryGetComp<CompDeepScanner>();
                        if (compDeepScanner != null && compDeepScanner.ShouldShowDeepResourceOverlay())
                        {
                            __result = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
