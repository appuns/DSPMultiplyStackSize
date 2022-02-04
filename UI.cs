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
using TranslationCommon.SimpleJSON;

namespace DSPMultiplyStackSize
{
    public class UI : MonoBehaviour
    {
        public static UIMessageBox messageBox;
        
        public static GameObject selectedItem;

        public static GameObject stackSizeButton;
        public static GameObject ItemPicker;
        public static GameObject selectedItemStackSizeInput;
        public static GameObject selectedItemName;
        public static GameObject stackSizeLabel;
        public static GameObject applyButton;
        public static GameObject resetButton;
        public static GameObject btnBox;
        public static GameObject numTextsBase;
        public static Text prefabNumText;
        public static Text[] numTexts = new Text[Main.maxIndex];
        public static GameObject selectedItemIcon;


        public static Color normalColor = new Color(0.4f, 0.8f, 1.0f, 0.8f);
        public static Color editedColor = new Color(1.0f, 0.6f, 0.0f, 0.9f);


        public static void Create()
        {
            ItemPicker = UIRoot.instance.uiGame.itemPicker.gameObject;
            //ItemPicker.name = "ItemPicker";
            prefabNumText = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/prefab-number-text").GetComponent<Text>(), ItemPicker.transform.Find("content").gameObject.transform);
            prefabNumText.fontSize = 14;
            prefabNumText.gameObject.GetComponent<Outline>().effectDistance = new Vector2(2, 2);
            prefabNumText.gameObject.GetComponent<Outline>().enabled = true;
            prefabNumText.gameObject.GetComponent<Shadow>().effectDistance = new Vector2(2, 2);
            prefabNumText.gameObject.GetComponent<Shadow>().enabled = true;
            prefabNumText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(46, 24);

            //LogManager.Logger.LogInfo("-------------------------------------------------------------prefabNumText");

            //選択アイテム用ベースオブジェクト
            numTextsBase = new GameObject();
            numTextsBase.transform.SetParent(ItemPicker.transform.Find("content").gameObject.transform, false);
            numTextsBase.name = "numTextsBase";
            numTextsBase.transform.localPosition = new Vector3(-273, 5, 0);
            numTextsBase.SetActive(false);


            //スタックサイズ表示用のTextを作成
            for (int i = 0; i < Main.maxIndex; i++)
            {
                //LogManager.Logger.LogInfo("-------------------------------------------------------------i : " + i);
                numTexts[i] = Instantiate(prefabNumText, numTextsBase.gameObject.transform);
                numTexts[i].name = "numTexts" + i;
                int row = i % 12;
                int col = i / 12;
                numTexts[i].rectTransform.anchoredPosition = new Vector2((float)(row * 46 - 5), (float)(col * -46 - 29));

            }

            //LogManager.Logger.LogInfo("-------------------------------------------------------------numTexts");
            //選択アイテム用ベースオブジェクト
            selectedItem = new GameObject();
            selectedItem.transform.SetParent(ItemPicker.transform, false);
            selectedItem.name = "selectedItem";
            selectedItem.transform.localPosition = new Vector3(-95, -5, 0);
            selectedItem.SetActive(false);

            //LogManager.Logger.LogInfo("-------------------------------------------------------------selectedItem");

            //スタックサイズ入力用フィールド
            selectedItemStackSizeInput = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Blueprint Browser/inspector-group/group-1/input-short-text"), selectedItem.transform);
            selectedItemStackSizeInput.name = "inputText"; ;
            selectedItemStackSizeInput.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
            selectedItemStackSizeInput.GetComponent<InputField>().characterLimit = 6;
            selectedItemStackSizeInput.transform.localPosition = new Vector3(440, -75, 0);
            selectedItemStackSizeInput.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 24);
            selectedItemStackSizeInput.SetActive(true);
            Destroy(selectedItemStackSizeInput.GetComponent<UIButton>());

            //LogManager.Logger.LogInfo("-------------------------------------------------------------selectedItemStackSizeInput");

