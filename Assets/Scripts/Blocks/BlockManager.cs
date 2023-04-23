using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] BlockGrid { get; private set; }

    private Block currentHighlightBlock;
    [SerializeField]
    private int blockLayer = 6;
    public int BlockLayerMask { get; private set; }
    private int maxCost = 99;

    // NOTE: add these to different class?
    public List<List<Block>> currentMovementBlockPaths = new List<List<Block>>();
    public List<Block> currentAvailableMovementBlocks = new List<Block>();

    private PathFinding pathFinding;
    private FileManager fileManager;

    void Awake()
    {
        fileManager = FindObjectOfType<FileManager>();

        BlockLayerMask = 1 << blockLayer;
    }

    void Start()
    {
        // Use file to find dimensions ie. fileText array : ["000"]["000"]["000"] so 3x3 grid
        int xGridCount = fileManager.FileText[0].Length;
        int yGridCount = fileManager.FileText.Length;

        BlockGrid = new Block[xGridCount, yGridCount];
    }

    public void ResetPaths()
    {
        currentMovementBlockPaths.Clear();
        currentAvailableMovementBlocks.Clear();
    }

    public List<Block> FindPathWithBlock(List<List<Block>> blockPaths, Block block)
    {
        List<Block> pathWithBlock = new List<Block>();
        int blockPositionInList;
        foreach (List<Block> blockList in blockPaths)
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

    public Block FindBlockBelowEntity(Entity entity)
    {
        RaycastHit hit;
        Block block = null;
        if (Physics.Raycast(entity.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            block = hit.collider.GetComponentInParent<Block>();
        }
        return block;
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

    public void HighlightAllCells(bool b, List<Block> blocks)
    {
        foreach (Block block in blocks)
        {
            HighlightCellAvailable(block, b);
        }
    }

    public void PointerHighlight(Block block, List<Block> blocks)
    {
        if (!blocks.Contains(block) && currentHighlightBlock)
        {
            HighlightCellAvailable(currentHighlightBlock, true);
            HighlightCellHighlight(currentHighlightBlock, false);
            currentHighlightBlock = null;
        }
        if (!blocks.Contains(block) || currentHighlightBlock == block)
        {
            return;
        }
        else if (blocks.Contains(block))
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
}
