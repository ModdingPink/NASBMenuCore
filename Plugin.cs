using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MenuCore.Patches;
using System.Reflection;

namespace MenuCore
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.steven.nasb.moddingutils")]
    class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            // Init logs and patches.
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            MainOptionsScreen_MenuOpen.MenuCompleteAction += CustomOptionsMenuHandler.CreateInitialSelector;
            MainOptionsScreen_MenuOpen.MenuCompleteAction += CustomOptionsMenuHandler.MenuHasChanged;
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}

