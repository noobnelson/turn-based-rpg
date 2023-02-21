using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public Entity occupantEntity;

    public Transform cellAvailable;
    public Transform cellUnvailable;

    [SerializeField]
    private int movementCost;
    public int MovementCost { get{ return movementCost; } private set{ movementCost = value; } }

    void Start()
    {
        cellAvailable = transform.GetChild(0);
        cellUnvailable = transform.GetChild(1);
    }
}
