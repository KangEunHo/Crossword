using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace HealingJam.Crossword
{
    public class CrosswordMapManager : MonoSingleton<CrosswordMapManager>
    {
        public const int PACK_IN_STAGE_COUNT = 8;

        [SerializeField] private TextAsset[] crosswordTextAssets = null;

        public int ActiveStageIndex;
        public CrosswordMap ActiveCrosswordMap = null;

        public int MaxStage()
        {
            return crosswordTextAssets.Length;
        }

        public CrosswordMap GetCrosswordMap(int index)
        {
            return JsonConvert.DeserializeObject<CrosswordMap>(crosswordTextAssets[index].text);
        }
    }
}