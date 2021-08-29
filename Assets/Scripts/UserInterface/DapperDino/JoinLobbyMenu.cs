using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The games NetworkManager.")]
    private NetworkManagerLobby networkManager = null;

    [Header("User Interace")]
    [SerializeField]
    private GameObject landingPagePanel = null;

    [SerializeField]
    [Tooltip("The input field where the IP address is entered.")]
    private TMP_InputField ipAddressInputField;

    [SerializeField]
    [Tooltip("When this button is pressed we want to connect to the target machine.")]
    private Button joinButton = null;

    /*private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }*/

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
