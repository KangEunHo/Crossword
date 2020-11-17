using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace HealingJam.Crossword
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextAsset mapTextAsset = null;
        [SerializeField] private BoardController boardController = null;

        private void Start()
        {
            boardController.GenerateBoard(JsonConvert.DeserializeObject<CrosswordMap>(mapTextAsset.text));
        }
    }
}