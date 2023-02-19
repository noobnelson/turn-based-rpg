using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    

    private Transform cellAvailable;
    private Transform cellUnvailable;

    [SerializeField]
    private int movementCost;
    public int MovementCost { get{ return movementCost; } private set{ movementCost = value; } }

    void Start()
    {
        cellAvailable = transform.GetChild(0);
        cellUnvailable = transform.GetChild(1);
    }

    public void HighlightCellAvailable(bool b)
    {
        cellAvailable.gameObject.SetActive(b);
    }

    public void HighlightCellUnavailable(bool b)
    {
        cellUnvailable.gameObject.SetActive(b);
    }
}
