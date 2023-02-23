using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public struct PathAndCost
    {
        public List<Vector2Int> path;
        public int cost;

        public PathAndCost(List<Vector2Int> path, int cost)
        {
            this.path = path;
            this.cost = cost;
        }
    }

    public Block[,] blockGrid;
    private List<Vector2Int> neighbourPositions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    private List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();

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
        completedPaths.Clear();
        int oldPathsCount = 0;

        List<Vector2Int> accessedBlocks = new List<Vector2Int>() { currentPos };
        PathAndCost initialPathAndCost = new PathAndCost(new List<Vector2Int>() { currentPos }, 0); // start position 
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { initialPathAndCost }; 

        while (incompletePathsWithCost.Count > 0) 
        {
            oldPathsCount = incompletePathsWithCost.Count;
            List<PathAndCost> currentPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost currentPathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    int lastPositionInPath = currentPathWithCost.path.Count - 1;
                    Vector2Int nextPathPosition = currentPathWithCost.path[lastPositionInPath] + position;

                    // check if we've visited this block position and if it is within the grid
                    if (!accessedBlocks.Contains(nextPathPosition) && nextPathPosition.x >= 0 && nextPathPosition.x < blockGrid.GetLength(0) && nextPathPosition.y > 0 && nextPathPosition.y < blockGrid.GetLength(1))
                    {
                        Block currentBlock = blockGrid[nextPathPosition.x, nextPathPosition.y];
                        int costToMoveToBlock = currentPathWithCost.cost + currentBlock.MovementCost;
                        List<Vector2Int> extendedPath = new List<Vector2Int>(currentPathWithCost.path);
                        accessedBlocks.Add(nextPathPosition);

                        if (costToMoveToBlock > movementPoints || currentBlock.occupantEntity) // end of path. don't add block to path
                        {
                            completedPaths.Add(currentPathWithCost.path);
                        }
                        else if (costToMoveToBlock == movementPoints) // end of path. add block to path
                        {
                            extendedPath.Add(nextPathPosition);
                            completedPaths.Add(extendedPath);
                        }
                        else // continue path
                        {
                            extendedPath.Add(nextPathPosition);
                            PathAndCost extendedPathWithCost = new PathAndCost(extendedPath, costToMoveToBlock);
                            currentPathsWithCostToAdd.Add(extendedPathWithCost);
                        }
                    }
                }
            }

            // remove the paths we just went through and add the new ones to continue
            for (int i = 0; i < oldPathsCount; i++)
            {
                incompletePathsWithCost.RemoveAt(0);
            }

            // if there are no more paths, we've searched all possible paths
            for (int x = 0; x < currentPathsWithCostToAdd.Count; x++)
            {
                incompletePathsWithCost.Add(currentPathsWithCostToAdd[x]);
            }
        }
    }
}
