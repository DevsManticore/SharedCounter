using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharedCounter.Network
{
    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private Button stopHostButton = null;
        [SerializeField] private Button startGameButton = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        private void Awake()
        {
            SetVisibilityLobbyUI(false);
            stopHostButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(false);
        }

        public override void OnStartAuthority()
        {
            CmdTrySetDisplayName(PlayerNameInput.DisplayName);

            if (isServer)
            {
                stopHostButton.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(true);
            }

            SetVisibilityLobbyUI(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (!isOwned)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.isOwned)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }

                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting For Player...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CmdTrySetDisplayName(string displayName)
        {
            if (!Room.IsNameValid(this, displayName))
            {
                connectionToClient.Disconnect();
                return;
            }

            DisplayName = displayName;
        }

        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;

            HandleReadyStatusChanged(!IsReady, IsReady);

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            Room.StartGame();
        }

        public void SetVisibilityLobbyUI(bool on)
        {
            lobbyUI.SetActive(on);
        }

        public void OnStopClientClick()
        {
            Room.StopClient();
        }

        public void OnStopHostClick()
        {
            Room.StopHost();
        }
    }
}