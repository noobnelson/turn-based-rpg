using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour
{
    [SerializeField]
    private float distanceToMove = 0.1f;
    [SerializeField]
    private float movementSpeed = 1f;
    
    private GameState gameState;

    void Awake()
    {
        gameState = FindObjectOfType<GameState>();
    }

    public IEnumerator MoveEntity(Entity entity, Block newBlock, List<Block> path)
    {
        gameState.moving = true;
        entity.currentMovementPoints -= path.Count;
        foreach (Block block in path)
        {
            Vector3 targetPosition = new Vector3(block.transform.position.x, entity.transform.position.y, block.transform.position.z);
            Vector3 direction = targetPosition - entity.transform.position;
            while (Mathf.Abs(Vector3.Distance(entity.transform.position, targetPosition)) > distanceToMove)
            {
                entity.transform.Translate(direction.normalized * Time.deltaTime * movementSpeed);
                yield return null;
            }
            entity.transform.position = targetPosition;
        }
        gameState.moving = false;
        yield return null;
    }
}
