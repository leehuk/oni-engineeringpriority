using HarmonyLib;
using System.Reflection;

namespace ONI_EngineeringPriority
{
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    class Patch_Db_Initialize
    {
        public static void Postfix(Db __instance)
        {
            var method = __instance.ChoreTypes.GetType().GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            ChoreType ct = (ChoreType)method.Invoke(__instance.ChoreTypes, new object[] {
                    "EngBuild", new string[] { "EngBuild" }, "", new string[0], EngPriorityStrings.CHORETYPE_ENGBUILD_NAME.ToString(), EngPriorityStrings.CHORETYPE_ENGBUILD_STATUS.ToString(), EngPriorityStrings.CHORETYPE_ENGBUILD_TOOLTIP.ToString(), true, -1, null
            });

            ct.Name = EngPriorityStrings.CHORETYPE_ENGBUILD_NAME;
            ct.statusItem.Name = EngPriorityStrings.CHORETYPE_ENGBUILD_STATUS;
            ct.statusItem.tooltipText = EngPriorityStrings.CHORETYPE_ENGBUILD_TOOLTIP;

            // The ChoreTypes() constructor allocates these with gaps of 100, so pick 8 down from Build for no particular reason
            ct.interruptPriority = __instance.ChoreTypes.Build.interruptPriority - 8;
        }
    }
}