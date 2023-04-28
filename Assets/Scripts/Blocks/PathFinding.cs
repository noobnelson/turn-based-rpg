using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////// WIP CLASS ////////////////////////////
// Works but need to clean up code (ie. duplicate code)

public class PathFinding
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

    private List<Vector2Int> neighbourPositions =
        new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    // WIP: looks for available blocks, ALL block costs = 1, doesnt need a path
    public List<Block> AvailablePositions(Block startBlock, int maxCost, Block[,] blockGrid) 
    {
        List<Block> availablePositions = new List<Block>();
        availablePositions.Add(startBlock);
        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost =
                    new List<PathAndCost>() { new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            List<PathAndCost> newPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost pathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;

                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    Vector2Int lastBlockPosition = pathWithCost.blockPath[lastPositionInPath].positionOnGrid;

                    Vector2Int nextPathPosition = lastBlockPosition + position;

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

                    List<Block> newPath = new List<Block>(pathWithCost.blockPath);
                    accessedBlocks.Add(nextBlock);
                    int costToMoveToBlock = pathWithCost.cost + 1;

                    if (costToMoveToBlock > maxCost) // end of path. don't add block to path
                    {
                    }
                    else if (costToMoveToBlock == maxCost) // end of path. add block to path
                    {
                        newPath.Add(nextBlock);

                        availablePositions.Add(nextBlock);
                    }
                    else // continue path
                    {
                        newPath.Add(nextBlock);
                        PathAndCost newPathWithCost = new PathAndCost(newPath, costToMoveToBlock);
                        newPathsWithCostToAdd.Add(newPathWithCost);

                        availablePositions.Add(nextBlock);
                    }
                }
            }

            incompletePathsWithCost = newPathsWithCostToAdd;
        }
        return availablePositions;
    }

    // WIP: looks for available movements, block costs = cost of block ie. wall = 99, needs paths
    public List<Block> AvailableMoves(Block startBlock, int movementPoints, Block[,] blockGrid, List<List<Block>> blockPaths)
    {
        List<Block> availableBlocks = new List<Block>();
        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost =
            new List<PathAndCost>() { new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            List<PathAndCost> newPathsWithCostToAdd = new List<PathAndCost>();

            foreach (PathAndCost pathWithCost in incompletePathsWithCost) // go through each unfinished path
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;

                foreach (Vector2Int position in neighbourPositions) // check the 4 surroundings 
                {
                    Vector2Int lastBlockPosition = pathWithCost.blockPath[lastPositionInPath].positionOnGrid;

                    Vector2Int nextPathPosition = lastBlockPosition + position;

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

                    List<Block> newPath = new List<Block>(pathWithCost.blockPath);
                    accessedBlocks.Add(nextBlock);
                    int costToMoveToBlock = pathWithCost.cost + nextBlock.currentMovementCost;

                    if (costToMoveToBlock > movementPoints) // end of path. don't add block to path and remove starting block
                    {
                        blockPaths.Add(newPath.GetRange(1, newPath.Count - 1));
                    }
                    else if (costToMoveToBlock == movementPoints) // end of path. add block to path and remove starting block
                    {
                        newPath.Add(nextBlock);
                        blockPaths.Add(newPath.GetRange(1, newPath.Count - 1));

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
