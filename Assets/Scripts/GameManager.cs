using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTES: 
// try to avoid the use of 'null'

public class GameManager : MonoBehaviour
{
    public enum CurrentGameState
    {
        TurnStart,
        MoveStart,
        MoveInput,
        Moving,
        ActionStart,
        ActionInput,
        PerformingAction,
        TurnEnd
    }

    [HideInInspector]
    public CurrentGameState gameState;

    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Block currentEntityBlock;
    [HideInInspector]
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
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);
        RaycastHit blockHit;
        bool mouseOverBlock = Physics.Raycast(ray.origin, ray.direction, out blockHit, Mathf.Infinity, blockManager.BlockLayerMask);

        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                uiManager.UpdateInfoPanelPlayer(currentEntityTurn);
                currentEntityBlock = blockManager.FindBlockBelowEntity(currentEntityTurn);
                blockManager.DeactiveCells(blockManager.currentAvailableAttackBlocks);
                blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);
                blockManager.ClearAllLists();
                blockManager.currentAvailableMovementBlocks =
                    pathFinding.AvailableMoves(
                        currentEntityBlock, currentEntityTurn.currentMovementPoints, blockManager.BlockGrid, blockManager.currentMovementBlockPaths);
                blockManager.HighlightAndActiveCells(blockManager.currentAvailableMovementBlocks, blockManager.colorMove);

                if (currentEntityTurn.playerControlled)
                {
                    uiManager.UpdateActions(currentEntityTurn.actionList);
                    gameState = CurrentGameState.MoveStart;
                }
                else // skip computer turn for now
                {
                    gameState = CurrentGameState.TurnEnd;
                }

                break;

            case CurrentGameState.MoveStart:

                gameState = CurrentGameState.MoveInput;

                break;

            case CurrentGameState.MoveInput:
                if (mouseOverBlock)
                {
                    Block hitBlock = blockHit.collider.GetComponentInParent<Block>();
                    if (playerInput.MouseClick && blockManager.currentAvailableMovementBlocks.Contains(hitBlock)) // click to move
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(blockManager.currentMovementBlockPaths, hitBlock);
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(currentEntityBlock);
                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);
                        blockManager.currentHighlightBlock = null;

                        gameState = CurrentGameState.Moving;
                        break;
                    }

                    blockManager.HighlightingCell(hitBlock, blockManager.currentAvailableMovementBlocks, blockManager.colorMove);

                    // hover over an entity to see their stats
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
                    blockManager.RemoveHighlight(blockManager.colorMove);
                }

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
                blockManager.ClearAllLists();
                blockManager.currentAvailableAttackBlocks =
                    pathFinding.AvailablePositions(currentEntityBlock, currentAction.castRange, blockManager.BlockGrid);
                blockManager.HighlightAndActiveCells(blockManager.currentAvailableAttackBlocks, blockManager.colorAttackRange);

                gameState = CurrentGameState.ActionInput;

                break;

            case CurrentGameState.ActionInput:
                if (mouseOverBlock)
                {
                    Block hitBlock = blockHit.collider.GetComponentInParent<Block>();
                    if (playerInput.MouseClick)
                    {
                        // perform action
                        blockManager.DeactiveCells(blockManager.currentAvailableAttackBlocks);
                        blockManager.currentHighlightBlock = null;
                        currentAction = null;
                        gameState = CurrentGameState.PerformingAction;
                        break;
                    }

                    blockManager.HighlightingCell(hitBlock, blockManager.currentAvailableAttackBlocks, blockManager.colorAttackRange);
                }
                else
                {
                    blockManager.RemoveHighlight(blockManager.colorAttackRange);
                }

                break;

            case CurrentGameState.PerformingAction:
                gameState = CurrentGameState.TurnStart;

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
