﻿using System.Collections.Generic;
using System.Linq;
using BattleTech;
using CustomComponents;

namespace MechEngineer
{
    internal class CockpitHandler : IAdjustUpgradeDef, IAutoFixMechDef, IPreProcessor
    {
        internal static CockpitHandler Shared = new CockpitHandler();
        
        private readonly IdentityHelper identity;
        private readonly AutoFixMechDefHelper fixer;
        private readonly AdjustCompDefTonnageHelper reweighter;
        private readonly AdjustCompDefInvSizeHelper resizer;

        private CockpitHandler()
        {
            identity = Control.settings.AutoFixCockpitCategorizer;

            fixer = new AutoFixMechDefHelper(
                identity,
                Control.settings.AutoFixMechDefCockpitAdder
            );

            reweighter = new AdjustCompDefTonnageHelper(identity, Control.settings.AutoFixCockpitTonnageChange);
            resizer = new AdjustCompDefInvSizeHelper(identity, Control.settings.AutoFixCockpitSlotChange);
        }

        public void PreProcess(object target, Dictionary<string, object> values)
        {
            identity.PreProcess(target, values);
        }

        public void AdjustUpgradeDef(UpgradeDef upgradeDef)
        {
            reweighter.AdjustComponentDef(upgradeDef);
            resizer.AdjustComponentDef(upgradeDef);
        }

        public void AutoFixMechDef(MechDef mechDef, float originalTotalTonnage)
        {
            fixer.AutoFixMechDef(mechDef, originalTotalTonnage);
        }
    }
}