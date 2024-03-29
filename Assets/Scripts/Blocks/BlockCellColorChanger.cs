using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note to self: clean up script. messy and UGLY
public class BlockCellColorChanger : MonoBehaviour
{
    [field: SerializeField]
    public Color ColorMove { get; private set; }
    [field: SerializeField]
    public Color ColorPointer { get; private set; }
    [field: SerializeField]
    public Color ColorAttackRange { get; private set; }
    [field: SerializeField]
    public Color ColorAttackArea { get; private set; }

    private Block currentHighlightBlock;
    private List<Block> currentHighlightPath = new List<Block>();

    public void ChangeCellColor(Color color, Renderer renderer, MaterialPropertyBlock propBlock)
    {
        // get current value of material properties in renderer
        renderer.GetPropertyBlock(propBlock);
        // assign new value
        propBlock.SetColor("_Color", color);
        // apply edited value to renderer
        renderer.SetPropertyBlock(propBlock);
    }

    public void PointerHighlight(Block block)
    {
        if (currentHighlightBlock != block)
        {
            currentHighlightBlock = block;
            HighlightAndActiveCell(block, ColorPointer);
        }
    }

    public void RemoveHighlight(Color color = default)
    {
        if (currentHighlightBlock && color != default)
        {
            HighlightAndActiveCell(currentHighlightBlock, color);
            currentHighlightBlock = null;
        }
        else
        {
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

    public void HighlightPath(Block hitBlock, List<List<Block>> blockPaths)
    {
        if (currentHighlightBlock != hitBlock)
        {
            RemoveHighlightPath();
            currentHighlightBlock = hitBlock;
            foreach (List<Block> blockList in blockPaths)
            {
                if (blockList.Contains(hitBlock))
                {
                    int lastIndex = blockList.IndexOf(hitBlock) + 1;
                    currentHighlightPath =  blockList.GetRange(0, lastIndex);
                    foreach (Block block in currentHighlightPath)
                    {
                        HighlightCell(block, ColorPointer);
                    }
                    break;
                }
            }
        }
    }

    public void RemoveHighlightPath()
    {
        foreach (Block block in currentHighlightPath)
        {
            HighlightCell(block, ColorMove);
        }

        currentHighlightBlock = null;
    }

    public void HighlightCell(Block block, Color color)
    {
        ChangeCellColor(
            color,
            block.cellWithMaterialPropertyBlock._renderer,
            block.cellWithMaterialPropertyBlock._propBlock);
    }

    public void HighlightAndActiveCells(List<Block> blockList, Color color)
    {
        foreach (Block block in blockList)
        {
            HighlightAndActiveCell(block, color);
        }
    }

    public void HighlightCells(List<Block> blocks, Color color)
    {
        foreach (Block block in blocks)
        {
            HighlightAndActiveCell(block, color);
        }
    }

    public void HighlightAndActiveCell(Block block, Color color)
    {
        block.cell.SetActive(true);
        ChangeCellColor(
            color,
            block.cellWithMaterialPropertyBlock._renderer,
            block.cellWithMaterialPropertyBlock._propBlock);
    }
}
