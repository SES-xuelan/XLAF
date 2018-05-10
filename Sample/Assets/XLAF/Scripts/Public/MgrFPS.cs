using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XLAF.Public
{
	/// <summary>
	/// debug use, show FPS, memory info
	/// </summary>
	public class MgrFPS : MonoBehaviour
	{
		#region private variables

		private static MgrFPS instance;
		private bool showFPS = true;
		private float fpsUpdateDelay = 0.2f;
		private float fpsTime = 0f;
		private int fps = 0;
		private int minFps;
		private int maxFps;
		private int fpsCacheCount = 50;
		private List<int> fpsList = new List<int> ();

		private float mKBSize = 1024.0f * 1024.0f;
		private float totalAllocatedMemory;
		private float totalReservedMemory;
		private float totalUnusedReservedMemory;

		#endregion

		#region constructed function & initialization

		static MgrFPS ()
		{
			instance = XLAFMain.XLAFGameObject.AddComponent<MgrFPS> ();
			instance.showFPS = false;
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public functions
		/// <summary>
		/// Shows the FPS & memory info for debug.
		/// </summary>
		public static void ShowFPS ()
		{
			instance.showFPS = true;
		}
		/// <summary>
		/// Hides the FPS & memory info.
		/// </summary>
		public static void HideFPS ()
		{
			instance.showFPS = false;
		}

		#endregion

		#region private functions

		Color GetGUIColor (int value)
		{
			if (value > 30f)
				return Color.green;
			else if (value > 15f)
				return Color.yellow;
			else
				return Color.red;
		}

		void AddFPS (int fpsValue)
		{
			if (fpsList.Count >= fpsCacheCount) {
				fpsList.RemoveAt (0);
			}
			fpsList.Add (fpsValue);
		}

		#endregion


		#region MonoBehaviour functions

		void Update ()
		{
			if (showFPS) {
				if (fpsTime + fpsUpdateDelay < Time.unscaledTime) {
					fpsTime = Time.unscaledTime;
					fps = Mathf.RoundToInt (1f / Time.unscaledDeltaTime);
					AddFPS (fps);
					minFps = fpsList.Min ();
					maxFps = fpsList.Max ();

					totalReservedMemory = (float)UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong () / mKBSize;
					totalAllocatedMemory = (float)UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong () / mKBSize;
					totalUnusedReservedMemory = (float)UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong () / mKBSize;
				}
			}
		}

		void OnGUI ()
		{
			if (showFPS) {
				GUILayout.BeginVertical ();

				//fps
				GUILayout.BeginHorizontal ();
				GUI.color = GetGUIColor (fps);
				GUILayout.Label ("fps: " + fps.ToString ("f0"));
				GUI.color = GetGUIColor (minFps);
				GUILayout.Label ("min: " + minFps.ToString ("f0"));
				GUI.color = GetGUIColor (maxFps);
				GUILayout.Label ("max: " + maxFps.ToString ("f0"));
				GUILayout.EndHorizontal ();

				//memory usage
				GUILayout.BeginHorizontal ();
				GUI.color = Color.red;
				GUILayout.Label ("reserved: " + totalReservedMemory.ToString ("f2") + "M");
				GUILayout.Label ("allocated: " + totalAllocatedMemory.ToString ("f2") + "M");
				GUILayout.Label ("unused: " + totalUnusedReservedMemory.ToString ("f2") + "M");
				GUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
			}
		}

		#endregion
	}
}