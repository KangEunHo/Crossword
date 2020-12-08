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
        [SerializeField] CoinFlyAnimation coinFlyAnimation = null;
        [SerializeField] private CanvasGroup answerItemCanvasGroup = null;
        [SerializeField] private Text coinText = null;

        private List<RightAnswerCountData> rightAnswerCountDatas = null;
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;

        private int addCoinAmount = 0;
        private int remainingAdditionalCoin;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);

            answerItemController.Init();
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            WordMatchingPlayScreen.GameMode gameMode = (WordMatchingPlayScreen.GameMode)args[0];

            bool alreadyCompleted = (bool)args[1];
            answerItemDatas = args[2] as List<AnswerItem.AnswerItemData>;
            rightAnswerCountDatas = args[3] as List<RightAnswerCountData>;

            answerItemController.gameObject.SetActive(false);
            answerItemController.SetUp(answerItemDatas);

            addCoinAmount = 0;
            remainingAdditionalCoin = 0;

            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                badgeObject.gameObject.SetActive(false);
                foreach (var v in rightAnswerCountDatas)
                {
                    addCoinAmount += v.rightAnswerCount * 2;
                }

                SetRemaingingCoinAndText(addCoinAmount);
                coinFlyAnimation.gameObject.SetActive(true);
                coinFlyAnimation.PlayAnimation(GameMgr.Instance.topUIController.GetCoinRT(), OnCoinAnimationEnd, CoinFlyAnimation.DivisionCoinAmounts(addCoinAmount, 5));

                resultGaugeController.gameObject.SetActive(true);
                Invoke(nameof(PlayGaugeAnimation), 1f);
                Invoke(nameof(ShowAnswerItems), 2f);
            }
            else
            {
                badgeObject.gameObject.SetActive(true);
                resultGaugeController.gameObject.SetActive(false);
                badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex);
                badgeLevelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex + 1).ToString();
                if (alreadyCompleted)
                {
                    addCoinAmount = 0;
                    SetRemaingingCoinAndText(addCoinAmount);

                    ShowAnswerItems();
                }
                else
                {
                    addCoinAmount = 20;
                    SetRemaingingCoinAndText(addCoinAmount);
                    bool badgeChange = (CrosswordMapManager.Instance.ActiveLevelIndex + 1) % CrosswordMapManager.BADGE_IN_LEVEL_COUNT == 0;
                    badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex);

                    if (badgeChange)
                    {
                        badgeImage.transform.DOScale(1.2f, 0.2f).OnComplete(()=> { badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(CrosswordMapManager.Instance.ActiveLevelIndex + 1); });
                        badgeImage.transform.DOScale(11f, 0.3f).SetDelay(0.2f);
                    }

                    // 할것.
                    // 마지막 레벨을 깼을시 제한을 둬야되나.?
                    badgeLevelText.transform.DOScale(1.2f, 0.2f).OnComplete(() => { badgeLevelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex + 2).ToString(); });
                    badgeLevelText.transform.DOScale(1f, 0.3f).SetDelay(0.2f).OnComplete(()=>{ coinFlyAnimation.PlayAnimation(GameMgr.Instance.topUIController.GetCoinRT(), OnCoinAnimationEnd, CoinFlyAnimation.DivisionCoinAmounts(addCoinAmount, 5)); });

                    Invoke(nameof(ShowAnswerItems), 3f);
                }
            }
        }

        private void PlayGaugeAnimation()
        {
            resultGaugeController.PlayAnimation(rightAnswerCountDatas);
        }

        private void ShowAnswerItems()
        {
            coinFlyAnimation.gameObject.SetActive(false);
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
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }

        private void OnDisable()
        {
            if (remainingAdditionalCoin > 0)
            {
                SaveMgr.Instance.AddCoin(remainingAdditionalCoin);
                remainingAdditionalCoin = 0;
            }
        }

        private void SetRemaingingCoinAndText(int coin)
        {
            remainingAdditionalCoin = coin;
            coinText.text = coin.ToString();
        }
    }
}