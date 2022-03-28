using HarmonyLib;
using System;
using System.Reflection;

namespace ONI_EngineeringPriority
{
    [HarmonyPatch(typeof(Constructable))]
    [HarmonyPatch("PlaceDiggables")]
    class Patch_Constructable_PlaceDiggables
    {
        public static void Postfix(Constructable __instance)
        {
            Debug.Log("PlaceDiggables(): Start");
            var field = __instance.GetType().GetField("buildChore", BindingFlags.NonPublic | BindingFlags.Instance);
            if(field == null)
            {
                Debug.Log("Engineering Priority: PlaceDiggables(): No buildChore field(?)");
                return;
            }

            Chore buildChore = (Chore)field.GetValue(__instance);

            // We need an active buildChore which has the "Build" type and a non-empty required skill perk on the constructable object
            // This chore gets cancelled and replaced with one using the "EngBuild" type
            if(buildChore != null && !string.IsNullOrEmpty(__instance.requiredSkillPerk) && buildChore.choreType == Db.Get().ChoreTypes.Build)
            {
                buildChore.Cancel("EngPriority Override");
                field.SetValue(__instance, null);

                var ubshandle = __instance.GetType().GetMethod("UpdateBuildState", BindingFlags.NonPublic | BindingFlags.Instance);
                Action<Chore> ubsmethod = (Action<Chore>)ubshandle.CreateDelegate(typeof(Action<Chore>), __instance);

                buildChore = new WorkChore<Constructable>(Db.Get().ChoreTypes.Get("EngBuild"), __instance, null, true, new Action<Chore>(ubsmethod), new Action<Chore>(ubsmethod), new Action<Chore>(ubsmethod), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
                field.SetValue(__instance, buildChore);

                ubshandle.Invoke(__instance, new object[] { buildChore });
            }
        }
    }
}
