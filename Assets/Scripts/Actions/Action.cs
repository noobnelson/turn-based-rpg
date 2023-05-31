using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    [field: SerializeField]
    public string ActionName { get; private set; }
    [field: SerializeField]
    public int CastRange { get; private set; }
    // [field: SerializeField]
    // public int AreaOfEffectRange { get; private set; }
    [field: SerializeField]
    public int ActionCost { get; private set; }
    [field: SerializeField]
    public List<Effect> EffectSelf { get; private set; } = new List<Effect>();
    [field: SerializeField]
    public List<Effect> EffectOther { get; private set; } = new List<Effect>();

    public void PerformActionSelf(Entity entity)
    {
        foreach (Effect effect in EffectSelf)
        {
            effect.ApplyEffect(entity);
        }
    }

    public void PerformActionOther(Entity entity)
    {
        foreach (Effect effect in EffectOther)
        {
            effect.ApplyEffect(entity);
        }
    }
}
