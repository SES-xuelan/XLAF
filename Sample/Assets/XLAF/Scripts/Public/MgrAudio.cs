using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace XLAF.Public
{
	/// <summary>
	/// Audio manager.
	/// </summary>
	public class MgrAudio:MonoBehaviour
	{
		#region private variables

		private static AudioSource musicSource;
		private static AudioSource soundSource;
		private static string audioNameFormat = "Audios/{0}";
		private static GameObject audioObject;
		private static float maxMusicVolume = 0.5f;
		private static float maxSoundVolume = 0.5f;
		#if UNITY_EDITOR
		#elif UNITY_ANDROID
        private static AndroidJavaObject audioCenter;
        private static Dictionary<string,int> androidSoundIds = new Dictionary<string, int> ();
        #endif
		#endregion

		#region constructed function & initialization

		static MgrAudio ()
		{
			audioObject = XLAFMain.XLAFGameObject;

//            instance = audioObject.GetComponent<MgrAudio> ();
			musicSource = audioObject.AddComponent<AudioSource> ();
			soundSource = audioObject.AddComponent<AudioSource> ();
			DontDestroyOnLoad (audioObject);

			if (GameObject.FindObjectOfType<AudioListener> () == null) {
				audioObject.AddComponent<AudioListener> ();
			}
			#if UNITY_EDITOR
			#elif UNITY_ANDROID
            audioCenter = new AndroidJavaObject ("plugin.albert.audiocenter.AudioCenter", 5);
			#endif
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{
            
		}

		#endregion

		#region public functions

		public static void SwitchMusic (bool isMusicOn)
		{
			MgrData.Set (MgrData.appSettingsName, "XLAF.music", isMusicOn);
		}

		public static void SwitchSound (bool isSoundOn)
		{
			MgrData.Set (MgrData.appSettingsName, "XLAF.sound", isSoundOn);
		}


		#if UNITY_EDITOR
		#elif UNITY_ANDROID
        
        /// <summary>
        /// Preloads the audio.
        /// </summary>
        /// <param name="soundName">Sound name. (with ext  e.g. click.mp3)</param>
        public static void PreloadAudio (string soundName)
        {
            if (soundName.IndexOf (".") < 0) {
                Log.Warning ("You must use soundName's extension, for example \"click.mp3\"");
                return;
            }
        int id = audioCenter.Call<int> ("LoadSound", _GetAudioSource (soundName));
            androidSoundIds.Add (soundName, id);
        }
        #endif
		public static void PlayMusic (string musicName, bool loop = true, float fadeInTime = 0f)
		{
			if (!MgrData.GetBool (MgrData.appSettingsName, "XLAF.music", true)) {
				return;
			}
			if (musicName.IndexOf (".") < 0) {
				Log.Warning ("You must use musicName's extension, for example \"click.mp3\"");
				return;
			}
			AudioClip clip = Resources.Load<AudioClip> (_GetAudioSource (musicName.Split (new char[]{ '.' }) [0]));
			musicSource.loop = loop;
			musicSource.clip = clip;
			musicSource.Play ();
			if (fadeInTime <= 0f) {
				musicSource.volume = maxMusicVolume;
			} else {
				_FadeInOutVolume (musicSource, fadeInTime, 0, maxMusicVolume);
			}
		}

		public static void PlayMusic (string musicName, float fadeInTime)
		{
			PlayMusic (musicName, true, fadeInTime);
		}

		public static void StopMusic (float fadeInTime = 0f)
		{
			if (fadeInTime <= 0f) {
				musicSource.Stop ();
			} else {
				_FadeInOutVolume (musicSource, fadeInTime, maxMusicVolume, 0, () => {
					musicSource.Stop ();
				});
			}
		}

		public static void PauseMusic (float fadeInTime = 0f)
		{
			if (fadeInTime <= 0f) {
				musicSource.Pause ();
			} else {
				_FadeInOutVolume (musicSource, fadeInTime, maxMusicVolume, 0, () => {
					musicSource.Pause ();
				});
			}
		}

		public static void ResumeMusic (float fadeInTime = 0f)
		{

			musicSource.UnPause ();
			if (fadeInTime <= 0f) {
				musicSource.volume = maxMusicVolume;
			} else {
				_FadeInOutVolume (musicSource, fadeInTime, 0, maxMusicVolume);
			}
		}

		/// <summary>
		/// Plaies the sound.
		/// </summary>
		/// <param name="soundName">Sound name. (with ext  e.g. click.mp3)</param>
		public static void PlaySound (string soundName)
		{
			PlaySound (soundName, maxSoundVolume);
		}

		/// <summary>
		/// Plaies the sound.
		/// </summary>
		/// <param name="soundName">Sound name. (with ext  e.g. click.mp3)</param>
		/// <param name="valume">Valume.</param>
		public static void PlaySound (string soundName, float valume)
		{
			if (!MgrData.GetBool (MgrData.appSettingsName, "XLAF.sound", true)) {
				return;
			}
			if (soundName.IndexOf (".") < 0) {
				Log.Warning ("You must use soundName's extension, for example \"click.mp3\"");
				return;
			}
			#if UNITY_ANDROID  && !UNITY_EDITOR
            int soundId = 0;
            if (androidSoundIds.TryGetValue (soundName, out soundId)) {
                audioCenter.Call ("PlaySound", soundId, valume);
            }
			#else
			AudioClip clip = Resources.Load<AudioClip> (_GetAudioSource (soundName.Split (new char[]{ '.' }) [0]));
			if (clip != null) {
				soundSource.volume = valume;
				soundSource.PlayOneShot (clip);
			}
			#endif
		}

		public static void StopSound ()
		{
//            #if UNITY_ANDROID && !UNITY_EDITOR
//            PluginAndroid.StopSound ();
//            #else
			soundSource.Stop ();
//            #endif
		}


		public static void StopAll (float fadeInTime = 0f)
		{
			StopMusic (fadeInTime);
			StopSound ();
		}

		public static void PauseAll (float fadeInTime = 0f)
		{
			PauseMusic (fadeInTime);
		}

		public static void ResumeAll (float fadeInTime = 0f)
		{
			ResumeMusic (fadeInTime);
		}

		#endregion

		#region private functions

		private static void _FadeInOutVolume (AudioSource source, float fadeInTime, float from, float to, Action cb = null)
		{
			iTween.ValueTo (audioObject, iTween.Hash (
				"from", from,
				"to", to,
				"time", fadeInTime,
				"onupdate", (Action<float>)((volume) => {
				source.volume = volume;
			}),
				"oncomplete", (Action)(() => {
				if (cb != null)
					cb ();
			})
			));
		}

		private static string _GetAudioSource (string audioName)
		{
			return string.Format (audioNameFormat, audioName);
		}

		#endregion

	}
}