using UnityEngine;
using UnityEngine.UI;
using HealingJam.Crossword.Save;
using DG.Tweening;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class TopUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform coinRT = null;
        [SerializeField] private Text coinText = null;
        [SerializeField] private Text coinAddText = null;

        [SerializeField] private GameObject coinObject = null;
        [SerializeField] private GameObject backButton = null;
        [SerializeField] private GameObject optionButton = null;

        private Tween coinAmountTween = null;

        private void OnEnable()
        {
            if (SaveMgr.Instance.Loaded == false)
                SaveMgr.Instance.Load();

            SaveMgr.Instance.CoinAddAction += OnCoinAdded;
            SaveMgr.Instance.CoinChangeAction += OnCoinChanged;

            OnCoinChanged(SaveMgr.Instance.GetCoin());
        }

        private void OnDisable()
        {
            SaveMgr.Instance.CoinAddAction -= OnCoinAdded;
            SaveMgr.Instance.CoinChangeAction -= OnCoinChanged;
        }

        private void OnCoinAdded(int amount)
        {
            if (enabled == false)
                return;

            if (coinAddText == null)
                return;

            if (amount >= 0)
                coinAddText.text = "+" + amount.ToString();
            else
                coinAddText.text = amount.ToString();

            if (coinAmountTween != null)
                coinAmountTween.Kill(true);

            coinAddText.gameObject.SetActive(true);
            coinAmountTween = DOTween.Sequence()
                .Append(coinAddText.transform.DOScaleY(1.2f, 0.1f))
                .Append(coinAddText.transform.DOScaleY(1f, 0.2f))
                .OnComplete(() => { coinAddText.gameObject.SetActive(false); })
                .SetLink(coinAddText.gameObject);

        }

        private void OnCoinChanged(int coin)
        {
            coinText.text = coin.ToString();
        }

        public RectTransform GetCoinRT()
        {
            return coinRT;
        }

        public void OnBackButtonClick()
        {
            ScreenMgr.Instance.GetCurrentScreen().Escape();
        }

        public void OnCoinButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }

        public void OnOptionButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }

        public void SetActiveCoinButton(bool active)
        {
            coinObject.SetActive(active);
        }

        public void SetActiveBackButton(bool active)
        {
            backButton.SetActive(active);
        }

        public void SetActiveOptionButton(bool active)
        {
            optionButton.SetActive(active);
        }
    }
}