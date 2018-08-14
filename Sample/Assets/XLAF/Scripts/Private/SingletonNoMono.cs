using System;
using System.Threading;

namespace XLAF.Private
{
	/// <summary>
	/// 单例基类 不继承MonoBehaviour 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Singleton<T> where T : class, new()
	{
		private static T s_singleton = null;
		private static object s_objectLock = new object ();


		public static T Instance {
			get {
				if (null == s_singleton) {
					object obj;
					Monitor.Enter (obj = s_objectLock);//加锁防止多线程创建单例
					try {
						if (s_singleton == null) {
							s_singleton = (default(T) == null) ? Activator.CreateInstance<T> () : default(T);
						}
					} finally {
						Monitor.Exit (obj);
					}
				}
				return s_singleton;
			}
		}
	}
}