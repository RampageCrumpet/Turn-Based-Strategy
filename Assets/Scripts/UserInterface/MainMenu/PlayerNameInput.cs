using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerNameInput : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The text input the players name is entered into")]
    [SerializeField]
    private TMP_InputField nameInputField = null;

 

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private const string defaultName = "Player";

    Event OnDeselectTextbox;

    private void Start()
    {
        nameInputField.onEndEdit.AddListener((string arg) => SavePlayerName());
    }

    private void SetUpInputField()
    {
        string name = defaultName;

        //If no playername is set in the PlayerPrefs we can't load one. Return.
        if (PlayerPrefs.HasKey(PlayerPrefsNameKey)) 
        {
            name = PlayerPrefs.GetString(PlayerPrefsNameKey);
        }

        nameInputField.text = name;
    }



    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        //If the name is valid save it. Otherwise get the last saved name.
        if (!string.IsNullOrEmpty(name))
        {
            PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        }
        else
        {
            nameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
        }
        
    }



}
