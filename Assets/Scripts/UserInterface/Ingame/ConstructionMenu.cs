using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionMenu : MonoBehaviour
{
    RectTransform panel;

    List<ConstructionButton> constructionButtons = new List<ConstructionButton>();


    // Start is called before the first frame update
    void Start()
    {
        panel = this.GetComponent<RectTransform>();

        //Hide this menu.
        this.gameObject.SetActive(false);
    }

    public void HideConstructionMenu()
    {
        this.gameObject.SetActive(false);

        foreach (ConstructionButton constructionButton in constructionButtons)
        {
            //Return the buttons to the pool.
            constructionButton.button.gameObject.SetActive(false);
            constructionButton.button.onClick.RemoveAllListeners();
        }

        constructionButtons.Clear();
    }

    public void ShowConstructionMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void InitializeConstructionButton(UnityAction<string, Vector3> constructionCallback,UnityAction stateMachineCallback, Unit unit, Vector3 constructionPosition)
    {
        GameObject buttonObject = ObjectPooler.objectPooler.GetPooledObject("ConstructionButton");
        ConstructionButton constButton = buttonObject.GetComponent<ConstructionButton>();

        constButton.UnitName = unit.name;
        constButton.Cost = unit.Cost.ToString();

        constButton.button.onClick.AddListener(() => constructionCallback(unit.gameObject.name, constructionPosition));
        constButton.button.onClick.AddListener(stateMachineCallback);
        buttonObject.transform.SetParent(panel);
        constructionButtons.Add(constButton);

        buttonObject.SetActive(true);
    }
}
