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
        }

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }
    }
}
