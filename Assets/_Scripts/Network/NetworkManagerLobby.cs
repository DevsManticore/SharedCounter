using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SharedCounter.Network
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private SharedCounterGameplay sharedCounterPrefab;
        private SharedCounterGameplay sharedCounterInstance;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

        public override void OnStartServer()
        {
            base.OnStartServer();

            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        [Server]
        private void SpawnSharedCounter()
        {
            if (sharedCounterInstance != null) return;

            sharedCounterInstance = Instantiate(sharedCounterPrefab);
            NetworkServer.Spawn(sharedCounterInstance.gameObject);
        }

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

#if UNITY_WEBGL && !UNITY_EDITOR
            foreach (var player in RoomPlayers)
            {
                player.RpcPlayerJoined();
            }
#endif
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            OnServerStopped?.Invoke();

            RoomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) { return false; }

            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }

            return true;
        }

        public void StartGame()
        {
            if (!IsReadyToStart()) { return; }

            SpawnSharedCounter();
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }

        public bool IsNameValid(NetworkRoomPlayerLobby player, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return !RoomPlayers.Any(p => p != player && p.DisplayName == name);
        }
    }
}

