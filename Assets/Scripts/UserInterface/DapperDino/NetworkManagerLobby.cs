using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField]
    [Tooltip("The minimum number of players required for the game to start.")]
    private int minPlayers = 2;


    [Scene]
    [SerializeField]
    [Tooltip("The lobby scene.")]
    private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField]
    [Tooltip("The lobby player prefab.")]
    private LobbyPlayer roomPlayerPrefab = null;

    public static event Action<NetworkConnection> OnClientConnected;
    public static event Action<NetworkConnection> OnClientDisconnected;

    public List<LobbyPlayer> RoomPlayers { get; } = new List<LobbyPlayer>();

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //If OnClientConnected is not null invoke it.
        OnClientConnected?.Invoke(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        //If  OnClientDisconnected is not null invoke it.
        OnClientDisconnected?.Invoke(conn);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        //If we have too many players disconnect the newly connected player.
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        //If we're not in our lobby scene disconnect the newly connected player.
        if(SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            LobbyPlayer player = conn.identity.GetComponentInChildren<LobbyPlayer>();
            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            LobbyPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
   
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach(LobbyPlayer player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
            return false;

        foreach(LobbyPlayer player in RoomPlayers)
        {
            if (!player.IsReady)
                return false;
        }

        return true;
    }
}
