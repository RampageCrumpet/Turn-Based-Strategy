using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    RectTransform panel;

    [SerializeField]
    Button endTurnButton;

    [SerializeField]
    Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        panel = this.GetComponent<RectTransform>();

        //Hide this menu.
        this.gameObject.SetActive(false);
    }


    public void HideGameMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowGameMenu(Vector2 displayPosition)
    {
        panel.position = displayPosition;
        this.gameObject.SetActive(true);
    }


    public void InitializeButtons(UnityAction endTurnCallback, UnityAction cancelCallback)
    {
        endTurnButton.onClick.AddListener(endTurnCallback);
        cancelButton.onClick.AddListener(cancelCallback);
    }


}
