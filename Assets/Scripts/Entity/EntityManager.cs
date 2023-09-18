using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [HideInInspector]
    public List<Entity> currentEntities = new List<Entity>();

    public Entity FindEntityAboveBlock(Block block)
    {
        RaycastHit hit;
        Entity entity = null;
        if (Physics.Raycast(block.transform.position, Vector3.up, out hit, Mathf.Infinity))
        {
            entity = hit.collider.GetComponentInParent<Entity>();
        }
        return entity;
    }
}
