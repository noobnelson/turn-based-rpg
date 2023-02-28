using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [HideInInspector]
    public List<Entity> currentEntities = new List<Entity>();

    public void MoveEntity(Entity entity, Block newBlock, List<Block> path)
    {
        Vector3 newBlockPosition = newBlock.transform.position;
        entity.transform.position = new Vector3(newBlockPosition.x, entity.transform.position.y, newBlockPosition.z);
        entity.currentMovementPoints -= path.Count;
    }

    public Block GetBlockBelowEntity(Entity entity)
    {
        RaycastHit hit;
        Block block = null;
        if (Physics.Raycast(entity.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            block = hit.collider.GetComponentInParent<Block>();
        }
        return block;
    }
}
