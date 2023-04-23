using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CurrentGameState
    {
        TurnStart,
        PlayerInput,
        ComputerInput,
        Moving,
        ActionStart,
        ActionSelect,
        TurnEnd
    }
    [HideInInspector]
    public CurrentGameState gameState;

    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Block currentEntityBlock;
    //[HideInInspector]
    public Action currentAction;

    private Camera cam;
    private BlockManager blockManager;
    private PlayerInput playerInput;
    private EntityManager entityManager;
    private UIManager uiManager;
    private PathFinding pathFinding;

    void Awake()
    {
        cam = Camera.main;
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        entityManager = FindObjectOfType<EntityManager>();
        uiManager = FindObjectOfType<UIManager>();
        pathFinding = new PathFinding();
    }

    void Start()
    {
        gameState = CurrentGameState.TurnStart;

        currentEntityTurn = entityManager.currentEntities[whichEntityTurn];
        uiManager.UpdateInfoPanelPlayer(currentEntityTurn);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);
        RaycastHit hit;

        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                currentEntityBlock = blockManager.FindBlockBelowEntity(currentEntityTurn);
                
                if (currentEntityTurn.currentMovementPoints != 0)
                {
                    blockManager.ResetPaths();
                    blockManager.currentAvailableMovementBlocks = pathFinding.AvailableMoves(currentEntityBlock, currentEntityTurn.currentMovementPoints, blockManager.BlockGrid, blockManager.currentMovementBlockPaths);
                    blockManager.HighlightAllCells(true, blockManager.currentAvailableMovementBlocks);
                }

                if (currentEntityTurn.playerControlled)
                {
                    uiManager.UpdateActions(currentEntityTurn.actionList);
                    gameState = CurrentGameState.PlayerInput;
                }
                else
                {
                    gameState = CurrentGameState.ComputerInput;
                }

                break;

            case CurrentGameState.PlayerInput:
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, blockManager.BlockLayerMask))
                {
                    Block hitBlock = hit.collider.GetComponentInParent<Block>();
                    blockManager.PointerHighlight(hitBlock, blockManager.currentAvailableMovementBlocks);
                    if (playerInput.MouseClick && blockManager.currentAvailableMovementBlocks.Contains(hitBlock))
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(blockManager.currentMovementBlockPaths, hitBlock);
                        blockManager.RemoveHighlightBlock();
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(currentEntityBlock);
                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.HighlightAllCells(false, blockManager.currentAvailableMovementBlocks);

                        blockManager.ResetPaths();
                        gameState = CurrentGameState.Moving;
                    }

                    Entity lookAtEntity = entityManager.FindEntityAboveBlock(hitBlock);
                    if (lookAtEntity && lookAtEntity != currentEntityTurn)
                    {
                        uiManager.UpdateInfoPanelOther(lookAtEntity);
                        uiManager.ToggleInfoPanelOther(true);
                    }
                    else
                    {
                        uiManager.ToggleInfoPanelOther(false);
                    }
                }
                else
                {
                    blockManager.RemoveHighlightBlock();
                }

                break;

            case CurrentGameState.ComputerInput:
                gameState = CurrentGameState.TurnEnd;

                break;

            case CurrentGameState.Moving:
                bool currentlyMoving = entityManager.CheckMoving();
                if (!currentlyMoving)
                {
                    gameState = CurrentGameState.TurnStart;
                }

                break;

            case CurrentGameState.ActionStart:
                //Debug.Log("where to attack");
                //gameState = CurrentGameState.TurnStart;

                break;

            case CurrentGameState.ActionSelect:
                break;

            case CurrentGameState.TurnEnd:
                blockManager.HighlightAllCells(false, blockManager.currentAvailableMovementBlocks);
                blockManager.RemoveHighlightBlock();
                currentEntityTurn.ResetValues();

                whichEntityTurn++;
                if (whichEntityTurn == entityManager.currentEntities.Count)
                {
                    whichEntityTurn = 0;
                }
                currentEntityTurn = entityManager.currentEntities[whichEntityTurn];
                gameState = CurrentGameState.TurnStart;
                
                break;
        }
    }
}