            //選択アイテム画像
            selectedItemIcon = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Replicator Window/recipe-tree/center-icon")) as GameObject;
            selectedItemIcon.transform.SetParent(selectedItem.transform, false);
            selectedItemIcon.transform.localPosition = new Vector3(300, -75, 0);
            selectedItemIcon.transform.Find("place-text").GetComponentInChildren<Text>().text = "Selected Item".Translate();
            Destroy(selectedItemIcon.transform.Find("vline-m").gameObject);
            Destroy(selectedItemIcon.transform.Find("hline-0").gameObject);
            Destroy(selectedItemIcon.transform.Find("hline-1").gameObject);
            Destroy(selectedItemIcon.transform.Find("icon 2").gameObject);
            Destroy(selectedItemIcon.transform.Find("text 1").gameObject);
            Destroy(selectedItemIcon.transform.Find("text 2").gameObject);
            Destroy(selectedItemIcon.transform.Find("time-text").gameObject);
            Destroy(selectedItemIcon.transform.Find("time-text").gameObject);
            selectedItemIcon.SetActive(true);

            //LogManager.Logger.LogInfo("-------------------------------------------------------------selectedItemIcon");

            //選択アイテム名用テキスト
            selectedItemName = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Common Tools/Item Picker/title-text"), selectedItem.transform) as GameObject;
            selectedItemName.transform.localPosition = new Vector3(350, -47, 0);
            selectedItemName.SetActive(true);


            //LogManager.Logger.LogInfo("-------------------------------------------------------------selectedItemName");

            //「stacksize」ラベル
            stackSizeLabel = Instantiate(selectedItemIcon.transform.Find("place-text").gameObject, selectedItem.transform) as GameObject;
            stackSizeLabel.transform.localPosition = new Vector3(385, -87, 0);
            stackSizeLabel.GetComponent<Text>().text = "Stack Size".Translate();
            stackSizeLabel.SetActive(true);

            //「適応」ボタン
            applyButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Station Window/storage-box-0/popup-box/sd-option-button-0"), selectedItem.transform) as GameObject;
            applyButton.transform.localPosition = new Vector3(510, -85, 0);
            applyButton.name = "applyButton";
            applyButton.GetComponentInChildren<Text>().text = "Apply".Translate();
            applyButton.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 26);
            applyButton.GetComponent<Image>().color = new Color(0.240f, 0.55f, 0.65f, 0.7f);
            applyButton.SetActive(true);

