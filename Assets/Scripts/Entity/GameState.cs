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
        TurnEnd
    }
    private CurrentGameState gameState;

    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Camera cam;

    private BlockManager blockManager;
    private PlayerInput playerInput;
    private EntitySpawner entitySpawner;
    private EntityManager entityManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        entityManager = FindObjectOfType<EntityManager>();
        entitySpawner = FindObjectOfType<EntitySpawner>();
    }

    void Start()
    {
        cam = Camera.main;
        gameState = CurrentGameState.TurnStart;

        currentEntityTurn = entitySpawner.spawnedEntities[whichEntityTurn];
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
                    Block currentBlock = entityManager.GetBlockBelowEntity(currentEntityTurn);
                    blockManager.AvailableMoves(currentBlock, currentEntityTurn.currentMovementPoints);
                    blockManager.HighlightAllCells(true);
                }

                gameState = CurrentGameState.PlayerInput;
                break;

            case CurrentGameState.PlayerInput:
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, blockManager.BlockLayerMask))
                {
                    Block hitBlock = hit.collider.GetComponentInParent<Block>();
                    blockManager.PointerHighlight(hitBlock);
                    if (playerInput.MouseClick && blockManager.AvailableBlocks.Contains(hitBlock))
                    {
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(hitBlock);
                        Block occupiedBlock = entityManager.GetBlockBelowEntity(currentEntityTurn);
                        entityManager.MoveEntity(hitBlock, currentEntityTurn, blockPathToFollow);
 
                        blockManager.RemoveHighlightBlock();
                        blockManager.BlockCostUpdate(hitBlock, 99);
                        blockManager.BlockCostReset(occupiedBlock);
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
                blockManager.ResetPaths();
                gameState = CurrentGameState.TurnStart;
                break;

            case CurrentGameState.TurnEnd:
                blockManager.ResetPaths();
                currentEntityTurn.ResetValues();
                //whichEntityTurn++;
                if (whichEntityTurn == entitySpawner.spawnedEntities.Count)
                {
                    whichEntityTurn = 0;
                }
                currentEntityTurn = entitySpawner.spawnedEntities[whichEntityTurn];
                gameState = CurrentGameState.TurnStart;
                break;
        }
    }
}
