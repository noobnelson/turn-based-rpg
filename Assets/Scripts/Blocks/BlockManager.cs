using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] blockGrid;
    public List<Block> AvailableBlocks { get; private set; } = new List<Block>();

    private Block currentHighlightBlock;
    [SerializeField]
    private int blockLayer = 6;
    public int BlockLayerMask { get; private set; }
    private int maxCost = 99;

    private PathFinding pathFinding;

    void Awake()
    {
        pathFinding = FindObjectOfType<PathFinding>();

        BlockLayerMask = 1 << blockLayer;
    }

    public void BlockCostMax(Block block)
    {
        block.currentMovementCost = maxCost;
    }

    public void BlockCostReset(Block block)
    {
        block.currentMovementCost = block.MovementCost;
    }

    public void HighlightCellAvailable(Block block, bool b)
    {
        block.cellAvailable.gameObject.SetActive(b);
    }

    public void HighlightCellHighlight(Block block, bool b)
    {
        block.cellHighlight.gameObject.SetActive(b);
    }

    public void HighlightAllCells(bool b)
    {
        foreach (Block block in AvailableBlocks)
        {
            HighlightCellAvailable(block, b);
        }
    }

    public void PointerHighlight(Block block)
    {
        if (!AvailableBlocks.Contains(block) && currentHighlightBlock)
        {
            HighlightCellAvailable(currentHighlightBlock, true);
            HighlightCellHighlight(currentHighlightBlock, false);
            currentHighlightBlock = null;
        }
        if (!AvailableBlocks.Contains(block) || currentHighlightBlock == block)
        {
            return;
        }
        else if (AvailableBlocks.Contains(block))
        {
            if (currentHighlightBlock)
            {
                HighlightCellAvailable(currentHighlightBlock, true);
                HighlightCellHighlight(currentHighlightBlock, false);
            }

            currentHighlightBlock = block;
            HighlightCellAvailable(currentHighlightBlock, false);
            HighlightCellHighlight(currentHighlightBlock, true);

        }
    }

    public void RemoveHighlightBlock()
    {
        if (currentHighlightBlock)
        {
            HighlightCellAvailable(currentHighlightBlock, true);
            HighlightCellHighlight(currentHighlightBlock, false);
            currentHighlightBlock = null;
        }
    }

    public List<Block> FindPathWithBlock(Block block)
    {
        List<Block> pathWithBlock = new List<Block>();
        int blockPositionInList;
        foreach (List<Block> blockList in pathFinding.CurrentBlockPaths)
        {
            if (blockList.Contains(block))
            {
                blockPositionInList = blockList.IndexOf(block);
                pathWithBlock = blockList.GetRange(0, blockPositionInList + 1);
                return pathWithBlock;
            }
        }

        return pathWithBlock;
    }

    public void FindAvailableMoves(Block startBlock, int movementPoints)
    {
        AvailableBlocks = pathFinding.AvailableMoves(startBlock, movementPoints, blockGrid);
    }
    
    public Block GetBlockBelowEntity(Entity entity)
    {
        RaycastHit hit;
        Block block = null;
        if (Physics.Raycast(entity.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            block = hit.collider.GetComponentInParent<Block>();
        }
        return block;
    }

    public void ResetPaths()
    {
        HighlightAllCells(false);
        RemoveHighlightBlock();
        AvailableBlocks.Clear();
    }
}
