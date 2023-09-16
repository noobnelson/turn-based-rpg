using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CurrentGameState
    {
        TurnStart,
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
    public Entity CurrentEntitySelected { get; private set; }
    private Block currentEntityBlock;
    [HideInInspector]
    public Action currentAction;

    private Camera cam;
    private BlockManager blockManager;
    private BlockCellColorChanger cellColorChanger;
    private PlayerInput playerInput;
    private EntityManager entityManager;
    private UIManager uiManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        entityManager = FindObjectOfType<EntityManager>();
        uiManager = FindObjectOfType<UIManager>();
        cellColorChanger = FindObjectOfType<BlockCellColorChanger>();

        cam = Camera.main;
        gameState = CurrentGameState.TurnStart;
    }

    void Start()
    {
        CurrentEntitySelected = entityManager.currentEntities[whichEntityTurn];
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
                uiManager.UpdateInfoPanelPlayer(CurrentEntitySelected);
                currentEntityBlock = blockManager.FindBlockBelowEntity(CurrentEntitySelected);
                blockManager.ResetAllCells();
                blockManager.availableMovementBlocks = PathFinding.AvailableMoves(
                    currentEntityBlock,
                    CurrentEntitySelected.currentMovementPoints,
                    blockManager.movementBlockPaths);
                cellColorChanger.HighlightAndActiveCells(
                    blockManager.availableMovementBlocks,
                    cellColorChanger.ColorMove);
                currentAction = null;

                if (CurrentEntitySelected.PlayerControlled)
                {
                    uiManager.UpdateActions(CurrentEntitySelected.ActionList);
                    gameState = CurrentGameState.MoveInput;
                }
                // skip computer turn for now
                else
                {
                    gameState = CurrentGameState.TurnEnd;
                }

                break;

            case CurrentGameState.MoveInput:
                if (mouseOverBlock)
                {
                    Block hitBlock = blockHit.collider.GetComponentInParent<Block>();
                    cellColorChanger.HighlightingCell(
                        hitBlock,
                        blockManager.availableMovementBlocks,
                        cellColorChanger.ColorMove);

                    if (playerInput.MouseClick
                        && blockManager.availableMovementBlocks.Contains(hitBlock))
                    {
                        List<Block> blockPathToFollow = PathFinding.FindPathWithBlock(
                            blockManager.movementBlockPaths,
                            hitBlock);
                        hitBlock.currentMovementCost = blockManager.MaxCost;
                        currentEntityBlock.currentMovementCost = currentEntityBlock.MovementCost;
                        entityManager.MoveEntity(CurrentEntitySelected, hitBlock, blockPathToFollow);
                        blockManager.DeactiveCells(blockManager.availableMovementBlocks);
                        cellColorChanger.RemoveHighlight();
                        gameState = CurrentGameState.Moving;

                        break;
                    }

                    // hover over an entity to see their stats
                    Entity lookAtEntityStats = entityManager.FindEntityAboveBlock(hitBlock);
                    if (lookAtEntityStats && lookAtEntityStats != CurrentEntitySelected)
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
                    cellColorChanger.RemoveHighlight(cellColorChanger.ColorMove);
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
                blockManager.availableAttackBlocks = PathFinding.AvailableAttackCells(
                    currentEntityBlock,
                    currentAction.CastRange);
                if (currentAction.ActionSelectSelf)
                {
                    blockManager.availableAttackBlocks.Add(currentEntityBlock);
                }
                cellColorChanger.HighlightAndActiveCells(
                    blockManager.availableAttackBlocks,
                    cellColorChanger.ColorAttackRange);
                gameState = CurrentGameState.ActionInput;

                break;

            case CurrentGameState.ActionInput:
                if (mouseOverBlock)
                {
                    Block hitBlock = blockHit.collider.GetComponentInParent<Block>();

                    if (playerInput.MouseClick
                        && CurrentEntitySelected.currentActionPoints >= currentAction.ActionCost)
                    {
                        CurrentEntitySelected.currentActionPoints -= currentAction.ActionCost;
                        if (currentAction.EffectSelf.Count > 0)
                        {
                            currentAction.PerformActionSelf(CurrentEntitySelected);
                        }
                        if (currentAction.EffectOther.Count > 0)
                        {
                            Entity hitEntity = entityManager.FindEntityAboveBlock(hitBlock);
                            if (hitEntity)
                            {
                                currentAction.PerformActionOther(hitEntity);
                            }
                        }

                        blockManager.DeactiveCells(blockManager.availableAttackBlocks);
                        cellColorChanger.RemoveHighlight();
                        currentAction = null;
                        gameState = CurrentGameState.PerformingAction;

                        break;
                    }

                    cellColorChanger.HighlightingCell(
                        hitBlock,
                        blockManager.availableAttackBlocks,
                        cellColorChanger.ColorAttackRange);
                }
                else
                {
                    cellColorChanger.RemoveHighlight(cellColorChanger.ColorAttackRange);
                }

                break;

            case CurrentGameState.PerformingAction:
                gameState = CurrentGameState.TurnStart;

                break;

            case CurrentGameState.TurnEnd:
                CurrentEntitySelected.ResetValues();
                whichEntityTurn++;
                if (whichEntityTurn == entityManager.currentEntities.Count)
                {
                    whichEntityTurn = 0;
                }
                CurrentEntitySelected = entityManager.currentEntities[whichEntityTurn];
                gameState = CurrentGameState.TurnStart;

                break;
        }
    }
}
