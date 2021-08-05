using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Installation : MonoBehaviour
{
    [field: SerializeField]
    public int Income { get; private set; }

    [field: SerializeField]
    public int CapturePoints {get; private set; } = 10;

    GameBoard gameBoard;

    Player owningPlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = GameController.gameController.gameBoard;
        gameBoard.AddInstallation(this, gameBoard.WorldToCell(this.transform.position));

        //Snap this object to the grid if it's not on it already.
        this.transform.position = this.transform.position.GetGridsnapPosition();

        name = "Capture";
    }

    public void CaptureInstallation(Player player)
    {
        if (player = null)
            throw new System.NullReferenceException();

        //Remove the installation from the old player.
        if (owningPlayer != null)
            owningPlayer.RemoveInstallation(this);

        player.AddInstallation(this);
        owningPlayer = player;
    }
}
