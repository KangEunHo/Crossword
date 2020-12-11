using UnityEngine;
using System.Collections;

using HealingJam.Crossword.UI;

namespace HealingJam.GameScreens
{
    public class FadeAndScaleTweenScreen : FadeScreen
    {
        private ScaleTweenAnimation[] scaleTweenAnimations = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (scaleTweenAnimations == null)
                scaleTweenAnimations = GetComponentsInChildren<ScaleTweenAnimation>();

            foreach (var v in scaleTweenAnimations)
            {
                v.Play();
            }
            
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            if (scaleTweenAnimations == null)
                scaleTweenAnimations = GetComponentsInChildren<ScaleTweenAnimation>();

            foreach (var v in scaleTweenAnimations)
            {
                v.Rewind();
            }
            
        }
    }
}