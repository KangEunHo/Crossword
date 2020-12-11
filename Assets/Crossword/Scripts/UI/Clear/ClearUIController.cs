using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HealingJam.Crossword.Save;
using DG.Tweening;
namespace HealingJam.Crossword.UI
{
    public class ClearUIController : MonoBehaviour
    {
        [SerializeField] private GameObject nextStageButton = null;
        [SerializeField] private Image gaugeImage = null;
        [SerializeField] private Text levelText = null;

        [SerializeField] private Transform coinIconTf = null;
        [SerializeField] private Text coinText = null;

        private int addCoinAmount = 0;
        private int remainingAdditionalCoin;

        public void SetUp(bool alreadyCompleted, int addCoinAmount)
        {
            this.addCoinAmount = addCoinAmount;
            remainingAdditionalCoin = addCoinAmount;

            bool packLastStage = ((CrosswordMapManager.Instance.ActivePackIndex + 1) % CrosswordMapManager.LEVEL_IN_PACK_COUNT) == 0;
            nextStageButton.SetActive(packLastStage == false);
            levelText.text = (CrosswordMapManager.Instance.ActiveLevelIndex +1).ToString();

            int completeCount = 0;
            int page = CrosswordMapManager.Instance.ActivePackIndex / 8;
            for (int i = 0; i < CrosswordMapManager.LEVEL_IN_PACK_COUNT; ++i)
            {
                int packIndex = page * CrosswordMapManager.LEVEL_IN_PACK_COUNT + i;

                if (SaveMgr.Instance.GetCompleteData(packIndex))
                {
                    completeCount++;
                }
            }

            float prePageProgress = (completeCount - 1) / (float)CrosswordMapManager.LEVEL_IN_PACK_COUNT * 0.8f;
            float pageProgress = completeCount / (float)CrosswordMapManager.LEVEL_IN_PACK_COUNT * 0.8f;

            if (alreadyCompleted)
            {
                gaugeImage.fillAmount = 0.1f + pageProgress;
                coinText.text = "0";
            }
            else
            {
                gaugeImage.fillAmount = 0.1f + prePageProgress;

                coinText.text = addCoinAmount.ToString();
                gaugeImage.DOFillAmount(0.1f + pageProgress, 0.5f)
                    .OnComplete(()=> { GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coinIconTf.position, GameMgr.Instance.topUIController.GetCoinRT(), OnCoinAnimationEnd, CoinFlyAnimation.DivisionCoinAmounts(addCoinAmount, 5)); });

            }
        }

        private void OnCoinAnimationEnd(int coin)
        {
            remainingAdditionalCoin -= coin;
            SaveMgr.Instance.AddCoin(coin);
        }

        //private void OnDisable()
        //{
        //    if (remainingAdditionalCoin > 0)
        //    {
        //        SaveMgr.Instance.AddCoin(remainingAdditionalCoin);
        //        remainingAdditionalCoin = 0;
        //    }
        //}
    }
}