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
    [SerializeField]
    private Block[,] blockGrid;

    [SerializeField]
    private List<Block> blockTypes = new List<Block>();
    [SerializeField]
    private List<char> charForBlockTypes = new List<char>();

    private Dictionary<char, Block> block = new Dictionary<char, Block>();

    private string mapFilePath;

    void Start()
    {
        for (int i = 0; i < blockTypes.Count; i++)
        {
            block.Add(charForBlockTypes[i], blockTypes[i]);
        }

        mapFilePath = AssetDatabase.GetAssetPath(mapFile);

        int zGridCount = File.ReadAllLines(mapFilePath).Length;
        int xGridCount = 0;
        foreach (string line in File.ReadLines(mapFilePath))
        {
            xGridCount = line.Length;
            break;
        }
        blockGrid = new Block[xGridCount, zGridCount];

        CreateMap(blockGrid);
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

                zFileCount++;
            }
            xFileCount++;
            zFileCount = 0;
        }
    }
}
