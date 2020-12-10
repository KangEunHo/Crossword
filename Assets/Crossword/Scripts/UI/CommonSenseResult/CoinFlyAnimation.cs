using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class CoinFlyAnimation : MonoBehaviour
    {
        private const float COIN_FLY_DURATION = 0.5f;
        private const float COIN_FLY_INTERVAL = 0.05f;
        private const float COIN_FIRST_PATH_MIN_DISTANCE = 20f;
        private const float COIN_FIRST_PATH_MAX_DISTANCE = 40f;

        [SerializeField] private GameObject coinPrefab = null;
        [SerializeField] private AnimationCurve targetScaleCurve = null;
        [SerializeField] private AnimationCurve coinFadeCurve = null;
        private Tween targetTween = null;
        private GameObjPool coinPool = null;

        public void PlayAnimation(RectTransform target, Action<int> coinAnimationEndAction, int[] coinAmounts)
        {
            if (coinPool == null)
            {
                coinPool = new GameObjPool(new InstantiateObjectFactory<GameObject>(coinPrefab, transform), new ObjPoolData(transform, 5));
            }
            CoroutineHelper.StartStaticCoroutine(PlayAnimationCoroutine(target, coinAnimationEndAction, coinAmounts));
            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.success);
        }

        private IEnumerator PlayAnimationCoroutine(RectTransform target, Action<int> coinAnimationEndAction, int[] coinAmounts)
        {
            var interval = new WaitForSeconds(COIN_FLY_INTERVAL);
            for (int i = 0; i < coinAmounts.Length; ++i)
            {
                GameObject coin = coinPool.Pop();
                coin.transform.position = transform.position;
                coin.GetComponent<Image>().SetAlpha(0f);

                int index = i;
                Vector3[] wayPoints = new Vector3[2];
                float wayPoint0_x = Random.Range(COIN_FIRST_PATH_MIN_DISTANCE, COIN_FIRST_PATH_MAX_DISTANCE);
                if (Random.value > 0.5f)
                    wayPoint0_x *= -1f;

                float wayPoint0_y = -Random.Range(COIN_FIRST_PATH_MIN_DISTANCE, COIN_FIRST_PATH_MAX_DISTANCE) * 2f;
                wayPoints[0] = transform.position + (new Vector3(wayPoint0_x, wayPoint0_y));
                wayPoints[1] = target.position;
                coin.transform.DOMove(wayPoints[0], COIN_FLY_DURATION * 0.5f).SetEase(Ease.InOutQuad);
                coin.transform.DOMove(wayPoints[1], COIN_FLY_DURATION * 0.5f).SetEase(Ease.InOutQuad).SetDelay(COIN_FLY_DURATION * 0.5f)
                    .OnComplete(() =>
                    {

                        if (targetTween != null)
                            targetTween.Kill(true);
                        targetTween = target.DOScale(1.2f, COIN_FLY_INTERVAL).SetEase(targetScaleCurve);
                        coinPool.Push(coin);

                        coinAnimationEndAction?.Invoke(coinAmounts[index]);
                    });

                coin.GetComponent<Image>().DOFade(1f, COIN_FLY_DURATION).SetEase(coinFadeCurve);
                yield return interval;
            }
        }

        public static int[] DivisionCoinAmounts(int coin, int division)
        {
            int[] coins = new int[Mathf.CeilToInt(coin / (float)division)];
            for (int i =0;i < coins.Length; ++i)
            {
                if (coin >= division)
                    coins[i] = division;
                else
                    coins[i] = coin;

                coin -= division;
            }

            return coins;
        }

    }
}