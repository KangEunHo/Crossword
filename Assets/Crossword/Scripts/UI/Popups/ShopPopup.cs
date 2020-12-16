using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HealingJam.Popups;
using HealingJam.Crossword.Save;
using UnityEngine.Purchasing;
using EasyMobile;
using HealingJam.Crossword.UI;


namespace HealingJam.Crossword
{
    public class ShopPopup : CallbackPopup
    {
        [SerializeField] private GameObject restoreButton = null;
        [SerializeField] private Button removeAdButton = null;
        [SerializeField] private Transform coin1Transform = null;
        [SerializeField] private Transform coin2Transform = null;
        [SerializeField] private Transform coin3Transform = null;
        [SerializeField] private Transform rewardAdTransform = null;
        [SerializeField] private TopUIController topUIController = null;


        private void Start()
        {
#if UNITY_ANDROID
            restoreButton.SetActive(false);
#elif UNITY_IOS
            restoreButton.SetActive(true);
#endif

            Advertising.AdsRemoved += ChangeRemoveAdButtonInteraction;
        }

        private void ChangeRemoveAdButtonInteraction()
        {
            removeAdButton.interactable = (Advertising.IsAdRemoved() == false);
        }

        public void OnRewardButtonClick()
        {
            GoogleMobileAdsMgr.Instance.ShowRewardedAd(Reward);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        private void Reward()
        {
            GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(rewardAdTransform.position, topUIController.GetCoinRT(), OnCoinAnimationEnd, UI.CoinFlyAnimation.DivisionCoinAmounts(30, 10));
        }

        public void GetPurchaseReward(Product product)
        {
            string id = product.definition.id;

            if (id == "com.healingjam.crossword.coin_150")
            {
                GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coin1Transform.position, topUIController.GetCoinRT(), OnCoinAnimationEnd, UI.CoinFlyAnimation.DivisionCoinAmounts(150, 30));
            }
            else if (id == "com.healingjam.crossword.coin_900")
            {
                GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coin3Transform.position, topUIController.GetCoinRT(), OnCoinAnimationEnd, UI.CoinFlyAnimation.DivisionCoinAmounts(900, 100));
            }
            else if (id == "com.healingjam.crossword.coin_2000")
            {
                GameMgr.Instance.topUIController.coinFlyAnimation.PlayAnimation(coin2Transform.position, topUIController.GetCoinRT(), OnCoinAnimationEnd, UI.CoinFlyAnimation.DivisionCoinAmounts(2000, 100));
            }
            else if (id == "com.healingjjam.blockmaker.remove_ad")
            {
                SaveMgr.Instance.SetAdRemove(true);
                Advertising.RemoveAds();
            }
        }

        private void OnCoinAnimationEnd(int coin)
        {
            Save.SaveMgr.Instance.AddCoin(coin);
        }

        public void RestoreProduct(Product product)
        {
            string id = product.definition.id;

            #if UNITY_IOS
            ToastPlugin.ToastHelper.ShowToast("복원에 성공했습니다");
#endif
            if (id == "com.healingjjam.blockmaker.remove_ad")
            {
                SaveMgr.Instance.SetAdRemove(true);
                Advertising.RemoveAds();
            }
        }

        public void OnPurchaseFailedHandler(Product failedProduct, PurchaseFailureReason failureReason)
        {
            Debug.Log("RESTORE FAILED FOR ITEM: " + failedProduct.definition.id + " BECAUSE: " + failureReason);

#if UNITY_IOS
            ToastPlugin.ToastHelper.ShowToast("복원할 상품이 없거나 복원에 실패했습니다", true);
#endif
        }
    }
}