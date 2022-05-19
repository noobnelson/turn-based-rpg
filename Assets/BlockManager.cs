using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset mapFile;
    [SerializeField]
    private Block blockPrefab;
    private Block[,] blockGrid;

    private string mapFilePath;

    private int layerMask;
    private Camera cam;

    private Block currentHighlightedBlock;

    void Start()
    {
        mapFilePath = AssetDatabase.GetAssetPath(mapFile); 
        
        int zGridCount = File.ReadAllLines(mapFilePath).Length;
        int xGridCount = 0;
        foreach (string line in File.ReadLines(mapFilePath))
        {
            xGridCount = line.Length;
            break;
        }
        blockGrid = new Block[xGridCount, zGridCount];

        CreateMap(blockGrid, blockPrefab);

        cam = Camera.main;
        layerMask = 1 << 6;
        //layerMask = ~layerMask;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(mousePos);
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            Block block = hit.collider.GetComponentInParent<Block>();
            //Block block = 
            if (!currentHighlightedBlock)
            {
                currentHighlightedBlock = block;
                block.HighlightCell(true);
            }
            else if (block != currentHighlightedBlock)
            {
                currentHighlightedBlock.HighlightCell(false);
                block.HighlightCell(true);
                currentHighlightedBlock = block;
            }
        }
        else
        {
            if (currentHighlightedBlock)
            {
                currentHighlightedBlock.HighlightCell(false);
            }
            currentHighlightedBlock = null;
        }
    }

    public void CreateMap(Block[,] grid, Block blockPrefab)
    {   
        GameObject parent = new GameObject("MapParent");
        
        int xFileCount = 0;
        int zFileCount = 0;

        foreach (string line in File.ReadLines(mapFilePath))
        {
            foreach (char c in line)
            {
                int value = (int)Char.GetNumericValue(c);
                if (value == 1) // 1 = block, 0 = empty space
                {
                    Block block = Instantiate(blockPrefab, new Vector3(xFileCount, 0, zFileCount), Quaternion.identity);
                    block.transform.SetParent(parent.transform);
                    block.name = block.name + xFileCount + zFileCount;
                    grid[xFileCount, zFileCount] = block;
                }
                zFileCount++;
            }
            xFileCount++;
            zFileCount = 0;
        }
    }
}
