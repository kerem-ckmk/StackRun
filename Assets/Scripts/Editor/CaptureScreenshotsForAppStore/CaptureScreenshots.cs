using UnityEditor;
using UnityEngine;

using System;
using System.Collections;
using System.Reflection;

using Kyusyukeigo.Helper;
using UnityExtensions;
using System.IO;

namespace takanakahiko.Utility {

    public class CaptureScreenshotsForAppStore : Editor {

        class GameViewSize : GameViewSizeHelper.GameViewSize {
            public GameViewSize(int width, int height, string baseText) {
                type = GameViewSizeHelper.GameViewSizeType.FixedResolution;
                this.width = width;
                this.height = height;
                this.baseText = baseText;
            }
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot1")]
        private static void CaptureScreenshot1() {
            EditorCoroutine.Start(CaptureScreenshot(1));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot2")]
        private static void CaptureScreenshot2() {
            EditorCoroutine.Start(CaptureScreenshot(2));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot3")]
        private static void CaptureScreenshot3() {
            EditorCoroutine.Start(CaptureScreenshot(3));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot4")]
        private static void CaptureScreenshot4()
        {
            EditorCoroutine.Start(CaptureScreenshot(4));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot5")]
        private static void CaptureScreenshot5()
        {
            EditorCoroutine.Start(CaptureScreenshot(5));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot6")]
        private static void CaptureScreenshot6()
        {
            EditorCoroutine.Start(CaptureScreenshot(6));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot7")]
        private static void CaptureScreenshot7()
        {
            EditorCoroutine.Start(CaptureScreenshot(7));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot8")]
        private static void CaptureScreenshot8()
        {
            EditorCoroutine.Start(CaptureScreenshot(8));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot9")]
        private static void CaptureScreenshot9()
        {
            EditorCoroutine.Start(CaptureScreenshot(9));
        }

        [MenuItem("CaptureScreenshots/CaptureScreenshot10")]
        private static void CaptureScreenshot10()
        {
            EditorCoroutine.Start(CaptureScreenshot(10));
        }

        static IEnumerator CaptureScreenshot(int number) {

            string directoryName = "screenshots";
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            var assembly = typeof(EditorWindow).Assembly;
            var groupType = currentSizeGroupType(assembly);
            var type = assembly.GetType("UnityEditor.GameView");
            var gameview = EditorWindow.GetWindow(type);

            GameViewSize[] customSizes =  {
                new GameViewSize(1242, 2688, "6.5"),
                new GameViewSize(1242, 2208, "5.5"),
                new GameViewSize(2048, 2732, "12.9"),
            };

            for (int i = 0; i < customSizes.Length; i++) {
                var customSize = customSizes[i];
                if (!GameViewSizeHelper.Contains(groupType, customSize))
                    GameViewSizeHelper.AddCustomSize(groupType, customSize);
                GameViewSizeHelper.ChangeGameViewSize(groupType, customSize);
                var filename = $"{directoryName}/{customSize.baseText}_{number}.png";
                EditorApplication.Step();
                EditorApplication.Step();
                ScreenCapture.CaptureScreenshot(filename);
                gameview.Repaint();
                Debug.Log(">> CaptureScreenshotsForAppStore : save to " + filename);
                //GameViewSizeHelper.RemoveCustomSize(groupType, customSize);
                yield return null;
            }
        }

        static GameViewSizeGroupType currentSizeGroupType(Assembly assembly) {
            Type gameView = assembly.GetType("UnityEditor.GameView");
            PropertyInfo currentSizeGroupType = gameView.GetProperty("currentSizeGroupType", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            return (GameViewSizeGroupType)currentSizeGroupType.GetValue(EditorWindow.GetWindow(gameView), null);
        }

    }
}
