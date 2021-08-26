using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Verse;
using RimWorld;
using Verse.AI;
using Vehicles;
using Vehicles.AI;
using SmashTools;

namespace Quenn1599.VehiclesAddon
{
    // TODO: Finish adapting logic for detecting what to give breach jobs for and when to use goto jobs
    public class JobGiver_VehicleBreach : ThinkNode_JobGiver
    {
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_VehicleBreach jobgiver = (JobGiver_VehicleBreach)base.DeepCopy(resolve);
            jobgiver.canMineMineables = this.canMineMineables;
            jobgiver.canMineNonMineables = this.canMineNonMineables;
            return jobgiver;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn is VehiclePawn vehicle)
            {
                CompVehicleBreacher breacher = vehicle.TryGetComp<CompVehicleBreacher>();
                if (breacher != null)
                {
                    IntVec3 intVec3 = vehicle.mindState.duty.focus.Cell;
                    if (vehicle.Position == intVec3)
                    {
                        // Destination reached
                        return null;
                    }

                    int num = GenRadial.NumCellsInRadius(2.9f);
                    int i = 0;
                    IntVec3 currentPos;
                    while (i < num)
                    {
                        currentPos = GenRadial.RadialPattern[i] + intVec3;
                        if (VehicleReachabilityUtility.CanReachVehicle(vehicle, currentPos, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.PassAllDestroyableThings))
                        {
                            PawnPath result;
                            // Threaded pathfinding
                            var cts = new CancellationTokenSource();
                            try
                            {
                                var tasks = new[]
                                {
                                    Task<(PawnPath path, bool found)>.Factory.StartNew(() => vehicle.Map.GetCachedMapComponent<VehicleMapping>()[vehicle.VehicleDef].VehiclePathFinder.FindVehiclePath(vehicle.Position, currentPos, TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false, false, false), cts.Token),cts.Token)
                                };
                                int taskIndex = Task.WaitAny(tasks, cts.Token);

                                if (tasks[taskIndex].Result.ToTuple().Item1 != null && !tasks[taskIndex].Result.ToTuple().Item1.Found && !tasks[taskIndex].Result.ToTuple().Item2)
                                {
                                    try
                                    {
                                        cts.Cancel();
                                        cts.Dispose();
                                        // No path :(
                                        return null;
                                    } catch (Exception ex)
                                    {
                                        Log.Error("Something has gone wrong with cancelling a pathfinding task.");
                                    }
                                }
                                result = tasks[0].Result.ToTuple().Item1;
                            } catch (AggregateException ex)
                            {
                                Log.Error("A cosmic entity has tampered with the threads. This is not good.");
                                cts.Cancel();
                                cts.Dispose();
                                return null;
                            }
                            IntVec3 cellBeforeBlocker;
                            Thing thing = result.FirstBlockingBuildingVehicle(out cellBeforeBlocker, vehicle);
                            
                        }
                        else
                        {
                            i++;
                        }
                        
                    }
                    return null;
                }
            }
        }

        private bool canMineMineables = true;
        private bool canMineNonMineables = true;
    }
}
