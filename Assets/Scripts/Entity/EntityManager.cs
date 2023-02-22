using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<Entity> entities = new List<Entity>();
    [SerializeField]
    private List<Vector2Int> entitiesStartPosition = new List<Vector2Int>();
    public List<Vector2Int> entitiesCurrentPosition = new List<Vector2Int>();
    [SerializeField]
    private Vector3 yPositionOffset = new Vector3(0, 0.5f, 0);

    private BlockManager blockManager;

    void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
    }

    void Start()
    {
        entitiesCurrentPosition = entitiesStartPosition;

        for (int i = 0; i < entities.Count; i++)
        {
            int x = entitiesStartPosition[i].x;
            int y = entitiesStartPosition[i].y;
            Block selectedBlock = blockManager.blockGrid[x, y];

            Vector3 entityPosition = selectedBlock.transform.position + yPositionOffset;
            Entity entity = Instantiate(entities[i], entityPosition, Quaternion.identity);
            blockManager.AddEntity(selectedBlock, entity);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            blockManager.AvailableMoves(entitiesCurrentPosition[0], 3);
        }
    }
}
