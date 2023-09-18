using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridVisualiser : MonoBehaviour
{
    public GameObject gridCanvas;
    public TMP_Text gridPositionTextPrefab;
    public float textPresetOffsetY;
    public LineRenderer gridLinePrefab;
    public Transform gridLinesParent;

    void Start()
    {
        // Grid lines
        int rowLength = BlockManager.BlockGrid.GetLength(0);
        int colLength = BlockManager.BlockGrid.GetLength(1);

        for (int i = 0; i < rowLength + 1; i++)
        {
            LineRenderer newLine = Instantiate(gridLinePrefab);
            newLine.transform.SetParent(gridLinesParent);
            Vector3 lineStart = new Vector3(i, 0, 0);
            Vector3 lineEnd = new Vector3(i, 0, colLength);

            newLine.SetPosition(0, lineStart);
            newLine.SetPosition(1, lineEnd);
            newLine.transform.position = new Vector3(-0.5f, 0.5f, -0.5f);
        }

        for (int j = 0; j < colLength + 1; j++)
        {
            LineRenderer newLine = Instantiate(gridLinePrefab);
            newLine.transform.SetParent(gridLinesParent);

            Vector3 lineStart = new Vector3(0, 0, j);
            Vector3 lineEnd = new Vector3(rowLength, 0, j);
            newLine.SetPosition(0, lineStart);
            newLine.SetPosition(1, lineEnd);
            newLine.transform.position = new Vector3(-0.5f, 0.5f, -0.5f);
        }

        // Grid position text
        foreach (Block block in BlockManager.BlockGrid)
        {
            TMP_Text gridPositionText = Instantiate(
                gridPositionTextPrefab,
                new Vector3(block.transform.position.x, textPresetOffsetY, block.transform.position.z),
                Quaternion.identity,
                gridCanvas.transform);
            gridPositionText.transform.Rotate(new Vector3(90, 0, 0));

            gridPositionText.text = block.transform.position.x + ", " + block.transform.position.z;
        }


    }
}
