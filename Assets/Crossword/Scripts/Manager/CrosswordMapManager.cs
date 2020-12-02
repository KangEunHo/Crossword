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

        [SerializeField] private TextAsset[] crosswordTextAssets = null;
        private List<TextAsset> listOfCrosswordMap = null;

        private int activeStageIndex;
        public int ActiveStageIndex { get { return activeStageIndex; }
            set
            {
                if (value >= 0 && value < MaxStage())
                    activeStageIndex = value;
            }
        }
        public int ActiveLevelIndex { get; set; }

        public CrosswordMap ActiveCrosswordMap = null;

        public override void Init()
        {
            listOfCrosswordMap = new List<TextAsset>(crosswordTextAssets);
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
    }
}