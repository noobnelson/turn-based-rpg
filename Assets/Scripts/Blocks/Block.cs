using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int positionOnGrid = new Vector2Int();
    [field: SerializeField]
    public int MovementCost { get; private set; }
    [HideInInspector]
    public int currentMovementCost;
    [field: SerializeField]
    public char BlockTextFileChar { get; private set; }

    public CellWithMaterialPropertyBlock cellWithMaterialPropertyBlock;
    public GameObject cell;

    void Awake()
    {
        cell = transform.GetChild(0).gameObject;
        currentMovementCost = MovementCost;
        cellWithMaterialPropertyBlock = cell.GetComponent<CellWithMaterialPropertyBlock>();
    }
}
