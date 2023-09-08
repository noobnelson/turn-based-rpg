using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        cam = Camera.main;
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        entityManager = FindObjectOfType<EntityManager>();
        uiManager = FindObjectOfType<UIManager>();
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
        bool mouseOverBlock = Physics.Raycast(
            ray.origin, 
            ray.direction, 
            out blockHit, 
            Mathf.Infinity, 
            blockManager.BlockLayerMask);

        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                uiManager.UpdateInfoPanelPlayer(currentEntityTurn);
                currentEntityBlock = blockManager.FindBlockBelowEntity(currentEntityTurn);
                blockManager.ResetAllCells();
                blockManager.currentAvailableMovementBlocks = PathFinding.AvailableMoves(
                    currentEntityBlock, 
                    currentEntityTurn.currentMovementPoints, 
                    blockManager.BlockGrid, 
                    blockManager.currentMovementBlockPaths);
                blockManager.HighlightAndActiveCells(
                    blockManager.currentAvailableMovementBlocks, 
                    blockManager.ColorMove);
                currentAction = null;

                if (currentEntityTurn.PlayerControlled)
                {
                    uiManager.UpdateActions(currentEntityTurn.ActionList);
                    gameState = CurrentGameState.MoveStart;
                }
                // skip computer turn for now
                else 
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
                    if (playerInput.MouseClick 
                        && blockManager.currentAvailableMovementBlocks.Contains(hitBlock))
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(
                            blockManager.currentMovementBlockPaths, 
                            hitBlock);
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(currentEntityBlock);
                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.DeactiveCells(blockManager.currentAvailableMovementBlocks);
                        blockManager.currentHighlightBlock = null;
                        gameState = CurrentGameState.Moving;

                        break;
                    }

                    blockManager.HighlightingCell(
                        hitBlock, 
                        blockManager.currentAvailableMovementBlocks, 
                        blockManager.ColorMove);

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
                    blockManager.RemoveHighlight(blockManager.ColorMove);
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
                blockManager.ResetAllCells();
                blockManager.currentAvailableAttackBlocks = PathFinding.AvailablePositions(
                    currentEntityBlock, 
                    currentAction.CastRange, 
                    blockManager.BlockGrid);
                blockManager.HighlightAndActiveCells(
                    blockManager.currentAvailableAttackBlocks, 
                    blockManager.ColorAttackRange);
                gameState = CurrentGameState.ActionInput;

                break;

            case CurrentGameState.ActionInput:
                if (mouseOverBlock)
                {
                    Block hitBlock = blockHit.collider.GetComponentInParent<Block>();

                    if (playerInput.MouseClick 
                        && currentEntityTurn.currentActionPoints >= currentAction.ActionCost)
                    {
                        currentEntityTurn.currentActionPoints -= currentAction.ActionCost;

                        if (currentAction.EffectSelf.Count > 0)
                        {
                            currentAction.PerformActionSelf(currentEntityTurn);
                        }

                        if (currentAction.EffectOther.Count > 0)
                        {
                            Entity hitEntity = entityManager.FindEntityAboveBlock(hitBlock);
                            if (hitEntity)
                            {
                                currentAction.PerformActionOther(hitEntity);
                            }
                        }

                        blockManager.DeactiveCells(blockManager.currentAvailableAttackBlocks);
                        blockManager.currentHighlightBlock = null;
                        currentAction = null;
                        gameState = CurrentGameState.PerformingAction;
                        
                        break;
                    }

                    blockManager.HighlightingCell(
                        hitBlock, 
                        blockManager.currentAvailableAttackBlocks, 
                        blockManager.ColorAttackRange);
                }
                else
                {
                    blockManager.RemoveHighlight(blockManager.ColorAttackRange);
                }

                break;

            case CurrentGameState.PerformingAction:
                gameState = CurrentGameState.TurnStart;

                break;

            case CurrentGameState.TurnEnd:
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
