using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SMU.Utilities;

namespace MenuCore.Patches
{
    [HarmonyPatch(typeof(MenuSystem), nameof(MenuSystem.RequestSwitchScreen))]
    class MenuSystem_RequestSwitchScreen
    {
        private static bool Prefix(MenuSystem __instance, string screen, Action<MenuScreen> onswitch, bool waitUntilGameInstanceIsNull)
        {
            //if its a modded screen, we dont want to go through the base games menu system since it'll crap itself as the screen won't be found in the base game list
            if (screen.StartsWith("modded_")) 
            {
                SharedCoroutineStarter.StartCoroutine(RequestSwitchScreenDelayed(__instance, screen, onswitch, waitUntilGameInstanceIsNull));
                return false;
            }
            return true;
        }

        static IEnumerator RequestSwitchScreenDelayed(MenuSystem __instance, string screen, Action<MenuScreen> onSwitch = null, bool waitUntilGameInstanceIsNull = true)
        {
            if (waitUntilGameInstanceIsNull)
            {
                var gameInstance = UnityEngine.Object.FindObjectOfType<GameInstance>();
                yield return new WaitUntil(() => gameInstance == null);
                yield return Resources.UnloadUnusedAssets();
            }
            if (__instance.GetField<MenuSystem, bool>("isUpdating"))
            {
                __instance.GetField<MenuSystem, Queue<Action>>("queuedActions").Enqueue(delegate
                {
                    SwitchToScreen(__instance, screen, onSwitch);
                });
            }
            else
            {
                SwitchToScreen(__instance, screen, onSwitch);
            }
        }

        static void SwitchToScreen(MenuSystem __instance, string screen, Action<MenuScreen> onSwitched)
        {
            int focusLayer = __instance.GetField<MenuSystem, int>("focusLayer");

            List<MenuSystem.ScreenState> screenStack = __instance.GetField<MenuSystem, List<MenuSystem.ScreenState>>("screenStack");

            // base game code mostly
            for (int i = screenStack.Count - 1; i > -1; i--)
            {
                var state = screenStack[i];
                if (state.view && !state.isPopup)
                {
                    focusLayer = state.layer;
                    state.view = false;
                    screenStack[i] = state;
                    break;
                }
                if (state.view && state.isPopup && state.layer >= focusLayer)
                {
                    focusLayer = state.layer - 1;
                }
            }

            var screenState = default(MenuSystem.ScreenState);
            screenState.view = true;

            // options
            screenState.loaderIndex = 38;

            // means we dont have to worry about the items within the custom menu having a transition
            screenState.usesSlimeInTransition = true;
            screenState.usesSlimeOutTransition = true;

            // base game code
            screenState.id = screen;
            __instance.screenLoader.IncrementHolder(screenState.loaderIndex);
            screenState.layer = focusLayer;
            screenState.onswitch = onSwitched;
            screenStack.Add(screenState); 
            if (screenState.usesSlimeInTransition && !__instance.PlayingSlimeOutTransition)
            {
                __instance.PlaySlimeOutTransition();
            }
        }

    }
}
