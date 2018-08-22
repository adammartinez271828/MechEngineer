﻿using System;
using System.Collections.Generic;
using BattleTech;
using BattleTech.UI;
using Harmony;

namespace MechEngineer
{
    [HarmonyPatch(typeof(MechLabPanel), "GetNonFieldableErrorString")]
    public static class MechLabPanel_GetNonFieldableErrorString_Patch
    {
        private static bool _isSimGame;

        public static void Prefix(MechLabPanel __instance)
        {
            _isSimGame = __instance.IsSimGame;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                AccessTools.Method(typeof(MechValidationRules), "GetValidationErrors"),
                AccessTools.Method(typeof(MechLabPanel_GetNonFieldableErrorString_Patch), "GetValidationErrors")
            );
        }

        public static List<Localize.Text> GetValidationErrors(Dictionary<MechValidationType, List<Localize.Text>> errorMessages, List<MechValidationType> validationTypes)
        {
            try
            {
                if (_isSimGame)
                {
                    validationTypes.Add(MechValidationType.InvalidInventorySlots);
                }
            }
            catch (Exception e)
            {
                Control.mod.Logger.LogError(e);
            }

            return MechValidationRules.GetValidationErrors(errorMessages, validationTypes);
        }
    }
}