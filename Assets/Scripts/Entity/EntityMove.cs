using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour
{
    [SerializeField]
    private float minDistanceToTarget = 0.1f;
    [SerializeField]
    private float movementSpeed = 1f;
    public bool Moving { get; private set; }

    public IEnumerator MoveEntity(Entity entity, Block newBlock, List<Block> path)
    {
        Moving = true;
        entity.currentMovementPoints -= path.Count;

        foreach (Block block in path)
        {
            Vector3 targetPosition = 
                new Vector3(block.transform.position.x, entity.transform.position.y, block.transform.position.z);
            Vector3 direction = targetPosition - entity.transform.position;
            float distanceToTarget = Mathf.Abs(Vector3.Distance(entity.transform.position, targetPosition));
            
            while (distanceToTarget > minDistanceToTarget)
            {
                entity.transform.Translate(direction.normalized * Time.deltaTime * movementSpeed);
                distanceToTarget = Mathf.Abs(Vector3.Distance(entity.transform.position, targetPosition));
                yield return null;
            }
            
            entity.transform.position = targetPosition;
        }

        Moving = false;
        yield return null;
    }
}
