using System;
using UnityEngine;
using EasyMobile;
using ToastPlugin;

namespace HealingJam
{
    public class GoogleMobileAdsMgr : MonoSingleton<GoogleMobileAdsMgr>
    {
        private const double SHOW_INTERSTITIAL_DELAY_TIME = 300;
        private RealTimer timer = null;
        private Action onUserEarnedReward = null;
        private Action onInterstitialClosed = null;

        public override void Init()
        {
            if (!RuntimeManager.IsInitialized())
                RuntimeManager.Init();

#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);
#endif
        }

        private void OnEnable()
        {
            Advertising.RewardedAdCompleted += HandleUserEarnedReward;
            Advertising.InterstitialAdCompleted += HandleInterstitialClosed;
        }

        private void OnDisable()
        {
            Advertising.RewardedAdCompleted -= HandleUserEarnedReward;
            Advertising.InterstitialAdCompleted -= HandleInterstitialClosed;
        }

        public void ShowBannerAD()
        {
            Advertising.ShowBannerAd(BannerAdPosition.Bottom, BannerAdSize.Banner);
        }

        public void HideBannerAD()
        {
            Advertising.HideBannerAd();
        }

        //Returns an ad request with custom ad targeting.
        public void ShowInterstitial()
        {
            if (Advertising.IsAdRemoved())
                return;

            if (Advertising.IsInterstitialAdReady())
                Advertising.ShowInterstitialAd();
        }

        public bool ShowDelayInterstitial(Action callback = null)
        {
            if (Advertising.IsAdRemoved())
                return false;

#if UNITY_EDITOR
            return false;
#endif
            if (Advertising.IsInterstitialAdReady())
            {
                this.onInterstitialClosed = callback;

                if (timer == null) // 타이머가 없다면 시간에 상관없이 보여준다.
                {
                    Advertising.ShowInterstitialAd();
                    timer = new RealTimer();
                    return true;
                }
                else
                {
                    if (timer.GetElapsedTime() >= SHOW_INTERSTITIAL_DELAY_TIME)
                    {
                        Advertising.ShowInterstitialAd();
                        timer = new RealTimer();
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;

        }

        public void ShowRewardedAd(Action callback)
        {
#if UNITY_EDITOR
            callback?.Invoke();
            return;
#endif
            if (Advertising.IsRewardedAdReady())
            {
                this.onUserEarnedReward = callback;
                Advertising.ShowRewardedAd();
            }
            else
            {
                ToastHelper.ShowToast("광고를 준비중입니다. 잠시후 다시 시도하세요", true);
            }
        }

        public void SetRemoveAds()
        {
            Advertising.RemoveAds();
        }

        public bool IsAdRemoved()
        {
            return Advertising.IsAdRemoved();
        }
        #region Banner callback handlers


        #endregion

        #region Interstitial callback handlers

        public void HandleInterstitialClosed(InterstitialAdNetwork interstitialAdNetwork, AdPlacement adPlacement)
        {
            onInterstitialClosed?.Invoke();
        }

        #endregion

        #region RewardedAd callback handlers

        public void HandleUserEarnedReward(RewardedAdNetwork rewardedAdNetwork, AdPlacement adPlacement)
        {
            this.onUserEarnedReward?.Invoke();
        }

        #endregion
    }

}