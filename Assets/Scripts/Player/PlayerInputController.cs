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
    public Player LocalPlayer { get; private set; }
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
        LocalPlayer = this.GetComponent<Player>();
        gameBoard = GameController.gameController.gameBoard;

        if (!LocalPlayer.hasAuthority)
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
        LocalPlayer.CmdIssueMoveOrder(position, LocalPlayer.SelectedUnit.gameObject);
    }

    public void IssueAttackOrder(Vector2Int position)
    {
        LocalPlayer.CmdIssueAttackOrder(position, LocalPlayer.SelectedUnit.gameObject);
    }

    public void IssueAbilityOrder(Ability ability, Unit invokingUnit)
    {
        LocalPlayer.CmdExecuteAbility(ability.name, invokingUnit.gameObject);
    }

    public void IssueEndTurnCommand()
    {
        LocalPlayer.CmdEndTurn();
    }
}

