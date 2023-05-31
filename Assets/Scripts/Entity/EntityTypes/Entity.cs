using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [field: SerializeField]
    public int MovementPoints { get; private set; }
    [HideInInspector]
    public int currentMovementPoints;
    [field: SerializeField]
    public int HealthPoints { get; private set; }
    [HideInInspector]
    public int currentHealthPoints;
    [field: SerializeField]
    public int ActionPoints { get; private set; }
    [HideInInspector]
    public int currentActionPoints;

    [field: SerializeField]
    public List<Action> ActionList { get; private set; } = new List<Action>();
    [field: SerializeField]
    public bool PlayerControlled { get; private set; }

    virtual protected void Awake()
    {
        currentMovementPoints = MovementPoints;
        currentHealthPoints = HealthPoints;
        currentActionPoints = ActionPoints;
    }

    public void ResetValues()
    {
        currentMovementPoints = MovementPoints;
        currentActionPoints = ActionPoints;
    }
}
