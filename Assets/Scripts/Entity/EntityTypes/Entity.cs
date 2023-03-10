using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int movementPoints;
    [HideInInspector]
    public int currentMovementPoints;
    public int healthPoints;
    [HideInInspector]
    public int currentHealthPoints;

    public bool playerControlled;

    virtual protected void Awake()
    {
        currentMovementPoints = movementPoints;
        currentHealthPoints = healthPoints;
    }

    public void ResetValues()
    {
        currentMovementPoints = movementPoints;
    }
}
