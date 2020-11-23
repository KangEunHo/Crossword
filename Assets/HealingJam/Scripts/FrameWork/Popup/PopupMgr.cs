using UnityEngine;
using System.Collections.Generic;
using HealingJam.StateMachine;

namespace HealingJam.Popups
{
    using PopupID = Popup.PopupID;

    public class PopupMgr : MonoSingleton<PopupMgr>
    {
        [SerializeField] private Popup[] popups = null;
        [SerializeField] private Transform popupHolder = null;

        public MultiStateMachine<Popup> StateMachine {  get; private set; }

        public override void Init()
        {
            if (popupHolder == null)
                popupHolder = transform;

            StateMachine = new MultiStateMachine<Popup>();
            StateMachine.OnStateEntered += OnPopupEntered;
            StateMachine.OnStateExited += OnPopupExited;

            foreach (var popup in popups)
            {
                RegisterState(popup);
            }
        }

        public void ChangeState(PopupID popupID, params object[] enterArgs)
        {
            StateMachine.ChangeState(popupID.ToString(), enterArgs);
        }

        public void ChangeState(string name, params object[] enterArgs)
        {
            StateMachine.ChangeState(name, enterArgs);
        }

        public void EnterWithAnimation(PopupID popupID, IPopupAnimation popupAnimation, params object[] args)
        {
            Enter(popupID, args);
            var popup = StateMachine.GetStateInRegisteredStates(popupID.ToString());

            popupAnimation.Init(popup);
            popup.SetAnimation(popupAnimation);
            popupAnimation.PlayForward();
        }

        public void ExitWithAnimation(PopupID popupID, params object[] args)
        {
            Popup popup = StateMachine.GetStateInRegisteredStates(popupID.ToString());
            if (StateMachine.ContainsStateInActiveStates(popup.Name()))
            {
                IPopupAnimation popupAnimation = popup.PopupAnimation;
                if (popupAnimation == null)
                {
                    Exit(popupID, args);
                }
                else
                {
                    popupAnimation.OnAnimationEnd += () => { Exit(popupID, args); };
                    popupAnimation.PlayBackward();
                }
            }
            else
            {
                EditorDebug.LogWarning(popupID + " Not Contains State In Active States");
            }
        }

        public void Enter(PopupID popupID, params object[] args)
        {
            Enter(popupID.ToString(), args);
        }

        public void Enter(string name, params object[] args)
        {
            StateMachine.Enter(name, args);
        }

        public void Exit(PopupID popupID, params object[] args)
        {
            Exit(popupID.ToString(), args);
        }

        public void Exit(string name, params object[] args)
        {
            StateMachine.Exit(name, args);
        }

        public void RegisterState(Popup popup)
        {
            popup.gameObject.SetActive(false);
            popup.transform.SetParent(popupHolder);
            popup.transform.SetAsFirstSibling();

            StateMachine.RegisterState(popup);
        }

        public void UnRegisterState(Popup popup)
        {
            StateMachine.UnRegisterState(popup);
            Destroy(popup.gameObject);
        }


        private void OnPopupEntered(object sender, StateEventArgs<Popup> e)
        {
            Popup popup = e.state;
            popup.transform.SetAsLastSibling();
        }

        private void OnPopupExited(object sender, StateEventArgs<Popup> e)
        {

        }

        public bool CloseOpenedPopup()
        {
            Popup popup = StateMachine.CurrentState();
            if (popup == null || popup == default)
            {
                return false;
            }
            else
            {
                popup.Escape();
                return true;
            }
        }
    }
}