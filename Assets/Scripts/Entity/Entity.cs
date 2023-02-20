using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    internal EntityStatsScriptableObject entityStats;
    
    void Start()
    {
        
    }
}
