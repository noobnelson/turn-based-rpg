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
    public Entity currentEntityTurn;
    public Block currentEntityBlock;
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
        bool mouseOverBlock = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, blockManager.BlockLayerMask);
        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                currentEntityBlock = blockManager.FindBlockBelowEntity(currentEntityTurn);
                blockManager.DeactiveCells(blockManager.currentAvailableAttackBlocks);

                if (currentEntityTurn.currentMovementPoints != 0)
                {
                    blockManager.ResetPaths();
                    blockManager.currentAvailableMovementBlocks =
                        pathFinding.AvailableMoves(
                            currentEntityBlock, currentEntityTurn.currentMovementPoints, blockManager.BlockGrid, blockManager.currentMovementBlockPaths);
                    blockManager.HighlightCells(blockManager.currentAvailableMovementBlocks, blockManager.colorMove);
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
                if (mouseOverBlock)
                {
                    Block hitBlock = hit.collider.GetComponentInParent<Block>();
                    if (playerInput.MouseClick && blockManager.currentAvailableMovementBlocks.Contains(hitBlock)) // Click to move
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(blockManager.currentMovementBlockPaths, hitBlock);
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(currentEntityBlock);
                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);

                        blockManager.ResetPaths();
                        gameState = CurrentGameState.Moving;
                    }

                    if (blockManager.currentAvailableMovementBlocks.Contains(hitBlock))
                    {
                        if (blockManager.currentHighlightBlock)
                        {
                            if (blockManager.currentHighlightBlock != hitBlock)
                            {
                                blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorMove);
                                blockManager.PointerHighlight(hitBlock);
                            }
                        }
                        else
                        {
                            blockManager.PointerHighlight(hitBlock);
                        }
                    }
                    else
                    {
                        if (blockManager.currentHighlightBlock)
                        {
                            blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorMove);
                            blockManager.currentHighlightBlock = null;
                        }

                    }

                    // Hover over an entity to see their stats
                    Entity lookAtEntityStats = entityManager.FindEntityAboveBlock(hitBlock);
                    if (lookAtEntityStats && lookAtEntityStats != currentEntityTurn)
                    {
                        uiManager.UpdateInfoPanelOther(lookAtEntityStats);
                        uiManager.ToggleInfoPanelOther(true);
                    }
                    else
                    {
                        uiManager.ToggleInfoPanelOther(false);
                    }
                }
                else
                {
                    if (blockManager.currentHighlightBlock)
                    {
                        blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorMove);
                        blockManager.currentHighlightBlock = null;
                    }
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
                blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);
                blockManager.DeactiveCells(blockManager.currentAvailableAttackBlocks);
                blockManager.ResetPaths();
                blockManager.currentAvailableAttackBlocks =
                    pathFinding.AvailablePositions(currentEntityBlock, currentAction.castRange, blockManager.BlockGrid);
                blockManager.HighlightAndActiveCells(blockManager.currentAvailableAttackBlocks, blockManager.colorAttackRange);

                gameState = CurrentGameState.ActionSelect;

                break;

            case CurrentGameState.ActionSelect:
                if (mouseOverBlock)
                {
                    Block hitBlock = hit.collider.GetComponentInParent<Block>();

                    if (blockManager.currentAvailableAttackBlocks.Contains(hitBlock))
                    {
                        if (playerInput.MouseClick)
                        {
                            // perform action
                        }

                        if (blockManager.currentHighlightBlock)
                        {
                            if (blockManager.currentHighlightBlock != hitBlock)
                            {
                                blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorAttackRange);
                                blockManager.PointerHighlight(hitBlock);
                            }
                        }
                        else
                        {
                            blockManager.PointerHighlight(hitBlock);
                        }
                    }
                    else
                    {
                        if (blockManager.currentHighlightBlock)
                        {
                            blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorAttackRange);
                            blockManager.currentHighlightBlock = null;
                        }
                    }
                }
                else
                {
                    if (blockManager.currentHighlightBlock)
                    {
                        blockManager.HighlightCell(blockManager.currentHighlightBlock, blockManager.colorAttackRange);
                        blockManager.currentHighlightBlock = null;
                    }
                }
                break;

            case CurrentGameState.TurnEnd:
                blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);

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
