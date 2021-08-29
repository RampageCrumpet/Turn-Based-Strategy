using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The games NetworkManager.")]
    private NetworkManagerLobby networkManager = null;

    [Header("User Interace")]
    [SerializeField]
    private GameObject landingPagePanel = null;

   

    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);

    }
}
