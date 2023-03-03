using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [HideInInspector]
    public List<Entity> currentEntities = new List<Entity>();

    private GameState gameState;
    private EntityMove entityMove;
    private EntityAttack entityAttack;

    void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        entityMove = FindObjectOfType<EntityMove>();
    }

    public void MoveEntity(Entity entity, Block newBlock, List<Block> path)
    {
        StartCoroutine(entityMove.MoveEntity(entity, newBlock, path));
    }

    public Entity GetEntityAboveBlock(Block block)
    {
        RaycastHit hit;
        Entity entity = null;
        if (Physics.Raycast(entity.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            entity = hit.collider.GetComponentInParent<Entity>();
        }
        return entity;
    }
}
