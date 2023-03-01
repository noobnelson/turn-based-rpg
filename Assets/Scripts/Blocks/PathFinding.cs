using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public struct PathAndCost
    {
        public List<Block> blockPath;
        public int cost;

        public PathAndCost(List<Block> blockPath, int cost)
        {
            this.blockPath = blockPath;
            this.cost = cost;
        }
    }

    private List<Vector2Int> neighbourPositions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    public List<List<Block>> BlockPaths { get; private set; } = new List<List<Block>>();

    private BlockManager blockManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
    }

    public void AvailableMoves(Block startBlock, int movementPoints)
    {
        int oldPathsCount = 0;

        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            oldPathsCount = incompletePathsWithCost.Count;
            List<PathAndCost> currentPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost pathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;

                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    Vector2Int nextPathPosition = pathWithCost.blockPath[lastPositionInPath].positionOnGrid + position;

                    // check if we've visited this block position or the position is out of grid
                    if (nextPathPosition.x < 0 || nextPathPosition.x >= blockManager.blockGrid.GetLength(0)
                        || nextPathPosition.y < 0 || nextPathPosition.y >= blockManager.blockGrid.GetLength(1))
                    {
                        continue;
                    }

                    Block nextBlock = blockManager.blockGrid[nextPathPosition.x, nextPathPosition.y];

                    if (accessedBlocks.Contains(nextBlock))
                    {
                        continue;
                    }

                    int costToMoveToBlock = pathWithCost.cost + nextBlock.currentMovementCost;
                    List<Block> extendedPath = new List<Block>(pathWithCost.blockPath);
                    accessedBlocks.Add(nextBlock);

                    if (costToMoveToBlock > movementPoints) // end of path. don't add block to path
                    {
                        BlockPaths.Add(pathWithCost.blockPath);
                    }
                    else if (costToMoveToBlock == movementPoints) // end of path. add block to path
                    {
                        extendedPath.Add(nextBlock);
                        blockManager.AvailableBlocks.Add(nextBlock);
                        BlockPaths.Add(extendedPath);
                    }
                    else // continue path
                    {
                        extendedPath.Add(nextBlock);
                        blockManager.AvailableBlocks.Add(nextBlock);
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
        }
    }
}
