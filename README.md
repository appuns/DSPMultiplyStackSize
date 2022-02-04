# Multiply Stack Size
Multiply stack sizes of all items. This work with also modded items. Allow to change mecha space warper slot stack size.
MODで追加された物を含む全てのアイテムのスタックサイズを変更します。メカの空間湾曲器(space warper)スロットのスタックサイズも変更できるようになりました。

## Features　特徴
Multiply stack sizes of all items. This work with also modded items. Allow to change mecha waper slot stack size.
You can change the settings in the config file.steamapps/common/Dyson Sphere Program/config/Appun.DSP.plugin.MultiplyStacSize.cfg
  1.Stack Size Multiplier (StackSizeMultiplier:default 5)
  2.change Mecha Space Warper Slot Stack Size (changeWarperSlotStackSize:default on)

コンフィグファイルで設定を変更することができます。steamapps/common/Dyson Sphere Program/config/Appun.DSP.plugin.MultiplyStacSize.cfg
　・スタックサイズの倍率（StackSizeMultiplier:デフォルト5）
　・メカの空間湾曲器(warper)スロットのスタックサイズも変更するか(changeWarperSlotStackSize:デフォルトオン）

## How to install　インストール方法
1. Install BepInEx
   BepInExをインストールします。
2. Drag DSPMultiplyStacSize.dll into steamapps/common/Dyson Sphere Program/BepInEx/plugins
   DSPMultiplyStacSize.dllをsteamapps/common/Dyson Sphere Program/BepInEx/pluginsに配置します。

## Contact 問い合わせ先
If you have any problems or suggestions, please contact DISCORD MSP Modding server **Appun#8284**.
不具合、改善案などありましたら、DISCORD「DysonSphereProgram_Jp」サーバー**Appun#8284**までお願いします。


## Change Log
1.0.6 reduce the load 負荷を軽減しました。
1.0.5 allow to change mecha waper slot stack size. メカの空間湾曲器(space warper)スロットのスタックサイズも変更できるようになりました。
1.0.4 fixed applying again if loading fails. 起動直後のロードに失敗すると、再度乗算していたバグを修正しました。
1.0.3 now this work also modded items.　MODのアイテムにきちんと適応されるようになりました。
1.0.2 I fixed a fatal bug. this dont work modded items now.　スタックサイズが変更されないという致命的なバグを解決しました。まだMODアイテムには適応されません。
1.0.1 This work with also modded items.but didt work.  MODのアイテムにも適応したつもりがされていませんでした。(