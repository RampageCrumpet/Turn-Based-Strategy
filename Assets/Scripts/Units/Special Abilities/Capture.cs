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
        GameTile targetTile = gameBoard.GetTile(owner.Position);

        //There is no installation to capture.
        if (targetTile.installation == null)
        {
            return false;
        }

        //If the same player that owns the unit owns the installation they cannot capture it.
        if (owner.Player.Owns(targetTile.installation))
        {
            return false;
        }

        return true;
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
