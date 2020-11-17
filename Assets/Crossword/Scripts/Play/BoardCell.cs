using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    public class BoardCell : MonoBehaviour
    {
        public enum CellState
        {
            None, Unuse, Completed
        }

        [SerializeField] private Text letterText = null;
        [SerializeField] private Image cellImage = null;

        public Vector2Int Index { get; private set; }

        public WordDataForGame HorizontalWordData { get; set; } = null;
        public WordDataForGame VerticalWordData { get; set; } = null;
        public CellState State { get; private set; } = CellState.None;

        public void Init(Vector2Int index)
        {
            Index = index;
            HorizontalWordData = null;
            VerticalWordData = null;
        }

        public void SetUnuseState()
        {
            State = CellState.Unuse;
            gameObject.SetActive(false);
        }
    }
}