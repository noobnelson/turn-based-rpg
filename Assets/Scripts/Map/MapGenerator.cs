using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapFile;
    private string mapFilePath;

    // Since Unity doesn't show dictionaries in Inspector, create own Key,Value
    // KEY
    [SerializeField]
    private List<char> charForBlockTypes = new List<char>();
    // VALUE
    [SerializeField]
    private List<Block> blockTypes = new List<Block>();
    private Dictionary<char, Block> block = new Dictionary<char, Block>();

    public Block[,] BlockGrid { get; private set; }
    public int[,] BlockGridCosts { get; private set; }

    void Start()
    {
        // Fill the block dictionary 
        for (int i = 0; i < blockTypes.Count; i++) 
        {
            block.Add(charForBlockTypes[i], blockTypes[i]);
        }

        // Use file to find dimensions ie. fileText array : ["000"]["000"]["000"] so 3x3 grid
        mapFilePath = AssetDatabase.GetAssetPath(mapFile);
        string[] fileText = File.ReadAllLines(mapFilePath);
        int xGridCount = fileText[0].Length;
        int zGridCount = fileText.Length;

        BlockGrid = new Block[xGridCount, zGridCount];
        BlockGridCosts = new int[xGridCount, zGridCount];

        CreateMap(BlockGrid);
    }

    public void CreateMap(Block[,] grid)
    {
        GameObject parent = new GameObject("MapParent");

        int xFileCount = 0;
        int zFileCount = 0;

        foreach (string line in File.ReadLines(mapFilePath))
        {
            foreach (char c in line)
            {
                Block newBlock = Instantiate(block[c], new Vector3(xFileCount, 0, zFileCount), Quaternion.identity);
                newBlock.transform.SetParent(parent.transform);
                newBlock.name = newBlock.name + xFileCount + zFileCount;
                grid[xFileCount, zFileCount] = newBlock;
                BlockGridCosts[xFileCount, zFileCount] = newBlock.MovementCost;

                zFileCount++;
            }
            xFileCount++;
            zFileCount = 0;
        }
    }
}
