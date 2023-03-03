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
    public List<List<Block>> CurrentBlockPaths { get; private set; } = new List<List<Block>>();

    public List<Block> AvailableMoves(Block startBlock, int movementPoints, Block[,] blockGrid)
    {
        CurrentBlockPaths.Clear();

        List<Block> availableBlocks = new List<Block>();
        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            List<PathAndCost> newPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost pathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;

                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    Vector2Int nextPathPosition = pathWithCost.blockPath[lastPositionInPath].positionOnGrid + position;

                    // check if the position is out of grid
                    if (nextPathPosition.x < 0 || nextPathPosition.x >= blockGrid.GetLength(0)
                        || nextPathPosition.y < 0 || nextPathPosition.y >= blockGrid.GetLength(1))
                    {
                        continue;
                    }

                    Block nextBlock = blockGrid[nextPathPosition.x, nextPathPosition.y];

                    // check if accessed block already
                    if (accessedBlocks.Contains(nextBlock))
                    {
                        continue;
                    }

                    int costToMoveToBlock = pathWithCost.cost + nextBlock.currentMovementCost;
                    List<Block> newPath = new List<Block>(pathWithCost.blockPath);
                    accessedBlocks.Add(nextBlock);

                    if (costToMoveToBlock > movementPoints) // end of path. don't add block to path and remove starting block
                    {
                        CurrentBlockPaths.Add(newPath.GetRange(1, newPath.Count - 1));
                    }
                    else if (costToMoveToBlock == movementPoints) // end of path. add block to path and remove starting block
                    {
                        newPath.Add(nextBlock);
                        CurrentBlockPaths.Add(newPath.GetRange(1, newPath.Count - 1));
                        
                        availableBlocks.Add(nextBlock);
                    }
                    else // continue path
                    {
                        newPath.Add(nextBlock);
                        PathAndCost newPathWithCost = new PathAndCost(newPath, costToMoveToBlock);
                        newPathsWithCostToAdd.Add(newPathWithCost);

                        availableBlocks.Add(nextBlock);
                    }
                }
            }

            incompletePathsWithCost = newPathsWithCostToAdd;
        }

        return availableBlocks;
    }
}
