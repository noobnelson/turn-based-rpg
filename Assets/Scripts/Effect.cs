using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int value;

    public abstract void ApplyEffect(Entity target);
}
