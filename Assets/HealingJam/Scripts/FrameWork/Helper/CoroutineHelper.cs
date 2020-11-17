using UnityEngine;
using System.Collections;

namespace HealingJam
{
    public class CoroutineHelper : MonoSingleton<CoroutineHelper>
    {
        static public Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
        }

        static public IEnumerator RunAfterDelay(float delayTime, System.Action action)
        {
            yield return new WaitForSeconds(delayTime);

            action?.Invoke();
        }
    }
}