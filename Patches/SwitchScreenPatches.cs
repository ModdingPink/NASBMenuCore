using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using System.Reflection;
using TMPro;
using UnityEngine.UI;

namespace MenuCore
{
    public static class SwitchScreenPatches
    {

        [HarmonyPatch(typeof(MenuSystem), nameof(MenuSystem.RequestSwitchScreen)), HarmonyPrefix]
        private static bool switchscreen(MenuSystem __instance, string screen, Action<MenuScreen> onswitch, bool waitUntilGameInstanceIsNull)
        {
            //if its a modded screen, we dont want to go through the base games menu system since itll crap itself as the screen wont be found in the base game list
            if (screen.StartsWith("modded_")) 
            {
                __instance.StartCoroutine(RequestSwitchScreenDelayed(__instance, screen, onswitch, waitUntilGameInstanceIsNull));
                return false;
            }
            return true;
        }

        static IEnumerator RequestSwitchScreenDelayed(MenuSystem __instance, string screen, Action<MenuScreen> onswitch = null, bool waitUntilGameInstanceIsNull = true)
        {

            if (waitUntilGameInstanceIsNull)
            {
                GameInstance gameInstance = UnityEngine.Object.FindObjectOfType<GameInstance>();
                yield return new WaitUntil(() => gameInstance == null);
                yield return Resources.UnloadUnusedAssets();
            }
            if (__instance.GetField<MenuSystem, bool>("isUpdating"))
            {
                __instance.GetField<MenuSystem, Queue<Action>>("queuedActions").Enqueue(delegate
                {
                    SwitchToScreen(__instance, screen, onswitch);
                });
            }
            else
            {
                SwitchToScreen(__instance, screen, onswitch);
            }
            yield break; //same IEnumerator as the base games RequestSwitchScreenDelayed, but instead SwitchToScreen is our own SwitchToScreen
        }

        static private void SwitchToScreen(MenuSystem __instance, string screen, Action<MenuScreen> onSwitched)
        {
            int num = __instance.GetField<MenuSystem, int>("focusLayer");

            List<MenuSystem.ScreenState> screenStack = __instance.GetField<MenuSystem, List<MenuSystem.ScreenState>>("screenStack");

            for (int i = screenStack.Count - 1; i > -1; i--)
            {
                MenuSystem.ScreenState screenState = screenStack[i];
                if (screenState.view && !screenState.isPopup)
                {
                    num = screenState.layer;
                    screenState.view = false;
                    screenStack[i] = screenState;
                    break;
                }
                if (screenState.view && screenState.isPopup && screenState.layer >= num)
                {
                    num = screenState.layer - 1;
                }
            }//base game code mostly

            MenuSystem.ScreenState screenState2 = default(MenuSystem.ScreenState);
            screenState2.view = true;

            screenState2.loaderIndex = 38;
            //options

            screenState2.usesSlimeInTransition = true;
            screenState2.usesSlimeOutTransition = true;
            //means we dont have to worry about the items within the custom menu having a transition

            screenState2.id = screen;
            __instance.screenLoader.IncrementHolder(screenState2.loaderIndex);
            screenState2.layer = num;
            screenState2.onswitch = onSwitched;
            screenStack.Add(screenState2); 
            if (screenState2.usesSlimeInTransition && !__instance.PlayingSlimeOutTransition)
            {
                __instance.PlaySlimeOutTransition();
            }
            //base game code
        }

    }
}
