using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public string actionName;
    public int castRange;
    public int actionRange;
    public List<Effect> effectSelf = new List<Effect>();
    public List<Effect> effectOther = new List<Effect>();

    public void PerformActionSelf(Entity entity)
    {
        foreach (Effect effect in effectSelf)
        {
            effect.ApplyEffect(entity);
        }
    }

    public void PerformActionOther(Entity entity)
    {
        foreach (Effect effect in effectOther)
        {
            effect.ApplyEffect(entity);
        }
    }


}
