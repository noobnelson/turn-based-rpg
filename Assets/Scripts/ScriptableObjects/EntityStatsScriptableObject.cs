using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityStatsScriptableObject", order = 1)]
public class EntityStatsScriptableObject : ScriptableObject
{
    [SerializeField]
    private int maxHealth;
    internal int currentHealth;

    [SerializeField]
    private int movementPoints;
    internal int currentMovementPoints;

    [SerializeField]
    private int maxEnergy;
    internal int currentEnergy;

    [SerializeField]
    private int attackPower;
    internal int currentAttackPower;
}
