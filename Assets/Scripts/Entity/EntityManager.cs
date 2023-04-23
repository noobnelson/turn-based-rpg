using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [HideInInspector]
    public List<Entity> currentEntities = new List<Entity>();

    private EntityMove entityMove;

    void Awake()
    {
        entityMove = FindObjectOfType<EntityMove>();
    }

    public void MoveEntity(Entity entity, Block newBlock, List<Block> path)
    {
        StartCoroutine(entityMove.MoveEntity(entity, newBlock, path));
    }

    public bool CheckMoving()
    {
        return entityMove.moving;
    }
        
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
