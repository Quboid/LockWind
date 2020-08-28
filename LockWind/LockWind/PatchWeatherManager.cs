using CitiesHarmony.API;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using System;
using System.Reflection;
using UnityEngine;

namespace LockWind
{
    [HarmonyPatch(typeof(WeatherManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class PatchWeatherManager
    {
        static void Postfix()
        {
            if (ModInfo.enabled)
            {
                Singleton<WeatherManager>.instance.m_windDirection = ModInfo.direction;
            }
        }
    }
}
