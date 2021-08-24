using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Vehicles;

namespace Quenn1599.VehiclesAddon
{
    public class CompVehicleDeepScanner : CompDeepScanner
    {
        public new CompProperties_VehicleDeepScanner Props
        {
            get
            {
                return this.props as CompProperties_VehicleDeepScanner;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.CanUseNow && this.parent is VehiclePawn vehicle)
            {
                VehicleHandler handler = vehicle.handlers.Find(x => x.role.key == this.Props.roleKey);
                if (handler != null)
                {
                    if (handler.handlers.Count > 0)
                    {
                        Operate(handler.handlers.InnerListForReading);
                    }
                }
            }
        }

        public void Operate(List<Pawn> crew)
        {
            if (!this.CanUseNow)
            {
                Log.Error("Used while CanUseNow is false.");
            }
            this.lastScanTick = (float)Find.TickManager.TicksGame;
            this.lastUserSpeed = 1f;
            if (this.Props.scanSpeedStat != null)
            {
                this.lastUserSpeed = 0f;
                foreach(Pawn p in crew)
                {
                    this.lastUserSpeed += p.GetStatValue(this.Props.scanSpeedStat, true);
                }
            }
            this.daysWorkingSinceLastFinding += this.lastUserSpeed / 60000f;
            if (this.TickDoesFind(this.lastUserSpeed))
            {
                if (this.parent is Pawn vehiclepawn)
                {
                    this.DoFind(vehiclepawn);
                    this.daysWorkingSinceLastFinding = 0f;
                }
                else
                {
                    Log.Error("For some reason a vehicle deep scanner is not correctly attached to a vehicle pawn.");
                }
            }
        }
    }
}
