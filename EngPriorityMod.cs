using HarmonyLib;
using KMod;
using System.Reflection;

namespace ONI_EngineeringPriority
{
    public class EngPriorityMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Debug.Log("Engineering Priority: Loaded " + Assembly.GetExecutingAssembly().GetName().Version);
        }

    }
}
