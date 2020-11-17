using UnityEngine;
using System;
using System.Collections.Generic;
using UnityQuickSheet;

namespace HealingJam.Localization
{
    /// <summary>
    /// 로컬라이제이션 엑셀파일을 QuickSheet로 생성한 후 나온 에셋을 이용합니다.
    /// 언어 추가시 동적으로 동기화 되지 않습니다.
    /// 언어 추가, 제거시 ValidLanguages 와 localizationData 생성 부분을 변경해야합니다.
    /// </summary>
    public class LocalizationMgr : MonoSingleton<LocalizationMgr>
    {
        public const SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.English;
        private const string SAVED_LANGUAGE_KEY = "LocalizationSavedLanguage";

        public static event Action<SystemLanguage> OnLanguageChanged = null;

        [SerializeField] private UnityQuickSheet.Localization localizationAsset = null;

        public HashSet<SystemLanguage> ValidLanguages { get; private set; } = new HashSet<SystemLanguage>()
        {  SystemLanguage.English, SystemLanguage.Korean, SystemLanguage.Japanese};

        private Dictionary<string, Dictionary<SystemLanguage, string>> localizationDatas = null;

        /// <summary>
        /// 로컬 PlayerPrefs에 언어를 저장합니다.
        /// 저장되어있지 않을시 기본값은 SystemLanguage.Unknown 입니다.
        /// </summary>
        public SystemLanguage SavedLanguage { get
            {
                int savedVal = PlayerPrefs.GetInt(SAVED_LANGUAGE_KEY, (int)SystemLanguage.Unknown);
                return (SystemLanguage)savedVal;
            }
            set
            {
                PlayerPrefs.SetInt(SAVED_LANGUAGE_KEY, (int)value);
            }
        }

        private SystemLanguage currentLanguage;
        public SystemLanguage CurrentLanguage { get { return currentLanguage; }}

        public override void Init()
        {
            localizationDatas = new Dictionary<string, Dictionary<SystemLanguage, string>>();

            foreach (var data in localizationAsset.dataArray)
            {
                Dictionary<SystemLanguage, string> localizationData = new Dictionary<SystemLanguage, string>
                {
                    { SystemLanguage.English, data.English },
                    { SystemLanguage.Korean, data.Korean },
                    { SystemLanguage.Japanese, data.Japanese }
                };
                localizationDatas.Add(data.Key, localizationData);
            }

            localizationAsset = null;

            LanguageSetUp();
        }

        private void LanguageSetUp()
        {
            SystemLanguage savedLanguage = SavedLanguage;

            if (savedLanguage == SystemLanguage.Unknown)
            {
                if (ValidLanguages.Contains(Application.systemLanguage))
                {
                    ChangeLanguage(Application.systemLanguage);
                }
                else
                {
                    ChangeLanguage(DEFAULT_LANGUAGE);
                }
            }
            else
            {
                currentLanguage = savedLanguage;
            }
        }

        public string GetString(string key)
        {
            if (localizationDatas.TryGetValue(key, out var data))
            {
                if (data.TryGetValue(currentLanguage, out string value))
                {
                    return value;
                }
                else
                {
                    return key;
                }
            }
            else
            {
                return key;
            }

        }

        public void ChangeLanguage(SystemLanguage language)
        {
            if (currentLanguage == language)
                return;

            currentLanguage = language;
            SavedLanguage = language;
            OnLanguageChanged?.Invoke(language);
        }
    }
}