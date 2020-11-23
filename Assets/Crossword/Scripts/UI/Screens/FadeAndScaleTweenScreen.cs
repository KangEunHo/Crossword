using UnityEngine;
using System.Collections;

using HealingJam.Crossword.UI;

namespace HealingJam.GameScreens
{
    public class FadeAndScaleTweenScreen : FadeScreen
    {
        public override void Enter(params object[] args)
        {
            base.Enter(args);

            foreach(var v in GetComponentsInChildren<ScaleTweenAnimation>())
            {
                v.Play();
            }
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            foreach (var v in GetComponentsInChildren<ScaleTweenAnimation>())
            {
                v.Rewind();
            }
        }
    }
}