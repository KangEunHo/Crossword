using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HealingJam
{
    public class ToggleController : MonoBehaviour
    {
        public bool isOn;

        public Color onColorBg;
        public Color offColorBg;

        public Image toggleBgImage;
        public RectTransform toggle;

        public GameObject handle;
        private RectTransform handleTransform;

        private float handleSize;
        private float onPosX;
        private float offPosX;

        public float handleOffset;

        public GameObject onIcon;
        public GameObject offIcon;


        public float speed;
        private float t = 0.0f;

        public bool switching { get; private set; } = false;


        void Awake()
        {
            handleTransform = handle.GetComponent<RectTransform>();
            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleSize = handleRect.sizeDelta.x;
            float toggleSizeX = toggle.sizeDelta.x;
            onPosX = (toggleSizeX / 2) - (handleSize / 2) - handleOffset;
            offPosX = onPosX * -1;
        }

        public void SetPosition(bool isOn)
        {
            this.isOn = isOn;
            if (isOn)
            {
                if (toggleBgImage != null)
                    toggleBgImage.color = onColorBg;
                handleTransform.localPosition = new Vector3(onPosX, 0f, 0f);
                if (onIcon != null)
                    onIcon.gameObject.SetActive(true);
                if (offIcon != null)
                    offIcon.gameObject.SetActive(false);
            }
            else
            {
                if (toggleBgImage != null)
                    toggleBgImage.color = offColorBg;
                handleTransform.localPosition = new Vector3(offPosX, 0f, 0f);
                if (onIcon != null)
                    onIcon.gameObject.SetActive(false);
                if (offIcon != null)
                    offIcon.gameObject.SetActive(true);
            }
        }

        void Start()
        {
            SetPosition(isOn);
        }

        void Update()
        {
            if (switching)
            {
                Toggle(isOn);
            }
        }

        public void DoYourStaff()
        {
            //Debug.Log(isOn);
        }

        public void Switching()
        {
            switching = true;
        }



        public void Toggle(bool toggleStatus)
        {
            if (onIcon != null && offIcon != null)
            {
                if (!onIcon.activeSelf || !offIcon.activeSelf)
                {
                    onIcon.SetActive(true);
                    offIcon.SetActive(true);
                }
            }

            if (toggleStatus)
            {
                if (toggleBgImage != null)
                    toggleBgImage.color = SmoothColor(onColorBg, offColorBg);
                if (onIcon != null)
                    Transparency(onIcon, 1f, 0f);
                if (offIcon != null)
                    Transparency(offIcon, 0f, 1f);
                handleTransform.localPosition = SmoothMove(handle, onPosX, offPosX);
            }
            else
            {
                if (toggleBgImage != null)
                    toggleBgImage.color = SmoothColor(offColorBg, onColorBg);
                if (onIcon != null)
                    Transparency(onIcon, 0f, 1f);
                if (offIcon != null)
                    Transparency(offIcon, 1f, 0f);
                handleTransform.localPosition = SmoothMove(handle, offPosX, onPosX);
            }

        }


        Vector3 SmoothMove(GameObject toggleHandle, float startPosX, float endPosX)
        {

            Vector3 position = new Vector3(Mathf.Lerp(startPosX, endPosX, t += speed * Time.deltaTime), 0f, 0f);
            StopSwitching();
            return position;
        }

        Color SmoothColor(Color startCol, Color endCol)
        {
            Color resultCol;
            resultCol = Color.Lerp(startCol, endCol, t += speed * Time.deltaTime);
            return resultCol;
        }

        CanvasGroup Transparency(GameObject alphaObj, float startAlpha, float endAlpha)
        {
            CanvasGroup alphaVal;
            alphaVal = alphaObj.gameObject.GetComponent<CanvasGroup>();
            alphaVal.alpha = Mathf.Lerp(startAlpha, endAlpha, t += speed * Time.deltaTime);
            return alphaVal;
        }

        void StopSwitching()
        {
            if (t > 1.0f)
            {
                switching = false;

                t = 0.0f;
                switch (isOn)
                {
                    case true:
                        isOn = false;
                        DoYourStaff();
                        break;

                    case false:
                        isOn = true;
                        DoYourStaff();
                        break;
                }

            }
        }

    }
}