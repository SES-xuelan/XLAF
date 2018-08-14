/** 
****************************************************************************
 *Copyright(C) 2017 by Chess 
 *All rights reserved. 
 *FileName:     SingletonForMono.cs 
 *Author:       黄建 
 *Version:      1.0 
 *UnityVersion：5.4.2f2 
 *Date:         2017-01-10 
 *Description:    Unity单例脚本
 *History: 
 ****************************************************************************
*/
using UnityEngine;
using System.Collections;
using System;

namespace XLAF.Private
{
	/// <summary>
	/// Unity的脚本单例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SingletonForMono<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T m_Instance;
		//声明全局单例类自身类型的静态私有成员变量，作为单例对象使用。
		public static T Instance {
        //提供获取单例对象的类的静态方法。任何对单例对象的访问都要通过该方法来获取。
			get {
				return m_Instance;
			}
			set { m_Instance = value; }
		}

		public static T2 InstanceWithType<T2> ()
		{
			if (Instance != null)
				return (T2)Convert.ChangeType (Instance, typeof(T2));
			return default(T2);
		}

		void Awake ()
		{
			m_Instance = this.GetComponent<T> ();
			OnSingletonAwake ();
		}

		protected virtual void OnSingletonAwake ()
		{
		}

		protected virtual void OnSingletonDestroy ()
		{
		}

		void OnDestroy ()
		{
			OnSingletonDestroy ();
			m_Instance = null;
		}
		//public static T Instance
		//{
		//    //提供获取单例对象的类的静态方法。任何对单例对象的访问都要通过该方法来获取。
		//    get
		//    {
		//        if (null == m_Instance)
		//        {
		//            //寻找是否存在当前单例类对象
		//            GameObject go = GameObject.Find(typeof(T).Name);
		//            //不存在的话
		//            if (go == null)
		//            {
		//                //new一个并添加一个单例脚本
		//                go = new GameObject();
		//                m_Instance = go.AddComponent<T>();
		//            }
		//            else
		//            {
		//                if (go.GetComponent<T>() == null)
		//                {
		//                    go.AddComponent<T>();
		//                }
		//                m_Instance = go.GetComponent<T>();
		//            }
		//            //在切换场景的时候不要释放这个对象
		//            DontDestroyOnLoad(go);
		//        }
		//        return m_Instance;
		//    }
		//    set { m_Instance = value; }
		//}
	}
}