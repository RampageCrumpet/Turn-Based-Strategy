using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayer : NetworkBehaviour
{
    [Header("User Interface")]
    [SerializeField]
    private GameObject lobbyUI = null;

    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton;

    [field: SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName { get; private set; } = "Loading...";

    [field: SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady { get; private set; } = false;

    public bool IsLeader 
    { 
        get
        {
            return IsLeader;
        }

        set 
        { 
            IsLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby Room
    {
        get
        {
            if (Room != null)
            {
                return Room;
            }
            else
                return Room = NetworkManager.singleton as NetworkManagerLobby;


        }

        set
        {
            Room = value;
        }
    }


    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }
    
    public void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        //This is a hacky solution to get around the fact that UpdateDisplay is called on their player object rather than
        //my active player. 
        if(!hasAuthority)
        {
            foreach(LobbyPlayer player in Room.RoomPlayers)
            {
                if(player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for(int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for(int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStrart)
    {
        if (IsLeader)
            return;

        startGameButton.interactable = readyToStrart;
    }


    /// <summary>
    /// Sets the players name. Validation should be done here.
    /// </summary>
    /// <param name="displayName"></param>
    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        if (!string.IsNullOrEmpty(displayName))
            DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        //If we're not the room leader we can't start the game.
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            return;
    }
}
