using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInputStateMachine;
using Mirror;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(StateMachine))]
public class PlayerInputController : NetworkBehaviour
{
    //Control scripts
    Player player;
    GameBoard gameBoard;


    [Header("Prefabs")]
    public UnitMenu unitMenuPrefab;

    public GameMenu gameMenuPrefab;

    public ConstructionMenu constructionMenuPrefab;

    public UnitMenu UnitMenu { get; private set; }
    public GameMenu GameMenu { get; private set; }
    public ConstructionMenu ConstructionMenu { get; private set; }

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

        if (!player.hasAuthority)
        {
            return;
        }

        playerInputStateMachine = this.GetComponent<StateMachine>();
        playerInputStateMachine.ChangeState(new PlayerInputUnselected());

        Canvas canvas = FindObjectOfType<Canvas>();
        GameMenu = Object.Instantiate(gameMenuPrefab, canvas.transform).GetComponent<GameMenu>();
        ConstructionMenu = Object.Instantiate(constructionMenuPrefab, canvas.transform).GetComponent<ConstructionMenu>();
        UnitMenu = Object.Instantiate(unitMenuPrefab, canvas.transform).GetComponent<UnitMenu>();
    }

    public void IssueMoveOrder(Vector2Int position)
    {
        player.CmdIssueMoveOrder(position, player.SelectedUnit.gameObject);
    }

    public void IssueAttackOrder(Vector2Int position)
    {
        player.CmdIssueAttackOrder(position, player.SelectedUnit.gameObject);
    }
}

