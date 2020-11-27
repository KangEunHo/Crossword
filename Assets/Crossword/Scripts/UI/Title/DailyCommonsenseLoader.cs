using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class DailyCommonsenseLoader : MonoBehaviour
    {
        private static string URL_PATH {get {return Path.Combine("http://healingjam.cafe24.com/crossword", GetPlatformName()); } }
        public bool Showed { get; private set; } = false;
        private List<DailyCommonsense> dailyCommonsenses = null;

        public void Show()
        {
            if (Showed == false)
            {
                Showed = true;
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.InternetConnectionWarning,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
            else
            {
                StartCoroutine(LoadCommonsenseAndShowPopup());
            }
        }

        public IEnumerator LoadCommonsenseAndShowPopup()
        {
            dailyCommonsenses = new List<DailyCommonsense>();

            yield return StartCoroutine(LoadCommonSenseCoroutine());

            if (dailyCommonsenses.Count > 0)
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.DailyCommonSense,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f), dailyCommonsenses);
            }
        }

        IEnumerator LoadCommonSenseCoroutine()
        {
            for (int i = 0; i < 7; ++i)
            {
                string date = DailyCommonsensePopup.KOREA_TIME.AddDays(-i).ToString("yyyyMMdd");
                string fileName = date + ".txt";
                string webPath = Path.Combine(URL_PATH, fileName);

                using (UnityWebRequest request = UnityWebRequest.Get(webPath))
                {
                    yield return request.SendWebRequest();
                    if (request.isNetworkError)
                    {
                        EditorDebug.LogWarning(request.error);
                    }
                    else
                    {
                        if (request.downloadHandler.isDone)
                        {
                            //string[] data = request.downloadHandler.text.Split(new char[] { '\n' }, 2);
                            StringReader s = new StringReader(request.downloadHandler.text);
                            DailyCommonsense dailyCommonsense = new DailyCommonsense
                            {
                                date = date,
                                word = s.ReadLine(),
                                meaning = s.ReadToEnd()
                            };

                            dailyCommonsenses.Add(dailyCommonsense);
                        }
                    }
                }
            }
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformForAssetBundles(Application.platform);
#endif
        }

#if UNITY_EDITOR
        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
    }
}