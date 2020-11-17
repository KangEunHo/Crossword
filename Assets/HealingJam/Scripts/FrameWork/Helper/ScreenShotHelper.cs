using UnityEngine;
using System.IO;

namespace HealingJam
{
    public class ScreenShotHelper : MonoBehaviour
    {
        public bool isTransparent = false;
        public bool saveProjectPath = true;
        public string path;
        public KeyCode shotKeyCode = KeyCode.Q;

#if UNITY_EDITOR
        private void Start()
        {
            if (saveProjectPath)
                path = Path.Combine(Application.dataPath.Replace("Assets", ""), path);
        }

        private void Update()
        {
            if (Input.GetKeyDown(shotKeyCode))
            {
                TakeScreenShot();
            }
        }
#endif

        public void TakeScreenShot()
        {
            foreach (var v in FindObjectsOfType<Canvas>())
            {
                v.renderMode = RenderMode.ScreenSpaceCamera;
                v.worldCamera = Camera.main;
            }

            int resWidthN = Screen.width;
            int resHeightN = Screen.height;
            RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
            Camera camera = Camera.main;
            camera.targetTexture = rt;

            TextureFormat tFormat;
            if (isTransparent)
                tFormat = TextureFormat.ARGB32;
            else
                tFormat = TextureFormat.RGB24;


            Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidthN, resHeightN);

            File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));

            foreach (var v in FindObjectsOfType<Canvas>())
            {
                v.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

        private string ScreenShotName(int width, int height)
        {

            string strPath = "";

            strPath = string.Format("{0}/screen_{1}x{2}_{3}.png",
                                 path,
                                 width, height,
                                           System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

            return strPath;
        }
    }
}