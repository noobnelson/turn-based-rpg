using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
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

    private List<Vector2Int> neighbourPositions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    public List<List<Vector2Int>> completedPaths = new List<List<Vector2Int>>();
    public List<List<Block>> BlockPaths { get; private set; } = new List<List<Block>>();
    
    private BlockManager blockManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
    }

    public void AvailableMoves(Block startBlock, int movementPoints)
    {
        int oldPathsCount = 0;
        Vector2Int currentPos = startBlock.positionOnGrid;

        List<Vector2Int> accessedBlocks = new List<Vector2Int>() { currentPos };
        PathAndCost initialPathAndCost = new PathAndCost(new List<Vector2Int>() { currentPos }, 0); // start position 
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { initialPathAndCost };
        while (incompletePathsWithCost.Count > 0)
        {
            oldPathsCount = incompletePathsWithCost.Count;
            List<PathAndCost> currentPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost currentPathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                int accessedBlockCount = 0;
                int lastPositionInPath = currentPathWithCost.path.Count - 1;

                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    Vector2Int nextPathPosition = currentPathWithCost.path[lastPositionInPath] + position;

                    // check if we've visited this block position or the position is out of grid
                    if (accessedBlocks.Contains(nextPathPosition) || nextPathPosition.x < 0 || nextPathPosition.x >= blockManager.blockGrid.GetLength(0) 
                        || nextPathPosition.y < 0 || nextPathPosition.y >= blockManager.blockGrid.GetLength(1))
                    {
                        accessedBlockCount += 1;
                        if (accessedBlockCount == 4)
                        {
                            completedPaths.Add(currentPathWithCost.path);
                        }
                        continue;
                    }
                    
                    Block currentBlock = blockManager.blockGrid[nextPathPosition.x, nextPathPosition.y];
                    int costToMoveToBlock = currentPathWithCost.cost + currentBlock.currentMovementCost;
                    List<Vector2Int> extendedPath = new List<Vector2Int>(currentPathWithCost.path);
                    accessedBlocks.Add(nextPathPosition);

                    if (costToMoveToBlock > movementPoints || accessedBlockCount == 4) // end of path. don't add block to path
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

            foreach (List<Vector2Int> path in completedPaths)
            {
                if (path.Count > 0)
                {
                    path.RemoveAt(0);
                }
                List<Block> currentBlockPath = new List<Block>();
                for (int i = 0; i < path.Count; i++)
                {
                    Block block = blockManager.blockGrid[path[i].x, path[i].y];
                    currentBlockPath.Add(block);
                    blockManager.AvailableBlocks.Add(block);
                }

                BlockPaths.Add(currentBlockPath);
            }
        }
    }
}
