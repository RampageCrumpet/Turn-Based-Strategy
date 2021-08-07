using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The textbox to display the units name in.")]
    Text nameText;

    [SerializeField]
    [Tooltip("The textbox to display the units name in.")]
    Text costText;

    public Button button;

    public void Start()
    {
        button = this.GetComponent<Button>();
    }


    public string UnitName { get { return nameText.text; } set { nameText.text = value;} }

    public string Cost { get { return costText.text; } set { costText.text = value; } }
}
