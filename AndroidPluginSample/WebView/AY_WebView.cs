using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using AppYeast.Private;

namespace AppYeast.Public
{
    public class AY_WebView
    {
        public const string EVENT_WEBVIEW_SHOW = "event_webview_show";
        public const string EVENT_WEBVIEW_HIDE = "event_webview_hide";
        public const string WEBVIEW_CATEGORY_SUPPORT = "webview_category_support";

        private static string webviewCategory = WEBVIEW_CATEGORY_SUPPORT;
        private static bool isInitialized = false;
        private static bool isWebViewShown = false;

        #if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void _WebViewInit(string unityReceiver);

        [DllImport ("__Internal")]
        private static extern void _WebViewSetVisibility(bool visibility);

        [DllImport ("__Internal")]
        private static extern void _WebViewLoadURL(string url);

        [DllImport ("__Internal")]
        private static extern void _WebViewPostURL(string url, string rtParams);
        #elif UNITY_ANDROID
        private const string webViewClassName = "com.unity.unitywebview.UnityWebView";
        private static AndroidJavaClass WebViewClass;
        #endif

        public static void Init()
        {
            if (isInitialized)
            {
                return;
            }
            isInitialized = true;

            AY_Helper.GetInstance().StartCoroutine(InitEnumerator());
        }

        private static IEnumerator InitEnumerator()
        {
            yield return new WaitForEndOfFrame ();

            new GameObject(AY_WebViewHelper.unityReceiver, typeof(AY_WebViewHelper));

            if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
            {
                #if UNITY_IOS
                _WebViewInit(AY_WebViewHelper.unityReceiver);
                #elif UNITY_ANDROID
                WebViewClass = new AndroidJavaClass(webViewClassName);
                WebViewClass.CallStatic("Init", AY_WebViewHelper.unityReceiver);
                #endif
            }
        }

        public static bool IsWebViewShown()
        {
            return isWebViewShown;
        }

        public static void ShowSupport()
        {
            webviewCategory = WEBVIEW_CATEGORY_SUPPORT;

            string severUrl = "https://apys.me/sp/";
            string supportUrl = severUrl + "?rt=" + AY_GTA.GetBase64ParamsForSupport ();
            //Can't use post
            //Add a retry button, if use post, the failUrl would not include GTA info, so use get
            //PostURL(severUrl, AY_GTA.GetBase64ParamsForSupport());
            LoadURL(supportUrl);

            AY_GTA.TrackeventSupportView ();
            AY_EventDispatcher.Instance.Dispatch(EVENT_WEBVIEW_SHOW, webviewCategory);
        }

        public static void HideSupport()
        {
            if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
            {
                #if UNITY_IOS
                _WebViewSetVisibility(false);
                #elif UNITY_ANDROID
                if (WebViewClass != null)
                {
                    WebViewClass.CallStatic ("SetVisibility", false);
                }
                #endif
            }
        }

        public static void LoadURL(string url)
        {
            if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
            {
                isWebViewShown = true;

                #if UNITY_IOS
                _WebViewLoadURL(url);
                _WebViewSetVisibility(true);
                #elif UNITY_ANDROID
                if (WebViewClass != null)
                {
                    WebViewClass.CallStatic("LoadURL", url);
                    WebViewClass.CallStatic("SetVisibility", true);
                }
                #endif
            }
        }

        private static void PostURL(string url, string rtParams)
        {
            if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
            {
                isWebViewShown = true;

                #if UNITY_IOS
                _WebViewPostURL(url, rtParams);
                _WebViewSetVisibility(true);
                #elif UNITY_ANDROID
                if (WebViewClass != null)
                {
                    WebViewClass.CallStatic("PostURL", url, rtParams);
                    WebViewClass.CallStatic("SetVisibility", true);
                }
                #endif
            }
        }

        private class AY_WebViewHelper : MonoBehaviour
        {
            public const string unityReceiver = "AY_WebViewHelper";

            private static AY_WebViewHelper instance;

            void Awake()
            {
                if (instance != null)
                {
                    Destroy (this);
                    return;
                }
                instance = this;
                name = unityReceiver;
                DontDestroyOnLoad(this.gameObject);
            }

            void OnWebViewClose(string originalUrl)
            {
                isWebViewShown = false;

                AY_GTA.TrackeventSupportClose ();
                AY_EventDispatcher.Instance.Dispatch(EVENT_WEBVIEW_HIDE, webviewCategory);
            }
        }
    }
}