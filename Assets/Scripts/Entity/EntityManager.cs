using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public void MoveEntity(Block currentBlock, Block newBlock, Entity entity, List<Block> path)
    {
        Vector3 newBlockPosition = newBlock.transform.position;
        entity.transform.position = new Vector3(newBlockPosition.x, entity.transform.position.y, newBlockPosition.z);
        entity.currentMovementPoints -= path.Count;
        
    }
}
