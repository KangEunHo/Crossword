using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;
using System.Collections;
using UnityEngine;

namespace HealingJam.Crossword.UI
{
    public class LoadingScreen : FadeScreen
    {
        [SerializeField] private ScaleTweenAnimation titleAnimation = null;

        public bool LoadedServerContents { get; private set; } = false;
        public enum LoginStep
        {
            None, LoginPopup, LoginGoogle, LoadGoogleData, End
        }

        private LoginStep loginStep = LoginStep.None;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            StartCoroutine(LoadingAll());

            GameMgr.Instance.topUIController.SetActiveBackButton(false);
            GameMgr.Instance.topUIController.SetActiveCoinButton(false);
            GameMgr.Instance.topUIController.SetActiveOptionButton(false);

            titleAnimation.Play();
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            GameMgr.Instance.topUIController.SetActiveBackButton(true);
            GameMgr.Instance.topUIController.SetActiveCoinButton(true);
            GameMgr.Instance.topUIController.SetActiveOptionButton(true);
        }

        private IEnumerator LoadingAll()
        {
            loginStep = LoginStep.None;

            // 1. 로컬의 세이브 데이터를 로드합니다.
            SaveMgr.Instance.Load(false, false, null);

            // 2. 구글 로그인을 사용한다면 로그인 후 데이터를 가져옵니다.
            if (SaveMgr.Instance.GetLoginType() == SaveData.LoginType.None)
            {
                loginStep = LoginStep.LoginPopup;
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Login, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f)
                   , new PopupClosedDelegate(LoginPopupCloseCallBack));
            }
            else
            {
                LoadGoogleSaveData();
            }

            // 3. 서버 콘텐츠를 로드합니다.
            //  (가로세로 에셋번들, 오늘의 상식)
            yield return StartCoroutine(LoadServerContents());

            while (loginStep != LoginStep.End)
            {
                yield return null;
            }

            // 4. 모든 작업이 끝났다면 타이틀로 이동합니다.
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }

        private void LoginPopupCloseCallBack(string key)
        {
            LoadGoogleSaveData();
        }

        private void LoadGoogleSaveData()
        {
            if (SaveMgr.Instance.GetLoginType() == SaveData.LoginType.Google)
            {
#if UNITY_EDITOR
                loginStep = LoginStep.End;
                return;
#endif
                GPGSMgr.Instance.Initialized((success) => {
                    loginStep = LoginStep.LoadGoogleData;
                    GPGSMgr.Instance.OpenSavedGame((openSuccess) => {
                        if (openSuccess)
                        {
                            SaveMgr.Instance.LoadGoogleGameService(true,
                                (loadSuccess) => { loginStep = LoginStep.End; }
                                );
                        }
                        else
                        {
                            loginStep = LoginStep.End;
                        }
                    });
                });
            }
            else
            {
                loginStep = LoginStep.End;
            }
        }

        private IEnumerator LoadServerContents()
        {
            yield return StartCoroutine(CrosswordMapManager.Instance.LoadCrosswordMapAtAssetBundle());

            CrosswordMapManager.Instance.SetUpDatabase();

            DailyCommonsensePopup dailyCommonsensePopup = PopupMgr.Instance.GetPopupById(Popup.PopupID.DailyCommonSense) as DailyCommonsensePopup;

            yield return StartCoroutine(dailyCommonsensePopup.LoadCommonSenseAsync());

            LoadedServerContents = true;
        }
    }
}