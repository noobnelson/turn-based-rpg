using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] BlockGrid { get; private set; }

    [HideInInspector]
    public Block currentHighlightBlock;
    [SerializeField]
    private int blockLayer = 6;
    public int BlockLayerMask { get; private set; }
    private int maxCost = 99;

    public Color colorMove;
    public Color colorPointer;
    public Color colorAttackRange;
    public Color colorAttackArea;
    private Color previousColorOfBlock;

    // NOTE: add these to different class?
    [HideInInspector]
    public List<List<Block>> currentMovementBlockPaths = new List<List<Block>>();
    [HideInInspector]
    public List<Block> currentAvailableMovementBlocks = new List<Block>();
    [HideInInspector]
    public List<Block> currentAvailableAttackBlocks = new List<Block>();
    [HideInInspector]
    public List<Block> currentAvailableAreaAttackBlocks = new List<Block>();

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

    // CELL 
    public void ChangeCellColor(Color color, Renderer renderer, MaterialPropertyBlock propBlock)
    {
        renderer.GetPropertyBlock(propBlock); // get current value of material properties in renderer
        propBlock.SetColor("_Color", color); // assign new value
        renderer.SetPropertyBlock(propBlock); // apply edited value to renderer
    }

    public void HighlightCell(Block block, Color color)
    {
        ChangeCellColor(color, block.cellWithMaterialPropertyBlock._renderer, block.cellWithMaterialPropertyBlock._propBlock);
    }

    public void HighlightAndActiveCells(List<Block> blockList, Color color)
    {
        foreach (Block block in blockList)
        {
            HighlightAndActiveCell(block, color);
        }
    }

    public void DeactiveCells(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            block.cell.SetActive(false);
        }
    }

    public void HighlightAndActiveCell(Block block, Color color)
    {
        block.cell.SetActive(true);
        ChangeCellColor(color, block.cellWithMaterialPropertyBlock._renderer, block.cellWithMaterialPropertyBlock._propBlock);
    }

    public void HighlightCells(List<Block> blocks, Color color)
    {
        foreach (Block block in blocks)
        {
            HighlightAndActiveCell(block, color);
        }
    }

    public void PointerHighlight(Block block)
    {
        if (currentHighlightBlock != block)
        {
            currentHighlightBlock = block;
            HighlightAndActiveCell(block, colorPointer);
        }
    }
    public void RemoveHighlight(Color color)
    {
        if (currentHighlightBlock)
        {
            HighlightAndActiveCell(currentHighlightBlock, color);
            currentHighlightBlock = null;
        }
    }

    public void HighlightingCell(Block hitBlock, List<Block> blockList, Color color)
    {
        if (blockList.Contains(hitBlock)) // highlight hover spot
        {
            if (currentHighlightBlock) // highlight block exists
            {
                if (currentHighlightBlock != hitBlock)
                {
                    HighlightAndActiveCell(currentHighlightBlock, color);
                    PointerHighlight(hitBlock);
                }
            }
            else // no highlight block exists
            {
                PointerHighlight(hitBlock);
            }
        }
        else // we're interacting with an unavailable cell
        {
            RemoveHighlight(color);
        }
    }
    //-----------------------

    // PATHS
    public void ClearAllLists()
    {
        currentMovementBlockPaths.Clear();
        currentAvailableMovementBlocks.Clear();
        currentAvailableAttackBlocks.Clear();
        currentAvailableAreaAttackBlocks.Clear();
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
    //------------------------

    // BLOCKS
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

    //----------------------------------------
}
