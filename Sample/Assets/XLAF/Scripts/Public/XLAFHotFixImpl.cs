using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLAF.Public
{
	/// <summary>
	/// XLAF hot fix config Base class, developer MUST inherit this class to create new class to enable hot fix 
	/// </summary>
	public class XLAFHotFixImpl
	{

		#region constructed function & initialization

		static XLAFHotFixImpl ()
		{

		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		//!!TODO!! 配置热更新地址、文件hash等各类 热更配置

		#region public variables

		/// <summary>
		/// use toLua hot fix or not
		/// </summary>
		public static bool useHotFix = true;

		#endregion

		#region virtual functions

		public virtual string hash ()
		{
			return "";
		}

		#endregion
	}
}