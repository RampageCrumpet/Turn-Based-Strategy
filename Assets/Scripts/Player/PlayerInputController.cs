using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInputStateMachine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(StateMachine))]
public class PlayerInputController : MonoBehaviour
{
    //Control scripts
    Player player;
    GameBoard gameBoard;

    [SerializeField]
    [Tooltip("The UnitMenu class we want to get Input from.")]
    public UnitMenu unitMenu;

    [SerializeField]
    [Tooltip("The GameMenu class we want to get Input from.")]
    public GameMenu gameMenu;

    [SerializeField]
    [Tooltip("The games ConstructionMenu")]
    public ConstructionMenu constructionMenu;

    [Header("Tile Indicator Sprites")]
    [SerializeField]
    [Tooltip("The sprite to be displayed on tiles the unit can move to.")]
    public Sprite moveIndicatorSprite;

    [SerializeField]
    [Tooltip("The sprite to be displayed on tiles the unit can attack.")]
    public Sprite attackIndicatorSprite;

    StateMachine playerInputStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<Player>();
        gameBoard = GameController.gameController.gameBoard;

        playerInputStateMachine = this.GetComponent<StateMachine>();
        playerInputStateMachine.ChangeState(new PlayerInputUnselected());

    }

}

