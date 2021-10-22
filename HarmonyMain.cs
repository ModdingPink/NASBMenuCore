using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuCore
{
    [BepInPlugin("pink-menucore", "MenuCore", "0.0.1")]
    class MenuCore : BaseUnityPlugin
    {
        static internal Harmony harmony = new Harmony(nameof(MenuCore));
        internal static MenuCore Instance;


        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            // Init logs and patches.
            harmony.PatchAll(typeof(patches));
            harmony.PatchAll(typeof(SwitchScreenPatches));
            harmony.PatchAll(typeof(CreateMenuCustomButton));
            harmony.PatchAll(typeof(CustomMenuPatches));

            CustomMenuPatches.menuCompleteAction += CustomOptionMenuHandler.createInitialSelector;
            CustomMenuPatches.menuCompleteAction += CustomOptionMenuHandler.menuHasChanged;

        }



        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion


        public static class patches 
        {

           

            

            

        }
    }
}

