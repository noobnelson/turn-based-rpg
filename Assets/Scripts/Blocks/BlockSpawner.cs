using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Block> blockTypes = new List<Block>();
    private Dictionary<char, Block> block = new Dictionary<char, Block>();

    private FileManager fileManager;

    void Awake()
    {
        fileManager = FindObjectOfType<FileManager>();

        for (int i = 0; i < blockTypes.Count; i++)
        {
            block.Add(blockTypes[i].BlockTextFileChar, blockTypes[i]);
        }
        
        int xGridCount = fileManager.FileText[0].Length;
        int yGridCount = fileManager.FileText.Length;
        BlockManager.BlockGrid = new Block[xGridCount, yGridCount];

        // Create the map
        GameObject mapParent = new GameObject("MapParent");
        int xFileCount = 0;
        int yFileCount = 0;
        foreach (string line in fileManager.FileText)
        {
            foreach (char c in line)
            {
                Vector3 blockPosition = new Vector3(xFileCount, 0, yFileCount);
                Block newBlock = Instantiate(block[c], blockPosition, Quaternion.identity);
                newBlock.transform.SetParent(mapParent.transform);
                newBlock.name = newBlock.name + xFileCount + yFileCount;
                newBlock.positionOnGrid = new Vector2Int(xFileCount, yFileCount);
                BlockManager.BlockGrid[xFileCount, yFileCount] = newBlock;
                xFileCount++;
            }
            xFileCount = 0;
            yFileCount++;
        }
    }
}
