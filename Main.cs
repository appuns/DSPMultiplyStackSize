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
using TranslationCommon.SimpleJSON;

namespace DSPMultiplyStackSize
{
    [BepInPlugin("Appun.DSP.plugin.MultiplyStackSize", "DSPMultiplyStackSize", "1.0.7")]

    public class Main : BaseUnityPlugin
    {
        public static bool StackSizeUpdated = false;

        public static Sprite stackIcon;

        public static ConfigEntry<int> configMultiplyStackSize;
        public static ConfigEntry<bool> editWarperSlotStackSize;


        public static bool stackSizeChangerMode = false;
        public static int maxIndex;
        
        public static Dictionary<int, int> stackSizeDictionaryOrigin = new Dictionary<int, int>();
        public static Dictionary<int, int> stackSizeDictionary = new Dictionary<int, int>();
        public static string PluginPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string jsonFilePath;


        public void Start()
        {


            configMultiplyStackSize = Config.Bind("General", "StackSizeMultiplier", 5, "Multiplies the stack size of all items.");
            editWarperSlotStackSize = Config.Bind("General", "changeWarperSlotStackSize", true, "change Mecha Warper Slot Stack Size ");

            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            //辞書ファイルの読み込み
            jsonFilePath = Path.Combine(PluginPath, "stackSizeDictionary.json");

            if (!File.Exists(jsonFilePath))
            {
                //LogManager.Logger.LogInfo("File not found" + jsonFilePath);
                File.WriteAllText(jsonFilePath, JSON.ToJson("{}"));
            }
            else
            {
                stackSizeDictionary = JSON.FromJson<Dictionary<int, int>>(File.ReadAllText(jsonFilePath));
                //LogManager.Logger.LogInfo("GridIndex dictionary load finish.");
            }

            //アイコンの読み込み
            LoadIcon();

            //アイテム選択用のUIを作成
            maxIndex = UIItemPicker.colCount * UIItemPicker.rowCount;
            UI.Create();

            //LogManager.Logger.LogInfo("-------------------------------------------------------------maxIndex : " + maxIndex);

        }


        public static void LoadIcon()
        {
            try
            {
                var assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DSPMultiplyStackSize.multiplystacksize"));
                if (assetBundle == null)
                {
                    LogManager.Logger.LogInfo("Icon loaded.");
                }
                else
                {
                    stackIcon = assetBundle.LoadAsset<Sprite>("StackSize");
                    assetBundle.Unload(false);
                }
            }
            catch (Exception e)
            {
                LogManager.Logger.LogInfo("e.Message " + e.Message);
                LogManager.Logger.LogInfo("e.StackTrace " + e.StackTrace);
            }
        }


    }



    public class LogManager
    {
        public static ManualLogSource Logger;
    }
}