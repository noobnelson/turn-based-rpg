using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] blockGrid;
    public int[,] blockGridCosts;

    private EntityManager entityManager;

    void Awake()
    {
        entityManager = FindObjectOfType<EntityManager>();
    }

    public void AddEntity(Block block, Entity entity)
    {
        block.occupantEntity = entity;
    }

    public void RemoveEntity(Block block)
    {
        block.occupantEntity = null;
    }

    public void HighlightCellAvailable(Block block, bool b)
    {
        block.cellAvailable.gameObject.SetActive(b);
    }

    public void HighlightCellUnavailable(Block block, bool b)
    {
        block.cellUnvailable.gameObject.SetActive(b);
    }

    public void AvailableMovement(Vector2Int entityPosition)
    {

        int currentX = entityPosition.x;
        int currentZ = entityPosition.y;
        Block currentBlock = blockGrid[currentX, currentZ];

        int movementPoints = currentBlock.occupantEntity.CurrentMovementPoints;
        Debug.Log(movementPoints);
        for (int i = 1; i < movementPoints + 1; i++)
        {
            Vector2Int north = new Vector2Int(Mathf.Clamp(currentX, 0, blockGrid.GetLength(0) - 1), Mathf.Clamp(currentZ - i, 0, blockGrid.GetLength(1) - 1));
            Vector2Int east = new Vector2Int(Mathf.Clamp(currentX + i, 0, blockGrid.GetLength(0) - 1), Mathf.Clamp(currentZ, 0, blockGrid.GetLength(1) - 1));
            Vector2Int south = new Vector2Int(Mathf.Clamp(currentX, 0, blockGrid.GetLength(0) - 1), Mathf.Clamp(currentZ + i, 0, blockGrid.GetLength(1) - 1));
            Vector2Int west = new Vector2Int(Mathf.Clamp(currentX - i, 0, blockGrid.GetLength(0) - 1), Mathf.Clamp(currentZ, 0, blockGrid.GetLength(1) - 1));

            HighlightCellAvailable(blockGrid[north.x, north.y], true);
            HighlightCellAvailable(blockGrid[east.x, east.y], true);
            HighlightCellAvailable(blockGrid[south.x, south.y], true);
            HighlightCellAvailable(blockGrid[west.x, west.y], true);

            // Mathf.Clamp(0, 0, blockGrid.GetLength(0)); // rows/height/x
            // Mathf.Clamp(0, 0, blockGrid.GetLength(1)); // columns/width/z
        }
    }

    public void AvailableMoves(Vector2Int currentPos, int[,] costGrid, int movementPoints)
    {
        List<Vector2Int> neighbourBlocks = new List<Vector2Int>();
        neighbourBlocks.Add(new Vector2Int(0, -1)); // North
        neighbourBlocks.Add(new Vector2Int(1, 0)); // East
        neighbourBlocks.Add(new Vector2Int(0, 1)); // South
        neighbourBlocks.Add(new Vector2Int(-1, 0)); // West

        List<Vector2Int> accessedBlocks = new List<Vector2Int>();
        Queue<List<Vector2Int>> currentPaths = new Queue<List<Vector2Int>>();
        List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();
        accessedBlocks.Add(currentPos);

        currentPaths.Enqueue(new List<Vector2Int>() {currentPos});
        int dequeueCount = 0;

        while (currentPaths.Count > 0)
        {
            dequeueCount = currentPaths.Count;
            List<List<Vector2Int>> listToAdd = new List<List<Vector2Int>>();

            foreach (List<Vector2Int> list in currentPaths)
            {
                foreach (Vector2Int position in neighbourBlocks)
                {
                    Vector2Int value = list[list.Count - 1] + position;
                    if (value.x < 0 || value.x > costGrid.GetLength(0) - 1 || value.y < 0 || value.y > costGrid.GetLength(1) - 1 || accessedBlocks.Contains(value))
                    {

                    }
                    else
                    {
                        List<Vector2Int> newList = new List<Vector2Int>(list);
                        newList.Add(value);

                        if (newList.Count == movementPoints + 1)
                        {
                            completedPaths.Add(newList);
                            Debug.Log("Completed");
                        }
                        else
                        {
                            listToAdd.Add(newList);
                        }

                        accessedBlocks.Add(value);
                    }
                }
            }

            for (int i = 0; i < dequeueCount; i++)
            {
                currentPaths.Dequeue();
            }

            for (int x = 0; x < listToAdd.Count; x++)
            {
                currentPaths.Enqueue(listToAdd[x]);
            }
        }

        foreach (Vector2Int vector in accessedBlocks)
        {
            HighlightCellAvailable(blockGrid[vector.x, vector.y], true);
        }
    }
}
