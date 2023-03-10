using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [HideInInspector]
    public Transform cellAvailable;
    [HideInInspector]
    public Transform cellHighlight;
    [HideInInspector]
    public Vector2Int positionOnGrid = new Vector2Int();
    
    [SerializeField]
    private int movementCost;
    public int MovementCost { get{ return movementCost; } private set{ movementCost = value; } }
    [HideInInspector]
    public int currentMovementCost;

    void Awake()
    {
        cellAvailable = transform.GetChild(0);
        cellHighlight = transform.GetChild(1);
        currentMovementCost = MovementCost;
    }
}
