using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Installation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The income provided each turn to the owner of this property.")]
    public int income;

    GameBoard gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = GameController.gameController.gameBoard;
        gameBoard.AddInstallation(this, gameBoard.WorldToCell(this.transform.position));

        //Snap this object to the grid if it's not on it already.
        this.transform.position = this.transform.position.GetGridsnapPosition();
    }
}
