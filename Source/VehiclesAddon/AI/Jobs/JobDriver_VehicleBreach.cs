using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;
using Vehicles;

namespace Quenn1599.VehiclesAddon
{
    class JobDriver_VehicleBreach : JobDriver
    {
        private Thing BreachTarget
        {
            get
            {
                return this.job.GetTarget(TargetIndex.A).Thing;
            }
        }
        private VehiclePawn vehicle;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

            Toil breach = new Toil();
            breach.tickAction = delegate ()
            {
                if (breach.actor is VehiclePawn vehicle) {
                    Thing target = this.BreachTarget;
                    if (this.ticksToNextHit-- <= 0)
                    {
                        IntVec3 position = BreachTarget.Position;
                        if (this.effecter == null)
                        {
                            this.effecter = EffecterDefOf.Mine.Spawn();
                        }
                        this.effecter.Trigger(breach.actor, BreachTarget);
                        
                        if (BreachTarget.def.useHitPoints)
                        {
                            Mineable mineable = BreachTarget as Mineable;
                            if (mineable == null || BreachTarget.HitPoints > DefaultDamagePerHit)
                            {
                                DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)DefaultDamagePerHit, 0f, -1f, breach.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
                                BreachTarget.TakeDamage(dinfo);
                            } else
                            {
                                mineable.Notify_TookMiningDamage(BreachTarget.HitPoints, breach.actor);
                                mineable.HitPoints = 0;
                                mineable.DestroyMined(breach.actor);
                            }
                        } else
                        {
                            // Destroy object if not using hp
                            BreachTarget.Destroy(DestroyMode.KillFinalize);
                        }
                        if (BreachTarget.Destroyed)
                        {
                            breach.actor.Map.mineStrikeManager.CheckStruckOre(position, BreachTarget.def, breach.actor);
                            if (this.pawn.Faction != Faction.OfPlayer)
                            {
                                List<Thing> thingList = position.GetThingList(this.Map);
                                for (int i = 0; i < thingList.Count; i++)
                                {
                                    thingList[i].SetForbidden(true, false);
                                }
                            }
                            this.ReadyForNextToil();
                            return;
                        }
                    }
                } else
                {
                    Log.Error("For some reason we're trying to do vehicle mining with a non-vehicle pawn. How did this happen??");
                    this.ReadyForNextToil();
                    return;
                }
            };
            breach.defaultCompleteMode = ToilCompleteMode.Never;
            if (BreachTarget.def.useHitPoints)
            {
                breach.WithProgressBar(TargetIndex.A, () => 1f - (float)this.BreachTarget.HitPoints / (float)this.BreachTarget.MaxHitPoints, false, -0.5f, false);
            }
            breach.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return breach;
            yield break;
        }

        private void RestTicksToNextHit()
        {
            this.ticksToNextHit = DefaultTicksBetweenHits;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.ticksToNextHit, "ticksToNextHit", 0, false);
        }

        private int ticksToNextHit = -1000;

        private Effecter effecter;

        public const int DefaultTicksBetweenHits = 10;

        public const int DefaultDamagePerHit = 100;
    }
}
