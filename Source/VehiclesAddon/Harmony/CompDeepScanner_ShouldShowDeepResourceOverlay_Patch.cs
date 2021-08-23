using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Quenn1599.VehiclesAddon
{
    // Messy transpiler patch, hopefully no one else is touching this method
    [HarmonyPatch(typeof(CompDeepScanner))]
    [HarmonyPatch("ShouldShowDeepResourceOverlay")]
    public static class CompDeepScanner_ShouldShowDeepResourceOverlay_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4_0)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                } else
                {
                    yield return instruction;
                }
            }
        }
    }
}
