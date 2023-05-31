using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Heal", order = 2)]
public class EffectHeal : Effect
{
    public override void ApplyEffect(Entity entity)
    {
        entity.currentHealthPoints += Value;
    }
}
