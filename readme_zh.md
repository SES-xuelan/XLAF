XLAF —— 一个简单的2D游戏/应用框架
======================
 [English Readme](https://github.com/SES-xuelan/XLAF/blob/master/readme.md)

XLAF(XueLan's Application Framework) 是一个简单的2D框架，包含完整的场景管理、声音管理、多语言切换、热更新(ToLua)等，开发者可以有更多的时间去开发游戏、应用的内部逻辑。<br />
一般用于只有一个Unity scene但是多个view(prefab)的游戏/应用，或者经过简单的修改，开发者也可以使用多个Unity Scene，更好地适应项目。

## 命名规则：
* 一般变量使用小驼峰 thisIsVariable；
* 方法/函数使用大驼峰 ThisIsFunction；
* 局部的变量、方法、函数等，使用下划线+驼峰  _thisIsPrivateVariable   _ThisIsPrivateFunction
(并不是所有的private都使用下划线)；
* unity中的gameobject使用大驼峰命名 SceneViewRoot；
* 一般情况下文件名就是类的名字（当然也有一个文件中有多个类的情况，这种情况一般就是以主要功能的那个类命名）；

## 使用的第三方插件
* SimpleJson
* iTween
* SharpZipLib
* ToLua
* Texture Packer

## 项目目录及相关约定
一般的，一个项目会是以下结构：<br />
注："【】"中的是目录说明<br />
注：标*的文件名不要修改，否则可能会出错

```
ProjectFolder
    |-- Assets
    |     |-- 3rd-party【C#写的第三方插件】
    |     |-- *Editor【编辑器的目录】
    |     |-- *Lua 【Lua脚本目录，热更新相关的，Editor的Lua插件会处理这个目录】
    |     |-- Materials【存放材质的目录】
    |     |-- *Plugins【原生插件目录  *.so  *.a等文件】
    |     |    |--Android【Android原生写的插件，一般是*.aar或者*.jar】
    |     |    `--IOS【iOS写的原生插件，一般是*.mm和*.h】
    |     |-- *Resources【Unity的资源目录】
    |     |    |--*Audios【存放音频，一般的m_代表music  s_代表sound】
    |     |    |--*ImageSheets【Texture Packer 打包的图集】
    |     |    `--*Views【存放各个界面的prefabs】
    |     |        |--*Popups【存放弹窗界面的prefabs】
    |     |        `--*Scenes【存放界面的prefabs】
    |     |-- *Scripts【存放C#脚本文件】
    |     |    |--*Views【存放各个界面的prefabs】
    |     |    |    |--*Popups【存放弹窗界面的prefabs】
    |     |    |    `--*Scenes【存放界面的prefabs】
    |     |    `--自定义的其他存放脚本的目录
    |     |-- *StreamingAssets【Unity默认目录，里面的文件不会被压缩，只读的】
    |     |-- *ToLua【ToLua插件目录】
    |     `-- *XLAF【本框架目录，里面的内容尽量不要修改；如果要修改，请确保你已经理解了内部结构和相互调用关系】
    |          |-- 3rd-party【第三方插件目录】
    |          |-- Editor【编辑器脚本/插件】
    |          |-- Plugins【原生插件目录  *.so  *.a等文件】
    |          |    |--Android【Android原生写的插件】
    |          |    `--IOS【iOS写的原生插件】
    |          |-- Resources【内置资源目录，目前只有后门界面】
    |          `-- Scripts【脚本目录】
    |              |-- Public【对外暴露的脚本，namespace:XLAF.Public】
    |              `-- Private【一般不对外暴露的脚本，namespace:XLAF.Private】
    `-- *Resources-Images【图片，如果放到Assets目录下，则会和图集冲突】
```




-----
## 其他要说明的
* 在命名时沿用了本人的一些习惯，所以一般的，在文档中和代码中提到的Scene都是指的包含一个界面的prefab，而不是unity的scene
* iTween我在原版的基础上做了以下修改：
    ```
    1. public enum EaseType  中增加了一项defaultType
    2. void CallBack (string callbackType) 函数中，增加了Action的callback
    ```


<br /><br /><br />

---
文档不断完善中……

