using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using Vehicles;
using Verse.AI;

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
                CompVehicleBreacher breacher = vehicle.TryGetComp<CompVehicleBreacher>();
                if (breacher != null) // Maybe have an activator check?
                {
                    // Change result for alternative navigation
                    __result = new FloatMenuOption("GoHere".Translate(), delegate ()
                    {
                        vehicle.mindState.duty = new PawnDuty(DutyDefOf.VehicleBreach, clickCell);
                    }, MenuOptionPriority.GoHere, null, null, 0f, null)
                    {
                        autoTakeable = true,
                        autoTakeablePriority = 10f
                    };
                }
            }
        }
    }
}
