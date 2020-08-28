using CitiesHarmony.API;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.Reflection;
using UnityEngine;

namespace LockWind
{
    public class ModInfo : LoadingExtensionBase, IUserMod
    {
        public string Name => "LockWind";
        public string Description => "Lock the wind's direction";
        public const string settingsFileName = "LockWind";

        public static SavedBool enabled = new SavedBool("enabled", settingsFileName, false, true);
        public static SavedFloat direction = new SavedFloat("direction", settingsFileName, 0f, true);

        public ModInfo()
        {
            try
            {
                // Creating setting file
                if (GameSettings.FindSettingsFileByName(settingsFileName) == null)
                {
                    GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = settingsFileName } });
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Could not load/create the setting file.\n{e}");
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (!(mode == LoadMode.LoadGame || mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario || mode == LoadMode.LoadScenario || mode == LoadMode.NewMap || mode == LoadMode.LoadMap))
            {
                return;
            }

            InstallMod();
        }

        public override void OnLevelUnloading()
        {
            UninstallMod();
        }

        public static void InstallMod()
        {
            Debug.Log($"Installing LockWind");
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public static void UninstallMod()
        {
            Debug.Log($"Uninstalling LockWind");
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup(Name);

            UICheckBox checkBox = (UICheckBox)group.AddCheckbox("Enabled", enabled.value, (b) =>
            {
                enabled.value = b;
            });

            group.AddSpace(10);

            UITextField textField = (UITextField)group.AddTextfield("Direction (0 - 360)", direction.ToString(), (t) =>
            {
                direction.value = Mathf.Clamp(float.Parse(t), 0f, 360f);
            });
        }

        internal static Assembly GetAssembly(string name)
        {
            Assembly assembly = null;
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly a in plugin.GetAssemblies())
                {
                    if (plugin.isEnabled && a.GetName().Name.ToLower() == name)
                    {
                        //Debug.Log($"Assembly {name} found");
                        assembly = a;
                        break;
                    }
                }
            }
            return assembly;
        }

        public void OnEnabled()
        {
            if (LoadingManager.exists && LoadingManager.instance.m_loadingComplete)
            {
                InstallMod();
            }
        }

        public void OnDisabled()
        {
            if (LoadingManager.exists && LoadingManager.instance.m_loadingComplete)
            {
                UninstallMod();
            }
        }
    }
}
