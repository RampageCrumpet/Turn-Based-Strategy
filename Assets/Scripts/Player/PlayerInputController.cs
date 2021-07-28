using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInputController : MonoBehaviour
{
    private enum MenuState { unselected, selected, moved, unitMenu, gameMenu, attack}
    MenuState menuState = MenuState.unselected;

    //Control scripts
    Player player;
    GameBoard gameBoard;

    [SerializeField]
    [Tooltip("The UnitMenu class we want to get Input from.")]
    public UnitMenu unitMenu;

    [SerializeField]
    [Tooltip("The GameMenu class we want to get Input from.")]
    public GameMenu gameMenu;

    [Header("Tile Indicator Sprites")]
    [SerializeField]
    [Tooltip("The sprite to be displayed on tiles the player can move to.")]
    Sprite moveIndicatorSprite;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<Player>();
        gameBoard = GameController.gameController.gameBoard;

        //Hook the UnitMenu buttons up to functions
        unitMenu.InitializeButtons(UnitMenuAttack, UnitMenuWait,
             UnitMenuCancel, UnitMenuSpecial);

        gameMenu.InitializeButtons(GameMenuEndTurn, GameMenuCancel);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int tilePosition = gameBoard.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));


        switch (menuState)
        {
            //If the player clicks a 
            case MenuState.unselected:

                //Select the unit under the tile
                if(Input.GetMouseButtonDown(0))
                {
                    //Select the unit in that tile
                    player.SelectUnit(tilePosition);


                    if (player.SelectedUnit != null)
                    {
                        //We selected something.
                        menuState = MenuState.selected;
                    }
                    else
                    {
                        menuState = MenuState.gameMenu;
                        gameMenu.ShowGameMenu(Input.mousePosition);
                    }
                }


                break;


            case MenuState.selected:
                //Deselect our unit if we rightclick.
                if(Input.GetMouseButtonDown(1))
                {
                    player.DeselectUnit();
                    menuState = MenuState.unselected;
                }

                //If we click an empty space we want to move there, if we click a unit we want to display the UnitMenu.
                if(Input.GetMouseButton(0))
                {
                    //We clicked an empty space, we want to move to it. 
                    if (gameBoard.GetTile(tilePosition).unit == null)
                    {
                        player.IssueMoveOrder(tilePosition);
                        menuState = MenuState.moved;
                    }
                    else
                    {
                        //Display the UnitMenu
                    }
                }
                break;

            case MenuState.moved:
                //Deselect our unit if we rightclick.
                if (Input.GetMouseButtonDown(1))
                {
                    player.DeselectUnit();
                    menuState = MenuState.unselected;
                }

                //If we click open up the unit menu and change to it's state.
                if (Input.GetMouseButton(0))
                {
                    menuState = MenuState.unitMenu;

                    //TODO: This needs to show True if we can attack a unit. 
                    unitMenu.ShowUnitMenu(Input.mousePosition, false);
                }
                break;

            case MenuState.unitMenu:
                //Right click to escape the menu.
                if (Input.GetMouseButtonDown(1))
                {
                    UnitMenuWait();
                }
                break;

            case MenuState.gameMenu:
                if (Input.GetMouseButtonDown(1))
                {
                    GameMenuCancel();
                }
                break;
        }

    }



    //TODO: Should eventually move the unit ba
    private void UnitMenuCancel()
    {

    }

    private void UnitMenuAttack()
    {

    }

    private void UnitMenuSpecial()
    {

    }

    private void UnitMenuWait()
    {
        menuState = MenuState.unselected;
        player.DeselectUnit();
        unitMenu.HideUnitMenu();
    }


    private void GameMenuCancel()
    {
        gameMenu.HideGameMenu();
        menuState = MenuState.unselected;
    }

    private void GameMenuEndTurn()
    {
        player.EndTurn();
    }
}

