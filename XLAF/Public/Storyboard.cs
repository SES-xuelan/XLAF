using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XLAF.Public
{
    public class Storyboard : MonoBehaviour
    {
        /*
         运行的顺序是从上到下的↓
         
		CreatScene 加载完界面，还未播放动画（只有界面加载的时候，才会触发；读取缓存界面不会触发）
		WillEnterScene 加载完毕scene，即将播放过渡动画
		EnterScene 播放完毕过渡动画

		WillExitScene 即将播放退出界面的动画
		ExitScene 播放完退出界面的动画
		DestoryScene 销毁界面前触发
        
        */
        public virtual void CreatScene (object obj)
        {
        }

        public virtual void WillEnterScene (object obj)
        {
        }

        public virtual void EnterScene (object obj)
        {
        }

        public virtual void WillExitScene ()
        {
        }

        public virtual void ExitScene ()
        {
        }

        public virtual void DestoryScene ()
        {
        }

        public virtual void OverlayBegan (object obj)
        {
        }

        public virtual void OverlayEnded (object obj)
        {
        }

        public virtual void AndroidGoBack ()
        {
        }

    }
}
