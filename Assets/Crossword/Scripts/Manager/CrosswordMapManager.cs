using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using AssetBundles;
using System.Collections;

namespace HealingJam.Crossword
{
    public class CrosswordMapManager : MonoSingleton<CrosswordMapManager>
    {
        private const string ASSET_BUNDLE_NAME = "mapdata";
        public const int LEVEL_IN_PACK_COUNT = 8;
        public const int BADGE_IN_LEVEL_COUNT = 5;

        [SerializeField] private TextAsset[] crosswordTextAssets = null;
        [SerializeField] private Sprite[] badgeSprites = null;
        public Sprite ZeroLevelBadgeSprite = null;

        public int BadgeSpriteLength => badgeSprites.Length;

        private List<TextAsset> listOfCrosswordMap = null;

        private int activePackIndex;
        public int ActivePackIndex { get { return activePackIndex; }
            set
            {
                if (value >= 0 && value < MaxStage())
                    activePackIndex = value;
            }
        }
        public int ActiveLevelIndex { get; set; }

        public CrosswordMap ActiveCrosswordMap = null;

        public override void Init()
        {
            listOfCrosswordMap = new List<TextAsset>(crosswordTextAssets);

            for (int i = 0; i < listOfCrosswordMap.Count; ++i)
            {
                if (listOfCrosswordMap[i].text.Contains("히틀러"))
                {
                    Debug.Log(i);
                }
            }
        }

        public void SetUpDatabase()
        {
            int maxStage = Instance.MaxStage();

            CrosswordMap[] crosswordMaps = new CrosswordMap[maxStage];
            for (int i = 0; i < maxStage; ++i)
            {
                crosswordMaps[i] = Instance.GetCrosswordMap(i);
            }
            LetterDatabase.SetUpDatabase(crosswordMaps);
        }

        public void AddCrosswordMaps(List<TextAsset> crosswordMaps)
        {
            listOfCrosswordMap.AddRange(crosswordMaps);
        }

        public int MaxStage()
        {
            return listOfCrosswordMap.Count;
        }

        public CrosswordMap GetCrosswordMap(int index)
        {
            return JsonConvert.DeserializeObject<CrosswordMap>(listOfCrosswordMap[index].text);
        }

        public IEnumerator LoadCrosswordMapAtAssetBundle()
        {
            AssetBundleManager.SetDevelopmentAssetBundleServer();

            var operation = AssetBundleManager.Initialize();
            yield return operation;

            var mapDataOperation = AssetBundleManager.LoadAllAssetAsync(ASSET_BUNDLE_NAME);
            yield return mapDataOperation;

            List<TextAsset> textAssets = new List<TextAsset>();
            var assetBundle = AssetBundleManager.GetLoadedAssetBundle(ASSET_BUNDLE_NAME, out string error);

            if (string.IsNullOrEmpty(error))
            {
                foreach (var asset in assetBundle.m_AssetBundle.LoadAllAssets())
                {
                    textAssets.Add(asset as TextAsset);
                }
            }
            else
            {
                EditorDebug.LogWarning(error);
            }

            AddCrosswordMaps(textAssets);
        }

        public Sprite GetBadgeSpriteToLevelIndex(int levelIndex)
        {
            if (levelIndex < 0)
                return ZeroLevelBadgeSprite;

            int badgeSpriteIndex = Mathf.FloorToInt(levelIndex / BADGE_IN_LEVEL_COUNT);
            badgeSpriteIndex = Mathf.Clamp(badgeSpriteIndex, 0, badgeSprites.Length - 1);

            return badgeSprites[badgeSpriteIndex];
        }

        public Sprite GetBadgeSpriteToBadgeIndex(int badgeIndex)
        {
            badgeIndex = Mathf.Clamp(badgeIndex, 0, badgeSprites.Length - 1);

            return badgeSprites[badgeIndex];
        }
    }
}