﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace Quenn1599.VehiclesAddon
{
    public class CompProperties_VehicleDeepScanner : CompProperties_ScannerMineralsDeep
    {
        public CompProperties_VehicleDeepScanner()
        {
            this.compClass = typeof(CompVehicleDeepScanner);
        }

        public string roleKey;
    }
}