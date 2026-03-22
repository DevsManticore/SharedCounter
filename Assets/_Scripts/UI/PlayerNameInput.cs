using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SharedCounter.WebGL;

namespace SharedCounter.UI
{
    public class PlayerNameInput : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;

        public static string DisplayName { get; private set; }

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string loadedName = string.Empty;
            string urlName = WebGLBridge.GetUrlParam("name");

            if (!string.IsNullOrEmpty(urlName))
            {
                loadedName = urlName;
            }
            else
            {
                loadedName = WebGLBridge.LoadName();
            }

            if (!string.IsNullOrEmpty(loadedName))
            {
                nameInputField.text = loadedName;
                SetPlayerName(loadedName);
            }
#else
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
#endif
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrWhiteSpace(name);
        }

        public void SavePlayerName()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            DisplayName = nameInputField.text;
            WebGLBridge.SaveName(DisplayName);
#else
            DisplayName = nameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
            PlayerPrefs.Save();
#endif
        }
    }
}
