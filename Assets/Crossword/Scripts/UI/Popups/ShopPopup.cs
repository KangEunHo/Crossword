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

        private bool saveTrigger = false;

        private void Start()
        {
#if UNITY_ANDROID
            restoreButton.SetActive(false);
#elif UNITY_IOS
            restoreButton.SetActive(true);
#endif

            Advertising.AdsRemoved += ChangeRemoveAdButtonInteraction;
        }

       
        private void OnDisable()
        {
            if (saveTrigger)
            {
                SaveMgr.Instance.Save();
                saveTrigger = false;
            }
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
            GameMgr.Instance.topUIController.PlayCoinFlyAnimation(rewardAdTransform.position, 30, 10);
            saveTrigger = true;
        }

        public void GetPurchaseReward(Product product)
        {
            string id = product.definition.id;

            if (id == "com.healingjam.crossword2.coin_150")
            {
                GameMgr.Instance.topUIController.PlayCoinFlyAnimation(coin1Transform.position, 150, 30);
            }
            else if (id == "com.healingjam.crossword2.coin_900")
            {
                GameMgr.Instance.topUIController.PlayCoinFlyAnimation(coin2Transform.position, 900, 100);
            }
            else if (id == "com.healingjam.crossword2.coin_2000")
            {
                GameMgr.Instance.topUIController.PlayCoinFlyAnimation(coin3Transform.position, 2000, 100);
            }
            else if (id == "com.healingjam.crossword2.remove_ad")
            {
                SaveMgr.Instance.SetAdRemove(true);
                Advertising.RemoveAds();
            }

            saveTrigger = true;
        }

        public void RestoreProduct(Product product)
        {
            string id = product.definition.id;

            #if UNITY_IOS
            ToastPlugin.ToastHelper.ShowToast("복원에 성공했습니다");
#endif
            if (id == "com.healingjam.crossword2.remove_ad")
            {
                SaveMgr.Instance.SetAdRemove(true);
                Advertising.RemoveAds();
                saveTrigger = true;
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