using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class BlockSpawner : MonoBehaviour
{
    // Since Unity doesn't show dictionaries in Inspector, create own Key,Value
    // KEY
    [SerializeField]
    private List<char> charForBlockTypes = new List<char>();
    // VALUE
    [SerializeField]
    private List<Block> blockTypes = new List<Block>();
    private Dictionary<char, Block> block = new Dictionary<char, Block>();

    private BlockManager blockManager;
    private FileManager fileManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        fileManager = FindObjectOfType<FileManager>();
    }

    void Start()
    {
        // Fill the block dictionary 
        for (int i = 0; i < blockTypes.Count; i++)
        {
            block.Add(charForBlockTypes[i], blockTypes[i]);
        }

        CreateMap(blockManager.BlockGrid, fileManager.FileText);
    }

    public void CreateMap(Block[,] grid, string[] mapText)
    {
        GameObject parent = new GameObject("MapParent");

        int xFileCount = 0;
        int yFileCount = 0;

        foreach (string line in mapText)
        {
            foreach (char c in line)
            {
                Block newBlock = Instantiate(block[c], new Vector3(xFileCount, 0, yFileCount), Quaternion.identity);
                newBlock.transform.SetParent(parent.transform);
                newBlock.name = newBlock.name + xFileCount + yFileCount;
                newBlock.positionOnGrid = new Vector2Int(xFileCount, yFileCount);
                grid[xFileCount, yFileCount] = newBlock;
                yFileCount++;
            }
            xFileCount++;
            yFileCount = 0;
        }
    }
}
