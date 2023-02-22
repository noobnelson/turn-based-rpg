using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] blockGrid;
    private List<Vector2Int> neighbourBlocks = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
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

    public void AvailableMoves(Vector2Int currentPos, int movementPoints)
    {
        List<Vector2Int> accessedBlocks = new List<Vector2Int>();
        List<List<Vector2Int>> incompletePaths = new List<List<Vector2Int>>();
        List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();

        accessedBlocks.Add(currentPos);
        incompletePaths.Add(new List<Vector2Int>() { currentPos });
        int oldPathsCount = 0;

        while (incompletePaths.Count > 0)
        {
            oldPathsCount = incompletePaths.Count;
            List<List<Vector2Int>> currentPathToAdd = new List<List<Vector2Int>>();

            foreach (List<Vector2Int> currentPath in incompletePaths)
            {
                foreach (Vector2Int position in neighbourBlocks)
                {
                    Vector2Int nextPathPosition = currentPath[currentPath.Count - 1] + position;
                    if (nextPathPosition.x >= 0 && nextPathPosition.x < blockGrid.GetLength(0) && nextPathPosition.y > 0 && nextPathPosition.y < blockGrid.GetLength(1) && !accessedBlocks.Contains(nextPathPosition))
                    {
                        List<Vector2Int> extendedPath = new List<Vector2Int>(currentPath);
                        extendedPath.Add(nextPathPosition);

                        if (extendedPath.Count == movementPoints + 1)
                        {
                            completedPaths.Add(extendedPath);
                        }
                        else
                        {
                            currentPathToAdd.Add(extendedPath);
                        }
                        accessedBlocks.Add(nextPathPosition);
                    }
                }
            }

            for (int i = 0; i < oldPathsCount; i++)
            {
                incompletePaths.RemoveAt(0);
            }

            for (int x = 0; x < currentPathToAdd.Count; x++)
            {
                incompletePaths.Add(currentPathToAdd[x]);
            }
        }

        foreach (Vector2Int vector in accessedBlocks)
        {
            Block currentBlock = blockGrid[vector.x, vector.y];
            if (currentBlock.MovementCost == 1)
            {
                HighlightCellAvailable(currentBlock, true);
            }
        }
    }
}
