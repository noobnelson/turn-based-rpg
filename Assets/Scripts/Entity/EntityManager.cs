using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<Entity> entities = new List<Entity>();

    void Start()
    {
        Entity[] entities = FindObjectsOfType<Entity>();
        this.entities.AddRange(entities);
    }
}
