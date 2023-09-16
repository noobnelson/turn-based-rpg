using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static Block[,] BlockGrid;
    
    [SerializeField]
    private int blockLayer = 6;
    public int BlockLayerMask { get; private set; }
    public int MaxCost { get; private set; } = 99;

    [HideInInspector]
    public List<List<Block>> movementBlockPaths = new List<List<Block>>();
    [HideInInspector]
    public List<Block> availableMovementBlocks = new List<Block>();
    [HideInInspector]
    public List<Block> availableAttackBlocks = new List<Block>();
    [HideInInspector]
    public List<Block> availableAreaAttackBlocks = new List<Block>();

    void Awake()
    {
        BlockLayerMask = 1 << blockLayer;
    }

    public void ResetAllCells()
    {
        DeactiveCells(availableAttackBlocks);
        DeactiveCells(availableMovementBlocks);
        movementBlockPaths.Clear();
        availableMovementBlocks.Clear();
        availableAttackBlocks.Clear();
        availableAreaAttackBlocks.Clear();
    }

    public void DeactiveCells(List<Block> blockList)
    {
        foreach (Block block in blockList)
        {
            block.cell.SetActive(false);
        }
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
}
