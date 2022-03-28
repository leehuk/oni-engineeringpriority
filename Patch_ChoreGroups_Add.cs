using HarmonyLib;
using System.Reflection;

namespace ONI_EngineeringPriority
{
    [HarmonyPatch(typeof(Database.ChoreGroups))]
    [HarmonyPatch("Add")]
    public static class ChoreGroups_Add
    {
        public static void Postfix(Database.ChoreGroups __instance, string id)
        {
            if (id == "Build")
            {
                var method = __instance.GetType().GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
                ChoreGroup cg = (ChoreGroup)method.Invoke(__instance, new object[] { "EngBuild", EngPriorityStrings.CHOREGROUP_ENGBUILD_NAME.ToString(), Db.Get().Attributes.Construction, "icon_errand_toggle", 2, true });
                cg.Name = EngPriorityStrings.CHOREGROUP_ENGBUILD_NAME;
                cg.description = EngPriorityStrings.CHOREGORUP_ENGBUILD_DESC;
            }
        }
    }
}
