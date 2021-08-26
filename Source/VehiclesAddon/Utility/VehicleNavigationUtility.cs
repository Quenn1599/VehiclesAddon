using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Vehicles;
using Verse.AI;

namespace Quenn1599.VehiclesAddon
{
    public static class VehicleNavigationUtility
    {
        // TODO: Finish functionality: correctly account for vehicle rotation and predicting vehicle hitbox along path to figure out which blocking Thing will be eligible for breaching by adjacent vehicle
        public static Thing FirstBlockingBuildingVehicle(this PawnPath path, out IntVec3 cellBefore, VehiclePawn vehicle)
        {
            if (!path.Found)
            {
                cellBefore = IntVec3.Invalid;
                return null;
            }
            List<IntVec3> nodesReversed = path.NodesReversed;
            if (nodesReversed.Count == 1)
            {
                cellBefore = nodesReversed[0];
                return null;
            }
            Building building = null;
            IntVec3 intVec = IntVec3.Invalid;
            for (int i = nodesReversed.Count - 2; i >= 0; i--)
            {
                // Building edifice = nodesReversed
            }
        }
        // TODO: See above TODO note, this function is merely for calculating the hitbox along the path and returning an enumerable of all of the blocking things
        public static IEnumerable<Thing> AllBlockingThings(VehiclePawn vehicle, IntVec3 center, Rot4 rot)
        {
            List<IntVec3> rectCells = CellRect.CenteredOn(center,vehicle.def.size.x,vehicle.def.size.z).Where
        }
    }
}
