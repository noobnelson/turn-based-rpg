using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    private List<Entity> entities = new List<Entity>();
    private List<Entity> spawnedEntities = new List<Entity>();
    [SerializeField]
    private List<Vector2Int> entitiesStartPosition = new List<Vector2Int>();
    [SerializeField]
    private Vector3 yPositionOffset = new Vector3(0, 0.5f, 0);
    private int whichEntityTurn = 0;
    private Entity currentEntityTurn;
    private Camera cam;

    private enum GameState
    {
        TurnStart,
        PlayerInput,
        ComputerInput,
        Moving,
        TurnEnd
    }
    private GameState gameState;

    private BlockManager blockManager;
    private PlayerInput playerInput;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Start()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            int x = entitiesStartPosition[i].x;
            int y = entitiesStartPosition[i].y;
            Block selectedBlock = blockManager.blockGrid[x, y];

            Vector3 entityPosition = selectedBlock.transform.position + yPositionOffset;
            Entity entity = Instantiate(entities[i], entityPosition, Quaternion.identity);
            entity.positionOnGrid = new Vector2Int(x, y);
            spawnedEntities.Add(entity);

            blockManager.AddEntity(selectedBlock, entity);
        }

        cam = Camera.main;
        gameState = GameState.TurnStart;

        currentEntityTurn = spawnedEntities[whichEntityTurn];
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(playerInput.MousePos);

        RaycastHit hit;

        switch (gameState)
        {
            case GameState.TurnStart:
                if (currentEntityTurn.currentMovementPoints != 0)
                {
                    blockManager.AvailableMoves(currentEntityTurn.positionOnGrid, currentEntityTurn.currentMovementPoints);
                    blockManager.HighlightAllCells(true);
                }
                
                gameState = GameState.PlayerInput;
                break;

            case GameState.PlayerInput:
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, blockManager.BlockLayerMask))
                {
                    Block hitBlock = hit.collider.GetComponentInParent<Block>();
                    blockManager.PointerHighlight(hitBlock);
                    if (playerInput.MouseClick && blockManager.AvailableBlocks.Contains(hitBlock))
                    {
                        Vector2Int blockPosition = currentEntityTurn.positionOnGrid;
                        Block entitiyBlock = blockManager.blockGrid[blockPosition.x, blockPosition.y];
                        blockManager.RemoveEntity(entitiyBlock);
                        blockManager.AddEntity(hitBlock, currentEntityTurn);
                        // search for it in blockPaths. use that list the block is in and the block's position to translate player
                        List<Block> blockPathToFollow = blockManager.FindPathWithBlock(hitBlock);
                        currentEntityTurn.transform.position = hitBlock.transform.position + yPositionOffset;
                        currentEntityTurn.currentMovementPoints -= blockPathToFollow.Count;
                        currentEntityTurn.positionOnGrid = new Vector2Int((int)hitBlock.transform.position.x, (int)hitBlock.transform.position.z);

                        blockManager.RemoveHighlightBlock();
                        gameState = GameState.Moving;
                    }
                }
                else
                {
                    blockManager.RemoveHighlightBlock();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameState = GameState.TurnEnd;
                }

                break;

            case GameState.ComputerInput:
                gameState = GameState.TurnEnd;
                break;

            case GameState.Moving:
                blockManager.ResetPaths();
                gameState = GameState.TurnStart;
                break;

            case GameState.TurnEnd:
                blockManager.ResetPaths();
                currentEntityTurn.ResetValues();
                //whichEntityTurn++;
                if (whichEntityTurn == spawnedEntities.Count)
                {
                    whichEntityTurn = 0;
                }
                currentEntityTurn = spawnedEntities[whichEntityTurn];
                gameState = GameState.TurnStart;
                break;
        }
    }
}
