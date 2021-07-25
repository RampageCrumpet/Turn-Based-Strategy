using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button endTurnButton;
    public Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        //Hide this menu.
        this.gameObject.SetActive(false);

    }


    public void SetHidden(bool hidden)
    {
        this.gameObject.SetActive(hidden);
    }


}