            //「リセット」ボタン
            resetButton = Instantiate(applyButton, ItemPicker.transform) as GameObject;
            resetButton.transform.localPosition = new Vector3(545, -105, 0);
            resetButton.name = "resetButton";
            resetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 20);
            resetButton.GetComponentInChildren<Text>().text = "Reset".Translate();
            resetButton.GetComponent<Image>().color = new Color(1.0f, 0.68f, 0.45f, 0.7f);
            resetButton.SetActive(true);

            //「X」ボタン
            btnBox = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Inserter Window/panel-bg/btn-box"), ItemPicker.transform) as GameObject;
            btnBox.transform.localPosition = new Vector3(570, 0, 0);
            btnBox.name = "btnBox";
            btnBox.SetActive(true);

            //スタックサイズエディタボタンの作成
            GameObject titleText = GameObject.Find("UI Root/Overlay Canvas/In Game/Windows/Player Inventory/panel-bg/title-text");
            stackSizeButton = Instantiate(GameObject.Find("UI Root/Overlay Canvas/In Game/Game Menu/detail-func-group/dfunc-1"), titleText.transform) as GameObject;
            stackSizeButton.name = "stackSizeButton";
            stackSizeButton.transform.localPosition = new Vector3(114, -3, 0);
            stackSizeButton.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            stackSizeButton.GetComponent<UIButton>().tips.tipTitle = "Stack size editor".Translate();
            stackSizeButton.GetComponent<UIButton>().tips.tipText = "Click to enter Stack Size edit mode.".Translate();
            stackSizeButton.GetComponent<UIButton>().tips.corner = 4;
            stackSizeButton.GetComponent<UIButton>().tips.offset = new Vector2(0, 20);
            stackSizeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(27, 27);
            stackSizeButton.transform.Find("icon").GetComponent<RectTransform>().sizeDelta = new Vector2(15, 15);

            stackSizeButton.transform.Find("icon").GetComponent<Image>().sprite = Main.stackIcon;
            stackSizeButton.GetComponent<UIButton>().highlighted = true;
            stackSizeButton.SetActive(true);


            //イベントの追加
            stackSizeButton.GetComponent<Button>().onClick.AddListener(OnClickStackSizeButton);
            applyButton.GetComponent<Button>().onClick.AddListener(OnClickApplyButton);
            resetButton.GetComponent<Button>().onClick.AddListener(OnClickResetButton);
            btnBox.GetComponentInChildren<Button>().onClick.AddListener(OnClickCloseButton);

        }

        public static void OnClickStackSizeButton()
        {
            if (UIItemPicker.isOpened)
            {
                Main.stackSizeChangerMode = false;
                UIItemPicker.Close();
                return;
            }
            Main.stackSizeChangerMode = true;
            UIRoot.instance.uiGame.itemPicker._Open();
            //itemPicker.pickerTrans.anchoredPosition = pos;

        }

        public static void OnClickApplyButton()
        {
            //foreach (var item in LDB.items.dataArray)
            //{
            //    if (item != null && stacksize != item.StackSize)
            //    {
            //        if (item.ID == Patch.selectedProto.ID)
            //        {
            //            break;
            //        }
            //    }
            //}
            ItemProto itemProto = LDB.items.Select(Patch.selectedProto.ID);
            int stacksize = int.Parse(UI.selectedItemStackSizeInput.GetComponent<InputField>().text);

            if (itemProto.StackSize != stacksize)
            {
                itemProto.StackSize = stacksize;
                StorageComponent.itemStackCount[itemProto.ID] = stacksize;
                //Patch.selectedProto.StackSize = int.Parse(UI.selectedItemStackSizeInput.GetComponent<InputField>().text);

                //LDB.items.dataArray[Patch.selectedProto.ID].StackSize = int.Parse(UI.selectedItemStackSizeInput.GetComponent<InputField>().text);

                if (Main.stackSizeDictionary.ContainsKey(Patch.selectedProto.ID))
                {
                    Main.stackSizeDictionary[Patch.selectedProto.ID] = stacksize;
                }
                else
                {
                    Main.stackSizeDictionary.Add(Patch.selectedProto.ID, stacksize);
                }
                
                File.WriteAllText(Main.jsonFilePath, JSON.ToJson(Main.stackSizeDictionary));
                //selectedItem.SetActive(false);
                UI.selectedItemName.GetComponent<Text>().text = "";
                UI.selectedItemIcon.transform.Find("icon 1").gameObject.SetActive(false);
                UI.selectedItemStackSizeInput.GetComponent<InputField>().text = "";
                UI.refreshNumTexts();

            }
        }



        public static void OnClickResetButton()
        {
            messageBox = UIMessageBox.Show(
                "DSPMultiplyStackSize".Translate(),
                "Reset all stack sizes?".Translate(),
                "Cancel".Translate(),
                "Reset".Translate(),
                0,
                null,
                new UIMessageBox.Response(Reset)
            );

        }


        public static void OnClickCloseButton()
        {
            selectedItem.SetActive(false);
            btnBox.SetActive(false);
            resetButton.SetActive(false);
            UI.numTextsBase.SetActive(false);
            Main.stackSizeChangerMode = false;
            UIItemPicker.Close();
        }


        public static void refreshNumTexts()
        {
            for (int i = 0; i < Main.maxIndex; i++)
            {
                if (UIRoot.instance.uiGame.itemPicker.protoArray[i] != null)
                {
                    numTexts[i].text = UIRoot.instance.uiGame.itemPicker.protoArray[i].StackSize.ToString();
                    //numTexts[i].text = "99999";
                    numTexts[i].gameObject.SetActive(true);

                    if (Main.stackSizeDictionary.ContainsKey(UIRoot.instance.uiGame.itemPicker.protoArray[i].ID))
                    {
                        numTexts[i].color = editedColor;
                    }
                    else
                    {
                        numTexts[i].color = normalColor;
                    }
                }
                else
                {
                    numTexts[i].gameObject.SetActive(false);
                }
            }
        }

        public static void Reset()
        {
            Main.stackSizeDictionary.Clear();
            File.WriteAllText(Main.jsonFilePath, JSON.ToJson(Main.stackSizeDictionary));

            foreach (var item in LDB.items.dataArray)
            {
                int stacksize = Main.stackSizeDictionaryOrigin[item.ID];
                item.StackSize = stacksize;
                StorageComponent.itemStackCount[item.ID] = stacksize;
            }

            refreshNumTexts();

        }
    }
}
