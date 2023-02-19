using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    internal EntityStatsScriptableObject entityStats;

    void Start()
    {
        entityStats = GetComponent<EntityStatsScriptableObject>();
    }
}
