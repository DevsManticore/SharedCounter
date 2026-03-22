using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SharedCounter.WebGL
{
    public static class WebGLBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SavePlayerName(string name);

        [DllImport("__Internal")]
        private static extern IntPtr LoadPlayerName();

        [DllImport("__Internal")]
        private static extern void ShowAlert(string msg);

        [DllImport("__Internal")]
        private static extern IntPtr GetURLParam(string param);

        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);
#endif

        public static void SaveName(string name)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SavePlayerName(name);
#endif
        }

        public static string LoadName()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var ptr = LoadPlayerName();
            return Marshal.PtrToStringAnsi(ptr);
#else
            return string.Empty;
#endif
        }

        public static void Alert(string msg)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowAlert(msg);
#endif
        }

        public static string GetUrlParam(string param)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var ptr = GetURLParam(param);
            return Marshal.PtrToStringAnsi(ptr);
#else
            return string.Empty;
#endif
        }

        public static void Copy(string text)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            CopyToClipboard(text);
#endif
        }
    }
}