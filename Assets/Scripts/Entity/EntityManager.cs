using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<Entity> entities = new List<Entity>();
    public List<Vector2> entitiesStartPosition = new List<Vector2>();
    public Vector3 yPositionOffset = new Vector3(0, 0.5f, 0);

    private BlockManager blockManager;
    void Start()
    {
        blockManager = FindObjectOfType<BlockManager>();

        for (int i = 0; i < entities.Count; i++)
        {
            int x = (int)entitiesStartPosition[i].x;
            int y = (int)entitiesStartPosition[i].y;
            Vector3 entityPosition = blockManager.BlockGrid[x,y].transform.position + yPositionOffset;
            Entity entity = Instantiate(entities[i], entityPosition, Quaternion.identity);
        }
    }
}
