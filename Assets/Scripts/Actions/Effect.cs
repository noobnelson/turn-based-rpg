using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [field: SerializeField]
    public int Value { get; private set; }

    public abstract void ApplyEffect(Entity target);
}
