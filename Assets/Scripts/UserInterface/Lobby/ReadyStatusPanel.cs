using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReadyStatusPanel : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("The players ReadyPanel to be instantiated whenevr a player joins.")]
    PlayerPanel playerPanelPrefab;

    List<PlayerPanel> playerPanels = new List<PlayerPanel>();

    RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();

        if(roomManager == null)
        {
            Debug.LogError("ReadyStatusPanel cannot find NetworkManagerLobby");
            return;
        }

        int maxPlayers = FindObjectOfType<NetworkManager>().maxConnections;
        CreatePlayerPanels(maxPlayers);
    }
    public void CreatePlayerPanels(int maxPlayerCount)
    {
        for(int x = 0; x < maxPlayerCount; x++)
        {
            GameObject newPanelObject = Object.Instantiate(playerPanelPrefab.gameObject, this.transform);
            PlayerPanel playerPanel = newPanelObject.GetComponent<PlayerPanel>();
            playerPanel.OwningPlayer = null;
            playerPanels.Add(playerPanel);
        }
    }

    /// <summary>
    /// Finds the first unused player slot and adds our lobby player to it.
    /// </summary>
    /// <param name="player"></param>
    public void AddPlayer(RoomPlayer player)
    {
        foreach (PlayerPanel playerPanel in playerPanels)
        {
            if(playerPanel.OwningPlayer == null)
            {
                playerPanel.OwningPlayer = player;
                //Return so we don't make all of the blank PlayerPanels represent our player.
                return;
            }
        }

        Debug.LogError("No panel to display " + player.name + " ready status.");
    }

    public void RemovePlayer(RoomPlayer player)
    {

        foreach(PlayerPanel playerPanel in playerPanels)
        {
            if(playerPanel.OwningPlayer == player)
            {
                playerPanel.OwningPlayer = null;
                return;
            }
        }
    }

    public void ToggleLocalReady()
    {


        RoomPlayer localPlayer = NetworkClient.localPlayer.gameObject.GetComponent<RoomPlayer>();

        localPlayer.CmdChangeReadyState(!localPlayer.readyToBegin);
        
        /*if(localPlayer.readyToBegin)
        {
            localPlayer.
        }*/
    }


    public void ShowReadyStatus(RoomPlayer changingPlayer, bool readyStatus)
    {
       
        foreach (PlayerPanel playerPanel in playerPanels)
        {
            if (playerPanel.OwningPlayer == changingPlayer)
            {
                playerPanel.SetPlayerReady(readyStatus);
                return; //No need to search anymore. 
            }
        }

    }
}
