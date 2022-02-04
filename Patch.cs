using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using UnityEngine.Rendering;
using Steamworks;
using rail;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]


namespace DSPMultiplyStackSize
{
    [HarmonyPatch]
    internal class Patch
    {

        public static ItemProto selectedProto;

        //アイテムをクリックしたら情報を表示
        [HarmonyPrefix, HarmonyPatch(typeof(UIItemPicker), "OnBoxMouseDown")]
        public static bool UIItemPicker_OnBoxMouseDowne_PrePatch(UIItemPicker __instance)
        {
            if (Main.stackSizeChangerMode)
            {
                if (__instance.hoveredIndex >= 0)
                {
                    selectedProto = __instance.protoArray[__instance.hoveredIndex];
                    if (selectedProto != null)
                    {
                        //UI.selectedItem.SetActive(true);
                        UI.selectedItemName.GetComponent<Text>().text = selectedProto.name;
                        UI.selectedItemIcon.transform.Find("icon 1").gameObject.SetActive(true);
                        UI.selectedItemIcon.transform.Find("icon 1").GetComponent<Image>().sprite = selectedProto.iconSprite;
                        UI.selectedItemStackSizeInput.GetComponent<InputField>().text = selectedProto.StackSize.ToString();
                        return false;
                    }
                }else
                {
                    //UI.selectedItem.SetActive(false);
                    UI.selectedItemName.GetComponent<Text>().text = "";
                    UI.selectedItemIcon.transform.Find("icon 1").gameObject.SetActive(false);
                    UI.selectedItemStackSizeInput.GetComponent<InputField>().text = "";
                }
            }
            return true;
        }

        //タブを切り替えたら選択アイテムをリセット
        [HarmonyPostfix, HarmonyPatch(typeof(UIItemPicker), "OnTypeButtonClick")]
        public static void UIItemPicker_OnTypeButtonClick_PostPatch(UIItemPicker __instance)
        {
            if (Main.stackSizeChangerMode)
            {
                //UI.selectedItem.SetActive(false);
                UI.selectedItemName.GetComponent<Text>().text = "";
                UI.selectedItemIcon.transform.Find("icon 1").gameObject.SetActive(false);
                UI.selectedItemStackSizeInput.GetComponent<InputField>().text = "";
                UI.refreshNumTexts();
            }
        }

        //スタックサイズを表示
        [HarmonyPostfix, HarmonyPatch(typeof(UIItemPicker), "_OnOpen")]
        public static void UIItemPicker_OnUpdate_Patch(UIItemPicker __instance)
        {
            if (Main.stackSizeChangerMode)
            {
                UI.numTextsBase.SetActive(true);
                UI.btnBox.SetActive(true);
                UI.resetButton.SetActive(true);
                UI.selectedItem.SetActive(true);
                UI.selectedItemName.GetComponent<Text>().text = "";
                UI.selectedItemIcon.transform.Find("icon 1").gameObject.SetActive(false);
                UI.selectedItemStackSizeInput.GetComponent<InputField>().text = "";
                UI.refreshNumTexts();
            }
            else
            {
                UI.selectedItem.SetActive(false);
                UI.numTextsBase.SetActive(false);
                UI.btnBox.SetActive(false);
                UI.resetButton.SetActive(false);
            }

        }

        //スタックサイズを表示
        [HarmonyPostfix, HarmonyPatch(typeof(UIItemPicker), "_OnClose")]
        public static void UIItemPicker_Close_Patch(UIItemPicker __instance)
        {
            Main.stackSizeChangerMode = false;
        }



        //起動後のスタックサイズ修正
        [HarmonyPostfix,HarmonyPatch(typeof(VFPreload), "InvokeOnLoadWorkEnded")]
        [HarmonyPriority(1)]

        public static void VFPreload_PreloadThread_Patch()
        {
            if (!Main.StackSizeUpdated)
            {
                LogManager.Logger.LogInfo("Apply Multiply Stack Size.");
                foreach (var item in LDB.items.dataArray)
                {
                    if (item != null)
                    {
                        int stacksize = item.StackSize * Main.configMultiplyStackSize.Value;
                        Main.stackSizeDictionaryOrigin.Add(item.ID, stacksize);
                        if (Main.stackSizeDictionary.ContainsKey(item.ID))
                        {
                            stacksize = Main.stackSizeDictionary[item.ID];
                        }
                        item.StackSize = stacksize;
                        StorageComponent.itemStackCount[item.ID] = stacksize;
                    }
                }
                Main.StackSizeUpdated = true;
            }
        }

        //メカのワーパースロットのスタックサイズ修正１
        [HarmonyPostfix,HarmonyPatch(typeof(Mecha), "Init")]
        public static void Mecha_Init_Patch(Mecha __instance)
        {
            if (Main.editWarperSlotStackSize.Value)
            {
                __instance.warpStorage.SetFilter(0, 1210, LDB.items.Select(1210).StackSize);
            }
        }

        //メカのワーパースロットのスタックサイズ修正２
        [HarmonyPostfix,HarmonyPatch(typeof(Mecha), "SetForNewGame")]
        public static void Mecha_SetForNewGame_Patch(Mecha __instance)
        {
            if (Main.editWarperSlotStackSize.Value)
            {
                __instance.warpStorage.SetFilter(0, 1210, LDB.items.Select(1210).StackSize);
            }
        }


    }
}
