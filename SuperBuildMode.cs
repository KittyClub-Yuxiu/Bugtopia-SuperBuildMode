using BepInEx;
using HarmonyLib;
using Micosmo.SensorToolkit;
using System.Collections.Generic;
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
        private static void Postfix(LandscapeSensors __instance, SpriteRenderer spriteRenderer)
        {
            var negativeSensorsField = AccessTools.Field(typeof(LandscapeSensors), "negativeSensors");
            var resultField = AccessTools.Field(typeof(LandscapeSensors), "result");

            List<Sensor> negativeSensors = (List<Sensor>)negativeSensorsField.GetValue(__instance);
            bool originalResult = (bool)resultField.GetValue(__instance);

            if (originalResult)
            {
                return;
            }

            bool preservedCheckPassed = true;
            if (negativeSensors != null)
            {
                foreach (var sensor in negativeSensors)
                {
                    if (sensor.GetDetections(null).Count != 0)
                    {
                        preservedCheckPassed = false;
                        break;
                    }
                }
            }

            if (preservedCheckPassed)
            {
                resultField.SetValue(__instance, true);

                if (spriteRenderer != null && RM.instance != null)
                {
                    spriteRenderer.color = RM.instance.sensorTrueColor;
                }
            }
        }
    }
}
