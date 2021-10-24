using HarmonyLib;
using MenuCore.Behaviours;
using Nick;
using SMU.Reflection;
using TMPro;

namespace MenuCore.Patches
{
    [HarmonyPatch(typeof(MenuUpdateFloat), nameof(MenuUpdateFloat.ConsumeInput))]
    class MenuUpdateFloat_ConsumeInput
    {
        private static void Postfix(MenuUpdateFloat __instance, MenuInputEvent ev, bool __result)
        {
            if (__instance.name.StartsWith("moddedSlider_") && __result)
            {
                var customSliderObj = __instance.GetComponent<CustomSlider>();
                var val = __instance.GetField<MenuUpdateFloat, MenuValueSource<float>.Tag>("srcTag").Value;
                customSliderObj.SetValue(val);
                __instance.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().text = val.ToString();
                __instance.transform.Find("SelectionSound").GetComponent<ButtonSFXTrigger>().PlaySound();
            }
            else if (__instance.name.StartsWith("moddedSelector_") && __result)
            {
                var val = __instance.GetField<MenuUpdateFloat, MenuValueSource<float>.Tag>("srcTag").Value;
                var customSelectorObj = __instance.GetComponent<CustomSelector>();
                __instance.InvokeMethod<MenuUpdateFloat>("SetValue", 2);
                var newItem = customSelectorObj.SetValue(val);

                __instance.transform.Find("Button").Find("textvalue").GetComponent<TextMeshProUGUI>().text = customSelectorObj.prefix + newItem;

                __instance.transform.Find("SelectionSound").GetComponent<ButtonSFXTrigger>().PlaySound();
            }
        }
    }
}
