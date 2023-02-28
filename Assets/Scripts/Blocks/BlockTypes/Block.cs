using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public Transform cellAvailable;
    public Transform cellHighlight;
    public Vector2Int positionOnGrid = new Vector2Int();
    
    [SerializeField]
    private int movementCost;
    public int MovementCost { get{ return movementCost; } private set{ movementCost = value; } }
    public int currentMovementCost;

    void Awake()
    {
        cellAvailable = transform.GetChild(0);
        cellHighlight = transform.GetChild(1);
        currentMovementCost = MovementCost;
    }
}
