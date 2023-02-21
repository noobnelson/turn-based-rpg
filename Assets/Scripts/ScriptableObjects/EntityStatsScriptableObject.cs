using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityStatsScriptableObject", order = 1)]
public class EntityStatsScriptableObject : ScriptableObject
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

}
