using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    public int movementPoints;
    [HideInInspector]
    public int currentMovementPoints;
    public int maxEnergy;
    [HideInInspector]
    public int currentEnergy;
    public int attackPower;
    [HideInInspector]
    public int currentAttackPower;
    public int speed;
    [HideInInspector]
    public int currentSpeed;
    [HideInInspector]
    public Vector2Int positionOnGrid = new Vector2Int();

    public bool playerControlled;

    virtual protected void Awake()
    {
        currentHealth = maxHealth;
        currentMovementPoints = movementPoints;
        currentEnergy = maxEnergy;
        currentAttackPower = attackPower;
        currentSpeed = speed;
    }

    public void ResetValues()
    {
        currentMovementPoints = movementPoints;
    }
}
