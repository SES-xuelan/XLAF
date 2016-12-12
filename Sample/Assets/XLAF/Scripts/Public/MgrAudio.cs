using UnityEngine;
using System.Collections;
using System;


namespace XLAF.Public
{
    /// <summary>
    ///音频管理
    /// </summary>
    public class MgrAudio:MonoBehaviour
    {
        //        private static MgrAudio instance;
        private static AudioSource musicSource;
        private static AudioSource soundSource;
        private static string audioNameFormat = "_Audios/{0}";
        private static GameObject audioObject;
        private static float maxMusicVolume = 0.5f;
        private static float maxSoundVolume = 0.5f;

        static MgrAudio ()
        {
            audioObject = new GameObject (typeof(MgrAudio).ToString (), typeof(MgrAudio));

//            instance = audioObject.GetComponent<MgrAudio> ();
            musicSource = audioObject.AddComponent<AudioSource> ();
            soundSource = audioObject.AddComponent<AudioSource> ();
            DontDestroyOnLoad (audioObject);

            if (GameObject.FindObjectOfType<AudioListener> () == null) {
                audioObject.AddComponent<AudioListener> ();
            }


        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {
            
        }

        public static void SwitchMusic (bool isMusicOn)
        {
            MgrData.Set (MgrData.appSettingsName, "XLAF.music", isMusicOn);
        }

        public static void SwitchSound (bool isSoundOn)
        {
            MgrData.Set (MgrData.appSettingsName, "XLAF.sound", isSoundOn);
        }


        public static void PreloadAudio ()
        {
            
        }

        public static void PlayMusic (string musicName, bool loop = true, float fadeInTime = 0f)
        {
            if (!MgrData.GetBool (MgrData.appSettingsName, "XLAF.music", true)) {
                return;
            }

            AudioClip clip = Resources.Load<AudioClip> (_GetAudioSource (musicName));
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


        public static void PlaySound (string soundName)
        {
            if (!MgrData.GetBool (MgrData.appSettingsName, "XLAF.sound", true)) {
                return;
            }
            
//            #if UNITY_ANDROID && !UNITY_EDITOR
//            PluginAndroid.PlaySound (GetAudioSource(soundName));
//            #else
            AudioClip clip = Resources.Load<AudioClip> (_GetAudioSource (soundName));
            if (clip != null) {
                soundSource.volume = maxSoundVolume;
                soundSource.PlayOneShot (clip);
            }
//            #endif
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






        private static void _FadeInOutVolume (AudioSource source, float fadeInTime, float from, float to, Action cb = null)
        {
            XLAF_Tween.ValueTo (audioObject, XLAF_Tween.Hash (
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


    }
}