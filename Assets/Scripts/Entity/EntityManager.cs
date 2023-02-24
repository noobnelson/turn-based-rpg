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

    private Camera cam;

    private enum GameState
    {
        TurnStart,
        PlayerInput,
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
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.TurnStart:
                blockManager.AvailableMoves(spawnedEntities[whichEntityTurn].positionOnGrid, spawnedEntities[whichEntityTurn].currentMovementPoints);
                blockManager.HighlightAllCells(true);

                gameState = GameState.PlayerInput;
                break;

            case GameState.PlayerInput:
                blockManager.PointerHighlight(playerInput.MousePos, cam);
                break;

            case GameState.Moving:
                blockManager.HighlightAllCells(false);

                gameState = GameState.TurnStart;
                break;

            case GameState.TurnEnd:
                whichEntityTurn++;
                if (whichEntityTurn == spawnedEntities.Count)
                {
                    whichEntityTurn = 0;
                }

                gameState = GameState.TurnStart;
                break;
        }   
    }
}
