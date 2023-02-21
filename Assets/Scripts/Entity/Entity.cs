using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    public int CurrentHealth { get; private set; }
    [SerializeField]
    private int movementPoints;
    public int CurrentMovementPoints { get; private set; }
    [SerializeField]
    private int maxEnergy;
    public int CurrentEnergy { get; private set; }
    [SerializeField]
    private int attackPower;
    public int CurrentAttackPower { get; private set; }

    void Start()
    {
        CurrentHealth = maxHealth;
        CurrentMovementPoints = movementPoints;
        CurrentEnergy = maxEnergy;
        CurrentAttackPower = attackPower;
    }
}
