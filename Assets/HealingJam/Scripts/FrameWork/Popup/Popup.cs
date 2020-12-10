using UnityEngine;
using HealingJam.StateMachine;

namespace HealingJam.Popups
{
    public class Popup : MonoBehaviour, IState
    {
        [SerializeField] protected PopupID popupID = PopupID.None;
        [SerializeField] protected RectTransform window = null;

        #region Enum

        public enum PopupID
        {
            None, Shop, Option, PlayExit, DailyCommonSense, InternetConnectionWarning, WordData, ModifyReview, Login, MainBack
        }

        #endregion

        #region Properties

        public RectTransform Window { get { return window; } }
        public bool IsShowing { get; private set; } = false;

        #endregion

        protected MultiStateMachine<Popup> stateMachine = null;
        public IPopupAnimation PopupAnimation { get; private set; } = null;

        public virtual void Init(object stateMachine)
        {
            this.stateMachine = stateMachine as MultiStateMachine<Popup>;
        }

        public virtual void Enter(params object[] args)
        {
            IsShowing = true;
            gameObject.SetActive(true);
        }

        public virtual void Exit(params object[] args)
        {
            IsShowing = false;
            gameObject.SetActive(false);
        }

        public virtual void Escape()
        {
            PopupMgr.Instance.ExitWithAnimation(popupID);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public string Name()
        {
            return popupID.ToString();
        }

        public PopupID GetID()
        {
            return popupID;
        }

        public virtual void SetAnimation(IPopupAnimation popupAnimation)
        {
            if (this.PopupAnimation != null)
            {
                this.PopupAnimation.Dispose();
            }
            this.PopupAnimation = popupAnimation;
        }
    }
}