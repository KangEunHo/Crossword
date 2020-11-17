#if DoTween
namespace HealingJam
{ 
    public static class DoTweenUtilities
    {
        public static void SafeKillTween(DG.Tweening.Tween tween, bool rewind = true, bool completed = false)
        {
            if (tween != null)
            {
                if (rewind)
                {
                    tween.Rewind();
                }
                tween.Kill(completed);
                tween = null;
            }
        }
    }
}
#endif
