XLAF —— A sample 2D game/app framework
====================================================
 [中文说明](https://github.com/SES-xuelan/XLAF/blob/master/readme_zh.md)

XLAF(XueLan's Application Framework) is a simple 2D framework, contains scene manager, audio manager, multi language manager and so on. Developers have more time to develop their games or apps.<br />
XLAF often use to develop games or apps for only one unity scene but many prefabs(views), developers can modify  code to support mutilple unity scenes.

## Naming Conventions
* Variables => camelCase. such as thisIsVariable.
* Functions => PascalCase. such as  ThisIsFunction.
* Local variables or functions => underline+camelCase/PascalCase. such as  _thisIsPrivateVariable   _ThisIsPrivateFunction
(not all private variables/functions start with underline ).
* gameobjects in unity =>PascalCase. such as SceneViewRoot.
* Normally class name is file name, if a file contains multi class, the file name is the primary class name.

## Used third party plugins
* SimpleJson
* iTween
* ToLua

## Project file list & illustration
Normally a project structure like this:<br />
note 1: "【】" include folder illustration.<br />
note 2: do NOT change name for marked star "\*" folders, or project will get errors.

```
Assets
|-- 3rd-party【plugins written by C#】
|-- *Editor【editor folder, unity built-in】
|-- Images【images】
|   |--Common【common images/many prefab use the same image】
|   `--View1【images in "View1"("View1" is an example)】
|-- *Lua 【Lua script folder, often used to do hotfix,Lua plugins in  Editor may handle this folder】
|-- Materials【materials】
|-- *Plugins【Native code plugins, such as   *.so  *.a files】
|    |--Android【Android native code, usually *.aar or *.jar】
|    `--IOS【iOS native code, usually *.mm & *.h】
|-- *Resources【Unity resources folder】
|    |--*Audios【Audios, start with "m_" means music, "s_" means sound】
|    `--*Views【Prefabs】
|        |--*Dialogs【Dialog prefabs】
|        `--*Scenes【Scene prefabs】
|-- *Scripts【C# scripts】
|    |--*Views
|    |    |--*Dialogs【Scripts for dialog prefabs】
|    |    `--*Scenes【Scripts for scene prefabs】
|    `--Any other C# code folders
|-- *StreamingAssets【Unity default folder, read only, not compressed】
|-- *ToLua【ToLua plugin folder】
`-- *XLAF【If you want to modify code, ensure you know the relationship with each other】
     |-- 3rd-party【The third party plugin】
     |-- Editor【editor folder, unity built-in】
     |-- Plugins【Native code plugins, such as   *.so  *.a files】
     |    |--Android【Android native code, usually *.aar or *.jar】
     |    `--IOS【iOS native code, usually *.mm & *.h】
     |-- Resources【Unity resources folder in XLAF, only backdoor resources currently】
     `-- Scripts
         |-- Public【Developer often use, namespace is XLAF.Public】
         `-- Private【XLAF private scripts, ONLY use for XLAF, namespace is XLAF.Private】
```


-----
## Others
* At the beginning, when I wrote this framework, I follow the previous habits, so in document "scene" means a prefab contains view, not unity scene.
* iTween plugin, the following modifications are made:
    ```
    1. `public enum EaseType`   add   `defaultType`
    2. function `void CallBack (string callbackType)`  add callback with `Action`
    ```


<br /><br /><br />

---
Continuous improvement ...

