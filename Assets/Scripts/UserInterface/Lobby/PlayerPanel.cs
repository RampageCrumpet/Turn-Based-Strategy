using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text PlayerNameText;

    [SerializeField]
    private TMP_Text PlayerReadyText;

    //The Player this panel is representing.
    private RoomPlayer owningPlayer;
    public RoomPlayer OwningPlayer
    {
        get { return owningPlayer; }
        set
        {
            owningPlayer = value;
            if(value == null)
            {
                PlayerNameText.text = "";
                PlayerReadyText.text = "";
            }
            else
            {
                SetPlayerName(value.name);
                SetPlayerReady(value.readyToBegin);
            }
        }
    }
    public void SetPlayerName(string name)
    {
        PlayerNameText.text = name;
    }

    public void SetPlayerReady(bool ready)
    {
       
        if (ready)
        {
            PlayerReadyText.color = Color.green;
            PlayerReadyText.text = "Ready";
        }
        else
        {
            PlayerReadyText.color = Color.red;
            PlayerReadyText.text = "Not Ready";
        }
    }
}
