using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;
using HealingJam.Crossword.Save;
using DG.Tweening;

namespace HealingJam.Crossword.UI
{
    public class CommonSenseResultScreen : FadeAndScaleTweenScreen
    {
        [Header("Badge Test")]
        [SerializeField] private GameObject badgeObject = null;
        [SerializeField] private Image badgeImage = null;
        [SerializeField] private Text badgeLevelText = null;

        [Header("Ability Test")]
        [SerializeField] ResultGaugeController resultGaugeController = null;

        [SerializeField] AnswerItemController answerItemController = null;

        [SerializeField] private CanvasGroup answerItemCanvasGroup = null;
        [SerializeField] private GameObject coinIcon = null;
        [SerializeField] private Text coinText = null;

        private List<RightAnswerCountData> rightAnswerCountDatas = null;
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;

        private int addCoinAmount = 0;
        private int remainingAdditionalCoin;
        private WordMatchingPlayScreen.GameMode gameMode;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);

            answerItemController.Init();
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            gameMode = (WordMatchingPlayScreen.GameMode)args[0];

            bool alreadyCompleted = (bool)args[1];
            answerItemDatas = args[2] as List<AnswerItem.AnswerItemData>;
            rightAnswerCountDatas = args[3] as List<RightAnswerCountData>;

            answerItemController.gameObject.SetActive(false);
            answerItemController.SetUp(answerItemDatas);

            addCoinAmount = 0;
            remainingAdditionalCoin = 0;

            coinIcon.gameObject.SetActive(true);

            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                badgeObject.gameObject.SetActive(false);
                foreach (var v in rightAnswerCountDatas)
                {
                    addCoinAmount += v.rightAnswerCount * 2;
                }

                SetRemaingingCoinAndText(addCoinAmount);
                GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coinIcon.transform.position, GameMgr.Instance.topUIController.GetCoinRT(), OnCoinAnimationEnd, CoinFlyAnimation.DivisionCoinAmounts(addCoinAmount, 5));

                resultGaugeController.gameObject.SetActive(true);
                Invoke(nameof(PlayGaugeAnimation), 1f);
                Invoke(nameof(ShowAnswerItems), 2f);
            }
            else
            {
                badgeObject.gameObject.SetActive(true);
                resultGaugeController.gameObject.SetActive(false);
                if (alreadyCompleted)
                {
                    addCoinAmount = 0;
;                    SetRemaingingCoinAndText(addCoinAmount);
                    badgeLevelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex + 1).ToString();
                    ShowAnswerItems();

                    badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex);
                }
                else
                {
                    addCoinAmount = 20;
                    SetRemaingingCoinAndText(addCoinAmount);
                    bool badgeChange = (CrosswordMapManager.Instance.ActiveLevelIndex) % CrosswordMapManager.BADGE_IN_LEVEL_COUNT == 0;
                    badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex -1);

                    if (badgeChange)
                    {
                        badgeImage.transform.DOScale(1.5f, 0.2f).SetDelay(1f).OnStart(()=> { badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex); });
                        badgeImage.transform.DOScale(1f, 0.3f).SetDelay(1.2f);
                    }

                    badgeLevelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex).ToString();
                    // 할것.
                    // 마지막 레벨을 깼을시 제한을 둬야되나.?
                    badgeLevelText.transform.DOScale(1.5f, 0.2f).SetDelay(1f).OnStart(() => { badgeLevelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex + 1).ToString(); });
                    badgeLevelText.transform.DOScale(1f, 0.3f).SetDelay(1.2f);

                    Invoke(nameof(ShowAnswerItems), 2f);

                    CoroutineHelper.RunAfterDelay(1.5f, () => { GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coinIcon.transform.position, GameMgr.Instance.topUIController.GetCoinRT(), OnCoinAnimationEnd, CoinFlyAnimation.DivisionCoinAmounts(addCoinAmount, 5)); });
                }
            }

            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.playSuccess);
            CoroutineHelper.RunAfterDelay(3f, () => { GoogleMobileAdsMgr.Instance.ShowDelayInterstitial(); });
        }

        private void PlayGaugeAnimation()
        {
            resultGaugeController.PlayAnimation(rightAnswerCountDatas);
        }

        private void ShowAnswerItems()
        {
            coinIcon.gameObject.SetActive(false);
            answerItemController.gameObject.SetActive(true);
            answerItemCanvasGroup.alpha = 0f;
            answerItemCanvasGroup.DOFade(1f, 0.5f);
        }

        private void OnCoinAnimationEnd(int coin)
        {
            remainingAdditionalCoin -= coin;
            SaveMgr.Instance.AddCoin(coin);
        }

        public override void Escape()
        {
            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
                ScreenMgr.Instance.ChangeState(ScreenID.Title);
            else
                ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        //private void OnDisable()
        //{
        //    if (remainingAdditionalCoin > 0)
        //    {
        //        SaveMgr.Instance.AddCoin(remainingAdditionalCoin);
        //        remainingAdditionalCoin = 0;
        //    }
        //}

        private void SetRemaingingCoinAndText(int coin)
        {
            remainingAdditionalCoin = coin;
            coinText.text = coin.ToString();
        }
    }
}