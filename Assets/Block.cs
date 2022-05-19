using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int cost = 1;
    [SerializeField]
    private Transform cell;

    public void HighlightCell(bool b)
    {
        cell.gameObject.SetActive(b);
    }
}
