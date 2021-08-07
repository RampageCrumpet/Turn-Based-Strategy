
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

    List<Button> abilityButtons = new List<Button>();

    public void Start()
    {
        panel = this.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }


    public void HideUnitMenu()
    {
        this.gameObject.SetActive(false);

        foreach (Button button in abilityButtons)
        {
            
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }

        abilityButtons.Clear();
    }

    public void ShowUnitMenu(Vector2 displayPosition, bool showAttack)
    {
        //Move the Unit Menu to the mouse position.
        panel.position = displayPosition;

        this.gameObject.SetActive(true);
        attackButton.gameObject.SetActive(showAttack);
    }

    public void InitializeStandardButtons(UnityAction attackCallback, UnityAction waitCallback, UnityAction cancelCallback)
    {
        attackButton.onClick.AddListener(attackCallback);
        waitButton.onClick.AddListener(waitCallback);
        cancelButton.onClick.AddListener(cancelCallback);
    }

    public void InitializeAbilityButton(Ability ability, UnityAction specialCallback)
    {
        GameObject buttonObject = ObjectPooler.objectPooler.GetPooledObject("Button");
        Button button = buttonObject.GetComponent<Button>();

        buttonObject.GetComponentInChildren<Text>().text = ability.name;

        button.onClick.AddListener(ability.Execute);
        button.onClick.AddListener(specialCallback);
        buttonObject.transform.SetParent(panel);
        abilityButtons.Add(button);

        buttonObject.SetActive(true);
    }
}
