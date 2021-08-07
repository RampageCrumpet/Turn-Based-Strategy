using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Capture : Ability
{
    GameBoard gameBoard;
    Installation lastCapturedInstallation = null;

    int captureProgress = 0;



    public void Initialize(Unit owner, GameBoard gameBoard)
    {
        base.Initialize(owner);

        this.gameBoard = gameBoard;
        name = "Capture";
    }


    public override bool CanExecute()
    {
        if(gameBoard.GetTile(owner.Position).installation != null)
        {
            return true;
        }

        return false;
    }

    public override void Execute()
    {
        Installation target = gameBoard.GetTile(owner.Position).installation;

        if (target == null)
            throw new System.NullReferenceException();

        if (target == lastCapturedInstallation)
        {
            captureProgress += owner.HitPoints;
            if(target.CapturePoints <= captureProgress)
            {
                target.CaptureInstallation(owner.Player);
            }
        }
        else
        {
            captureProgress = owner.HitPoints;
        }
            
    }


}
