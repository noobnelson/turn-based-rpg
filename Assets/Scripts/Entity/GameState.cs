using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Camera cam;

    private enum CurrentGameState
    {
        TurnStart,
        PlayerInput,
        ComputerInput,
        Moving,
        TurnEnd
    }
    private CurrentGameState gameState;

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
        Debug.Log(currentEntityTurn);
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
                    blockManager.AvailableMoves(currentEntityTurn.occupantBlock.positionOnGrid, currentEntityTurn.currentMovementPoints);
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
                        blockManager.RemoveEntity(currentEntityTurn.occupantBlock);
                        blockManager.AddEntity(hitBlock, currentEntityTurn);
                        // search for it in blockPaths. use that list the block is in and the block's position to translate player
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(hitBlock);
                        entityManager.MoveEntity(currentEntityTurn.occupantBlock, hitBlock, currentEntityTurn, blockPathToFollow);

                        blockManager.RemoveHighlightBlock();
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
