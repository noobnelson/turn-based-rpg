using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[,] BlockGrid { get; internal set; }
    public int[,] BlockGridCosts { get; internal set; }
}
