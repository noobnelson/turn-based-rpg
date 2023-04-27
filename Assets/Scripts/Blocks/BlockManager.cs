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

    public Color colorMove;
    public Color colorHighlight;
    public Color colorAttackRange;
    public Color colorAttackArea;

    // NOTE: add these to different class?
    public List<List<Block>> currentMovementBlockPaths = new List<List<Block>>();
    public List<Block> currentAvailableMovementBlocks = new List<Block>();
    public List<Block> currentAvailableAttackBlocks = new List<Block>();
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

    public void ChangeCellColor(Color color, Renderer renderer, MaterialPropertyBlock propBlock)
    {
        renderer.GetPropertyBlock(propBlock); // get current value of material properties in renderer
        propBlock.SetColor("_Color", color); // assign new value
        renderer.SetPropertyBlock(propBlock); // apply edited value to renderer
    }

    public void ResetPaths()
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

    public void HighlightAndActiveCells(List<Block> blockList, Color color)
    {
        foreach (Block block in blockList)
        {
            block.cell.SetActive(true);
            HighlightCell(block, color);
        }
    }

    public void DeactiveCells(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            block.cell.SetActive(false);
        }
    }

    public void HighlightCell(Block block, Color color)
    {
        block.cell.SetActive(true);
        ChangeCellColor(color, block.cellWithMaterialPropertyBlock._renderer, block.cellWithMaterialPropertyBlock._propBlock);
    }

    public void HighlightCells(List<Block> blocks, Color color)
    {
        foreach (Block block in blocks)
        {
            HighlightCell(block, color);
        }
    }
}
