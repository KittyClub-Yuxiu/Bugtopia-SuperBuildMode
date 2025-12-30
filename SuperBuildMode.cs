using BepInEx;
using HarmonyLib;
using UnityEngine;
using Peecub;

[BepInPlugin("com.yuxiu.bugtopiamods.superbuildmode", "Super Build Mode", "1.0.0")]
public class SuperBuildMode : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony("com.yuxiu.bugtopiamods.superbuildmode");
        harmony.PatchAll();
        Logger.LogInfo("超级建造模式启动！现在你可以随意重叠造景，甚至悬空放置造景！");
    }

    [HarmonyPatch(typeof(LandscapeSensors), "ChangeColor")]
    class Patch_LandscapeSensors_ChangeColor
    {
        private static bool Prefix(LandscapeSensors __instance, SpriteRenderer spriteRenderer)
        {
            AccessTools.Field(typeof(LandscapeSensors), "result").SetValue(__instance, true);

            if (spriteRenderer != null && RM.instance != null)
            {
                spriteRenderer.color = RM.instance.sensorTrueColor;
            }
            
            return false;
        }
    }
}
