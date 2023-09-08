using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////// WIP CLASS ////////////////////////////
// Works but need to clean up code (ie. duplicate code)

public static class PathFinding
{
    private struct PathAndCost
    {
        public List<Block> blockPath;
        public int cost;

        public PathAndCost(List<Block> blockPath, int cost)
        {
            this.blockPath = blockPath;
            this.cost = cost;
        }
    }

    public static readonly List<Vector2Int> neighbourPositions = new List<Vector2Int>() { 
        Vector2Int.up, 
        Vector2Int.right, 
        Vector2Int.down, 
        Vector2Int.left };

    // WIP: looks for available blocks, ALL block costs = 1, doesnt need a path
    public static List<Block> AvailablePositions(Block startBlock, int maxCost, Block[,] blockGrid) 
    {
        List<Block> availableBlocks = new List<Block>();
        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { 
            new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            List<PathAndCost> newPathsWithCostToAdd = new List<PathAndCost>();
            // go through each unfinished path
            foreach (PathAndCost pathWithCost in incompletePathsWithCost) 
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;
                // check the 4 surroundings 
                foreach (Vector2Int position in neighbourPositions) 
                {
                    Vector2Int lastBlockPosition = pathWithCost
                        .blockPath[lastPositionInPath]
                        .positionOnGrid;

                    Vector2Int nextPathPosition = lastBlockPosition + position;

                    // check if the position is out of grid
                    if (nextPathPosition.x < 0 
                        || nextPathPosition.x >= blockGrid.GetLength(0)
                        || nextPathPosition.y < 0 
                        || nextPathPosition.y >= blockGrid.GetLength(1))
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
                    int costToMoveToBlock = pathWithCost.cost + 1; // uniq
                    // end of path. don't add block to path
                    if (costToMoveToBlock > maxCost) 
                    {
                    }
                    // end of path. add block to path
                    else if (costToMoveToBlock == maxCost) 
                    {
                        newPath.Add(nextBlock);
                        availableBlocks.Add(nextBlock);
                    }
                    // continue path
                    else 
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

    // WIP: looks for available movements, block costs = cost of block ie. wall = 99, needs paths
    public static List<Block> AvailableMoves(
        Block startBlock, 
        int maxCost, 
        Block[,] blockGrid, 
        List<List<Block>> blockPaths)
    {
        List<Block> availableBlocks = new List<Block>();
        List<Block> accessedBlocks = new List<Block>() { startBlock };
        List<PathAndCost> incompletePathsWithCost = new List<PathAndCost>() { 
            new PathAndCost(new List<Block>() { startBlock }, 0) };

        while (incompletePathsWithCost.Count > 0)
        {
            List<PathAndCost> newPathsWithCostToAdd = new List<PathAndCost>();
            // go through each unfinished path
            foreach (PathAndCost pathWithCost in incompletePathsWithCost) 
            {
                int lastPositionInPath = pathWithCost.blockPath.Count - 1;
                // check the 4 surroundings 
                foreach (Vector2Int position in neighbourPositions) 
                {
                    Vector2Int lastBlockPosition = pathWithCost
                        .blockPath[lastPositionInPath]
                        .positionOnGrid;

                    Vector2Int nextPathPosition = lastBlockPosition + position;

                    // check if the position is out of grid
                    if (nextPathPosition.x < 0 
                        || nextPathPosition.x >= blockGrid.GetLength(0)
                        || nextPathPosition.y < 0 
                        || nextPathPosition.y >= blockGrid.GetLength(1))
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
                    int costToMoveToBlock = pathWithCost.cost + nextBlock.currentMovementCost; // uniq
                    // end of path. don't add block to path and remove starting block
                    if (costToMoveToBlock > maxCost) 
                    {
                        blockPaths.Add(newPath.GetRange(1, newPath.Count - 1));
                    }
                    // end of path. add block to path and remove starting block
                    else if (costToMoveToBlock == maxCost) 
                    {
                        newPath.Add(nextBlock);
                        blockPaths.Add(newPath.GetRange(1, newPath.Count - 1));
                        availableBlocks.Add(nextBlock);
                    }
                    // continue path
                    else 
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
