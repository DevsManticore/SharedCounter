using System.Collections;
using TMPro;
using UnityEngine;

namespace SharedCounter.Network
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject inputNamePanel = null;
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private GameObject hostLobbyButton = null;
        [SerializeField] private TMP_Text titleText = default;


        private void Awake()
        {
            titleText.text = "Shared Counter";
#if UNITY_WEBGL && !UNITY_EDITOR
            // cant be a server in webgl build
            hostLobbyButton.SetActive(false);
#endif
        }

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }

        private void HandleClientDisconnected()
        {
            inputNamePanel.SetActive(true);
            landingPagePanel.SetActive(false);
            StartCoroutine(ShowMessage("Invalid name!"));
        }

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }

        private IEnumerator ShowMessage(string message)
        {
            var oldText = titleText.text;
            titleText.text = message;

            yield return new WaitForSeconds(3f);

            titleText.text = oldText;
        }
    }
}
