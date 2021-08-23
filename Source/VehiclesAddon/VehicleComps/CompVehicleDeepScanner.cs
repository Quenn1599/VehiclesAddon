using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

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
    }
}
