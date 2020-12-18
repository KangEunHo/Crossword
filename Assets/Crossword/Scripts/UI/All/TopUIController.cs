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

        [SerializeField] private CoinFlyAnimation coinFlyAnimation = null;
        private Tween coinAmountTween = null;
        private int prevAddAmount = 0;
        private int coinTextCoin = 0;

        private void OnEnable()
        {
            SaveMgr.Instance.CoinAddAction += OnCoinAdded;
            //SaveMgr.Instance.CoinChangeAction += OnCoinChanged;

            UpdateCoinText(SaveMgr.Instance.GetCoin());
        }

        private void OnDisable()
        {
            SaveMgr.Instance.CoinAddAction -= OnCoinAdded;
           // SaveMgr.Instance.CoinChangeAction -= OnCoinChanged;
        }

        private void OnCoinAdded(int amount)
        {
            // 힌트 사용시에만 바로 코인텍스트가 적용됨.
            if (amount < 0)
            {
                UpdateCoinText(SaveMgr.Instance.GetCoin());
                PlayCoinAddAnimation(amount);
            }
        }

        private void PlayCoinAddAnimation(int amount)
        {
            if (enabled == false)
                return;

            if (coinAddText == null)
                return;

            if (coinAmountTween != null)
            {
                if (amount > 0)
                    amount += prevAddAmount;
                coinAmountTween.Kill(true);
            }

            if (amount >= 0)
                coinAddText.text = "+" + amount.ToString();
            else
                coinAddText.text = amount.ToString();

            prevAddAmount = amount;

            coinAddText.gameObject.SetActive(true);
            coinAmountTween = DOTween.Sequence()
                .Append(coinAddText.transform.DOScaleY(1.2f, 0.1f))
                .Append(coinAddText.transform.DOScaleY(1f, 0.2f))
                .AppendInterval(0.2f)
                .OnComplete(() => { coinAddText.gameObject.SetActive(false); coinAmountTween = null; })
                .SetLink(coinAddText.gameObject);

        }

        public void PlayCoinFlyAnimation(Vector3 startPos, int coin, int division, bool save = true)
        {
            if (save)
            {
                SaveMgr.Instance.AddCoin(coin);
                SaveMgr.Instance.Save();
            }
            coinFlyAnimation.PlayAnimation(startPos, coinRT, OnCoinEndFly, CoinFlyAnimation.DivisionCoinAmounts(coin, division));
        }

        private void OnCoinEndFly(int coinAmount)
        {
            UpdateCoinText(coinAmount + coinTextCoin);
            PlayCoinAddAnimation(coinAmount);
        }

        public void UpdateCoinText(int coin)
        {
            coinText.text = coin.ToString();
            coinTextCoin = coin;
        }

        //private void OnCoinChanged(int coin)
        //{
        //    coinText.text = coin.ToString();
        //}

        //public RectTransform GetCoinRT()
        //{
        //    return coinRT;
        //}

        public void OnBackButtonClick()
        {
            if (backButton.activeSelf)
            ScreenMgr.Instance.GetCurrentScreen().Escape();
        }

        public void OnCoinButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnOptionButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void SetActiveCoinButton(bool active)
        {
            coinObject.SetActive(active);
            if (active)
                UpdateCoinText(SaveMgr.Instance.GetCoin());
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