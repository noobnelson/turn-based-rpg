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
        Attacking,
        TurnEnd
    }
    [HideInInspector]
    public CurrentGameState gameState;

    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    [HideInInspector]
    public bool moving;
    private Block currentEntityBlock;

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
        uiManager.UpdateInfoPanelPlayer(currentEntityTurn);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);
        RaycastHit hit;

        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                currentEntityBlock = blockManager.BlockBelowEntity(currentEntityTurn);
                if (currentEntityTurn.currentMovementPoints != 0)
                {
                    blockManager.FindAvailableMoves(currentEntityBlock, currentEntityTurn.currentMovementPoints);
                    blockManager.HighlightAllCells(true);
                }

                if (currentEntityTurn.playerControlled)
                {
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
                    blockManager.PointerHighlight(hitBlock);
                    if (playerInput.MouseClick && blockManager.AvailableBlocks.Contains(hitBlock))
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(hitBlock);
                        blockManager.RemoveHighlightBlock();
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(currentEntityBlock);
                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.ResetPaths();
                        gameState = CurrentGameState.Moving;
                    }

                    Entity lookAtEntity = entityManager.EntityAboveBlock(hitBlock);
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
                if (!moving)
                {
                    gameState = CurrentGameState.TurnStart;
                }

                break;

            case CurrentGameState.Attacking:
                gameState = CurrentGameState.TurnStart;

                break;

            case CurrentGameState.TurnEnd:
                blockManager.ResetPaths();
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
