using UnityEngine;
using System.Collections;
#if !UNITY_EDITOR && UNITY_ANDROID
using Google.Play.Review;
#endif

namespace HealingJam
{
    public class InAppReviewMgr : MonoSingleton<InAppReviewMgr>
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        private ReviewManager reviewManager = null;
        private PlayReviewInfo playReviewInfo = null;
        public bool IsProgressing { get; private set; } = false;

        private void Start()
        {
            reviewManager = new ReviewManager();


            StartCoroutine(RequestReviewFlow());

        }

#endif

        public void LunchReview()
        {
            #if !UNITY_EDITOR && UNITY_ANDROID
            if (IsProgressing)
                return;

            StartCoroutine(LunchReviewFlow());
#           endif
        }

        #if !UNITY_EDITOR && UNITY_ANDROID
        private IEnumerator RequestReviewFlow()
        {
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;

            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                EditorDebug.LogWarning(requestFlowOperation.Error.ToString());
                yield break;
            }

            playReviewInfo = requestFlowOperation.GetResult();
        }

        private IEnumerator LunchReviewFlow()
        {
            IsProgressing = true;

            if (playReviewInfo == null)
            {
                yield return StartCoroutine(RequestReviewFlow());
            }

            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;
            playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                EditorDebug.LogWarning(launchFlowOperation.Error.ToString());

                IsProgressing = false;
                yield break;
            }
            else
            {
                IsProgressing = false;
            }
        }
#endif
    }
}