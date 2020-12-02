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

        static public Coroutine RunAfterDelay(float delay, System.Action action)
        {
            return Instance.StartCoroutine(RunAfterDelayCoroutine(delay, action));
        }

        static private IEnumerator RunAfterDelayCoroutine(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}