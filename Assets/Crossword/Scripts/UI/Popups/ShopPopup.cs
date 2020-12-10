using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HealingJam.Popups;
using HealingJam.Crossword.Save;
using UnityEngine.Purchasing;
using EasyMobile;


namespace HealingJam.Crossword
{
    public class ShopPopup : CallbackPopup
    {
        [SerializeField] private GameObject restoreButton = null;
        [SerializeField] private Button removeAdButton = null;

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
            SaveMgr.Instance.AddCoin(30);
            SaveMgr.Instance.Save();
        }

        public static void GetPurchaseReward(Product product)
        {
            string id = product.definition.id;

            if (id == "com.healingjam.crossword.coin_150")
            {
                SaveMgr.Instance.AddCoin(150);
                SaveMgr.Instance.Save();
            }
            else if (id == "com.healingjam.crossword.coin_900")
            {
                SaveMgr.Instance.AddCoin(900);
                SaveMgr.Instance.Save();
            }
            else if (id == "com.healingjam.crossword.coin_2000")
            {
                SaveMgr.Instance.AddCoin(2000);
                SaveMgr.Instance.Save();
            }
            else if (id == "com.healingjjam.blockmaker.remove_ad")
            {
                SaveMgr.Instance.SetAdRemove(true);
                Advertising.RemoveAds();
            }
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