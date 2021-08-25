using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using Vehicles;

namespace Quenn1599.VehiclesAddon
{
    // Needed to allow breaching/mining navigation
    [HarmonyPatch(typeof(FloatMenuMakerMap), "GotoLocationOption")]
    public static class FloatMenuMakerMap_GotoLocationOption_Patch
    {
        static void Postfix(IntVec3 clickCell, Pawn pawn, ref FloatMenuOption __result)
        {
            if (pawn is VehiclePawn vehicle)
            {

            }
        }
    }
}
