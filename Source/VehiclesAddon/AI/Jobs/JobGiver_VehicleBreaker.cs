using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Vehicles;
using Verse.AI;
using Vehicles.AI;

namespace Quenn1599.VehiclesAddon
{
    class JobGiver_VehicleBreaker : ThinkNode_JobGiver
    {
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_VehicleBreaker job = (JobGiver_VehicleBreaker)base.DeepCopy(resolve);
			job.canMineMineables = this.canMineMineables;
			job.canMineNonMineables = this.canMineNonMineables;
			return job;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.drafter.Drafted = true;

			if (pawn is VehiclePawn vehicle)
            {
				IntVec3 cell = pawn.mindState.duty.focus.Cell;
				

			}
			return null;




			
			if (intVec.IsValid && (float)intVec.DistanceToSquared(pawn.Position) < 100f && intVec.GetRoom(pawn.Map) == pawn.GetRoom(RegionType.Set_All) && intVec.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
			{
				pawn.GetLord().Notify_ReachedDutyLocation(pawn);
				return null;
			}
			if (!intVec.IsValid)
			{
				IAttackTarget attackTarget;
				if (!(from x in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
					  where !x.ThreatDisabled(pawn) && x.Thing.Faction == Faction.OfPlayer && pawn.CanReach(x.Thing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings)
					  select x).TryRandomElement(out attackTarget))
				{
					return null;
				}
				intVec = attackTarget.Thing.Position;
			}
			if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings))
			{
				return null;
			}
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false), PathEndMode.OnCell, null))
			{
				IntVec3 cellBeforeBlocker;
				Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
				if (thing != null)
				{
					Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, this.canMineMineables, this.canMineNonMineables);
					if (job != null)
					{
						return job;
					}
				}
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		private bool canMineMineables = true;

		private bool canMineNonMineables = true;

		private const float ReachDestDist = 10f;

		private const int CheckOverrideInterval = 500;
	}
}
