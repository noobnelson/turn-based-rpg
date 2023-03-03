using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private enum CurrentGameState
    {
        TurnStart,
        PlayerInput,
        ComputerInput,
        Moving,
        Attacking,
        TurnEnd
    }
    private CurrentGameState gameState;

    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Camera cam;
    [HideInInspector]
    public bool moving;

    private BlockManager blockManager;
    private PlayerInput playerInput;
    private EntityManager entityManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        entityManager = FindObjectOfType<EntityManager>();
    }

    void Start()
    {
        cam = Camera.main;
        gameState = CurrentGameState.TurnStart;

        currentEntityTurn = entityManager.currentEntities[whichEntityTurn];
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);

        RaycastHit hit;

        switch (gameState)
        {
            case CurrentGameState.TurnStart:
                if (currentEntityTurn.currentMovementPoints != 0)
                {
                    Block currentBlock = blockManager.GetBlockBelowEntity(currentEntityTurn);
                    blockManager.FindAvailableMoves(currentBlock, currentEntityTurn.currentMovementPoints);
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
                        Block entityBlock = blockManager.GetBlockBelowEntity(currentEntityTurn);
                        blockManager.RemoveHighlightBlock();
                        blockManager.BlockCostMax(hitBlock);
                        blockManager.BlockCostReset(entityBlock);

                        entityManager.MoveEntity(currentEntityTurn, hitBlock, blockPathToFollow);
                        blockManager.ResetPaths();
                        gameState = CurrentGameState.Moving;
                    }
                }
                else
                {
                    blockManager.RemoveHighlightBlock();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameState = CurrentGameState.TurnEnd;
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
