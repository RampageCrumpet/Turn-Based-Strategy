
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitMenu : MonoBehaviour
{

    RectTransform panel;

    [SerializeField]
    Button attackButton;

    [SerializeField]
    Button waitButton;

    [SerializeField]
    Button cancelButton;

    [SerializeField]
    Button specialButton;

    [SerializeField]
    Text specialText;

    public void Start()
    {
        panel = this.GetComponent<RectTransform>();

        this.gameObject.SetActive(false);


        //I'm not currently using the special button. It's being saved for later.
        specialButton.gameObject.SetActive(false);
    }


    public void HideUnitMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowUnitMenu(Vector2 mousePosition, bool showAttack)
    {
        //Move the Unit Menu to the mouse position.
        panel.position = mousePosition;

        this.gameObject.SetActive(true);
        attackButton.gameObject.SetActive(showAttack);
    }

    public void InitializeButtons(UnityAction attackCallback, UnityAction waitCallback, UnityAction cancelCallback, UnityAction specialCallback)
    {
        attackButton.onClick.AddListener(attackCallback);
        waitButton.onClick.AddListener(waitCallback);
        cancelButton.onClick.AddListener(cancelCallback);
        specialButton.onClick.AddListener(specialCallback);
    }
}
