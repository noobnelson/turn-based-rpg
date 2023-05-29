using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int positionOnGrid = new Vector2Int();
    [SerializeField]
    private int movementCost;
    public int MovementCost { get{ return movementCost; } private set{ movementCost = value; } }
    [HideInInspector]
    public int currentMovementCost;
    public CellWithMaterialPropertyBlock cellWithMaterialPropertyBlock;
    public GameObject cell;

    void Awake()
    {
        cell = transform.GetChild(0).gameObject;
        currentMovementCost = MovementCost;
        cellWithMaterialPropertyBlock = cell.GetComponent<CellWithMaterialPropertyBlock>();
    }
}
