# XLAF(XueLan's Application Framework) 文档

## 简介
XLAF是一个简单的2D游戏/应用框架，初衷是用于只有一个scene但是多个view的游戏/应用，经过简单的修改，就可以使用多个Scene了

## XLAF中的命名规则：
* 一般变量使用小驼峰 thisIsVariable；
* 方法/函数使用大驼峰 ThisIsFunction；
* 局部的变量、方法、函数等，使用下划线+驼峰  _thisIsPrivateVariable   _ThisIsPrivateFunction
(并不是所有的private都使用下划线)；
* unity中的gameobject使用大驼峰命名 SceneViewRoot；
* 一般情况下文件名就是类的名字（当然也有一个文件中有多个类的情况，这种情况一般就是以主要功能的那个类命名）；

## 使用的第三方插件
* SimpleJson
* iTween

## 项目目录及相关约定
一般的，一个项目会是以下结构：<br />
注："【】"中的是目录说明<br />
注：标*的文件名不要修改，否则会出错

```
Assets
|-- 3rd-party【C#写的第三方插件】
|-- *Editor【编辑器的目录】
|-- Images【美术原始图片】
|   |--Common【多个场景都用到的图片、能共用的】
|   `--View1【View1中用到的图片，此处的View1仅为示例】
|-- Materials【存放材质的目录】
|-- *Plugins【原生插件目录  *.so  *.a等文件】
|    |--Android【Android原生写的插件，一般是*.aar或者*.jar】
|    `--IOS【iOS写的原生插件，一般是*.mm和*.h】
|-- *Resources【Unity的资源目录】
|    |--*Audios【存放音频，一般的m_代表music  s_代表sound】
|    `--*Views【存放各个界面的prefabs】
|        |--*Dialogs【存放弹窗界面的prefabs】
|        `--*Scenes【存放界面的prefabs】
|-- *Scripts【存放C#脚本文件】
|    |--*Views【存放各个界面的prefabs】
|    |    |--*Dialogs【存放弹窗界面的prefabs】
|    |    `--*Scenes【存放界面的prefabs】
|    `--自定义的其他存放脚本的目录
|-- *StreamingAssets【Unity默认目录，里面的文件不会被压缩，只读的】
`-- *XLAF【本框架目录，里面的内容尽量不要修改】
```




-----
## 其他要说明的
* 一般的，在文档中提到的Scene都是指的包含一个界面的prefab，而不是unity的scene
* iTween我在原版的基础上做了以下修改：
    ```
    1. public enum EaseType  中增加了一项defaultType
    2. void CallBack (string callbackType) 函数中，增加了Action的callback
    ```


<br /><br /><br />

---
文档不断完善中……

