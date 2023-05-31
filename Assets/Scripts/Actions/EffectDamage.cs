using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Damage", order = 1)]
public class EffectDamage : Effect
{
    public override void ApplyEffect(Entity entity)
    {
        entity.currentHealthPoints -= Value;
    }
}
